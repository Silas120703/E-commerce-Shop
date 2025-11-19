using AutoMapper;
using FluentResults;
using VTT_SHOP_CORE.Errors;
using VTT_SHOP_DATABASE.Entities;
using VTT_SHOP_DATABASE.Repositories;
using VTT_SHOP_SHARED.DTOs;
using VTT_SHOP_SHARED.Interfaces.UnitOfWork;
using VTT_SHOP_SHARED.Services;
using System.Collections.Generic; // Thêm using này
using System.Linq;
using Microsoft.EntityFrameworkCore;
using VTT_SHOP_SHARED.Extensions; // Thêm using này

namespace VTT_SHOP_CORE.Services
{
    public class OrderService : ServiceBase<Order>
    {
        private readonly OrderRepository _order;
        private readonly OrderItemRepository _orderItem;
        private readonly UserRepository _user;
        private readonly AddressRepository _address;
        private readonly CartRepository _cart;
        private readonly CartItemRepository _cartItem;
        private readonly ProductRepository _product;
        private readonly CouponRepository _coupon;
        private readonly CouponUsageRepository _couponUsage;
        private readonly PaymentRepository _payment;
        private readonly ShipmentRepository _shipment;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly VnPayService _vnPayService;
        public OrderService(OrderRepository order,
            OrderItemRepository orderItem,
            UserRepository user,
            AddressRepository address,
            CartRepository cart,
            CartItemRepository cartItem,
            ProductRepository product,
            CouponRepository coupon,
            CouponUsageRepository couponUsage,
            PaymentRepository payment,
            ShipmentRepository shipment,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            VnPayService vnPayService) : base(order) 
        {
            _order = order;
            _orderItem = orderItem;
            _user = user;
            _address = address;
            _cart = cart;
            _cartItem = cartItem;
            _product = product;
            _coupon = coupon;
            _couponUsage = couponUsage;
            _payment = payment;
            _shipment = shipment;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _vnPayService = vnPayService; 
        }

        public async Task<Result<PagedResult<OrderDetailDTO>>> GetUserOrdersPagedAsync(long userId, PagingParams pagingParams)
        {
            var user = await _user.GetByIdAsync(userId);
            if (user == null)
            {
                return Result.Fail(new NotFoundError($"User not found with Id {userId}"));
            }

            var query = _order.GetAll()
                .Include(o => o.Items).ThenInclude(oi => oi.Product) 
                .Include(o => o.Payment)
                .Include(o => o.Shipment)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreateAt);

            var pagedEntities = await query.ToPagedListAsync(pagingParams.PageIndex, pagingParams.PageSize);

            var orderDtos = _mapper.Map<List<OrderDetailDTO>>(pagedEntities.Items);

            var result = new PagedResult<OrderDetailDTO>(
                orderDtos,
                pagedEntities.TotalCount,
                pagedEntities.PageIndex,
                pagedEntities.PageSize
            );

            return Result.Ok(result);
        }

        public async Task<Result<CreateOrderResponseDTO>> CreateOrderFromCartAsync(long userId, CreateOrderDTO createOrderDto, string ipAddress)
        {
            var user = await _user.GetByIdAsync(userId);
            if (user == null)
            {
                return Result.Fail(new NotFoundError($"User not found with Id {userId}"));
            }
            var cart = await _cart.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                return Result.Fail(new NotFoundError("Cart not found for the user."));
            }
            var cartItems = await _cartItem.GetCartItemsAndProductByCartIdAsync(cart.Id);
            if (cartItems == null || !cartItems.Any())
            {
                return Result.Fail(new NotFoundError("No items in the cart to create an order."));
            }
            var address = await _address.GetByIdAsync(createOrderDto.ShippingAddressId);
            if (address == null || address.UserId != userId)
            {
                return Result.Fail(new NotFoundError("Shipping address not found or does not belong to the user."));
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                decimal totalAmount = cartItems.Sum(item => item.Quantity * (item.Product?.Price ?? 0));
                decimal discountAmount = 0;
                long? couponId = null; 
                if (createOrderDto.CouponCode != null)
                {
                    var coupon = await _coupon.GetByCodeAsync(createOrderDto.CouponCode);
                    if (coupon == null)
                    {
                        return Result.Fail(new NotFoundError("Coupon not found or inactive."));
                    }
                    if (await _coupon.IsEligibleAsync(coupon.Id, totalAmount))
                    {
                        if (coupon.DiscountType == "Percentage")
                        {
                            discountAmount = totalAmount * (coupon.DiscountValue / 100);
                            if (discountAmount > coupon.MaxDiscountValue)
                            {
                                discountAmount = coupon.MaxDiscountValue;
                            }
                        }
                        else
                        {
                            discountAmount = coupon.DiscountValue;
                        }
                        couponId = coupon.Id;
                        _coupon.IncrementUsageCount(coupon);
                    }
                }
                decimal shippingFee = 30000;
                decimal finalAmount = totalAmount - discountAmount + shippingFee;

                var oderItems = _mapper.Map<List<OrderItem>>(cartItems);
                var order = new Order
                {
                    UserId = userId,
                    ShippingAddressId = address.Id,
                    Status = "Pending",
                    TotalAmount = totalAmount,
                    DiscountAmount = discountAmount,
                    ShippingFee = shippingFee,
                    FinalAmount = finalAmount,
                    ShippingAddress = address,
                    Items = oderItems
                };
                await _order.AddAsync(order);
                await _unitOfWork.SaveChangesAsync(); 

                var payment = new Payment
                {
                    OrderId = order.Id,
                    Amount = finalAmount,
                    PaymentMethod = createOrderDto.PaymentMethod,
                    Status = "pending",
                    TransactionDate = null
                };
                await _payment.AddAsync(payment);

                var shipment = new Shipment
                {
                    OrderId = order.Id,
                    Carrier = "Pending",
                    Status = "PendingPreparation",
                };
                await _shipment.AddAsync(shipment);

                if (couponId.HasValue) 
                {
                    var couponUsage = new CouponUsage
                    {
                        CouponId = couponId, 
                        UserId = userId,
                        OrderId = order.Id
                    };
                    await _couponUsage.AddAsync(couponUsage);
                }
                _cartItem.DeleteCartItemRange(cartItems);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();

                string? paymentUrl = null;
                if (createOrderDto.PaymentMethod == "VNPay")
                {
                    paymentUrl = await _vnPayService.CreatePaymentUrl(order.Id, ipAddress);
                }

                var responseDto = new CreateOrderResponseDTO
                {
                    OrderDetail = _mapper.Map<OrderDetailDTO>(order),
                    PaymentUrl = paymentUrl
                };

                return Result.Ok(responseDto);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return Result.Fail($"An error occurred while creating the order {ex.Message}.");
            }
        }

        public async Task<Result<CreateOrderResponseDTO>> CreateOrderFromProductAsync(long userId, OrderItemCreateDTO orderItemCreateDTO, CreateOrderDTO createOrderDto, string ipAddress)
        {
            var user = await _user.GetUserByIdAsync(userId);
            if (user == null)
            {
                return Result.Fail(new NotFoundError($"User not found with Id {userId}"));
            }
            var product = await _product.GetProductByIdAsync(orderItemCreateDTO.ProductId);
            if (product == null)
            {
                return Result.Fail(new NotFoundError($"Not found product with Id {orderItemCreateDTO.ProductId}"));
            }

            var address = await _address.GetByIdAsync(createOrderDto.ShippingAddressId);
            if (address == null || address.UserId != userId)
            {
                return Result.Fail(new NotFoundError("Shipping address not found or does not belong to the user."));
            }
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var oderItem = _mapper.Map<OrderItem>(orderItemCreateDTO);
                decimal totalAmount = orderItemCreateDTO.Quantity * orderItemCreateDTO.Price;
                decimal discountAmount = 0;
                long? couponId = null; 
                if (createOrderDto.CouponCode != null)
                {
                    var coupon = await _coupon.GetByCodeAsync(createOrderDto.CouponCode);
                    if (coupon == null)
                    {
                        return Result.Fail(new NotFoundError("Coupon not found or inactive."));
                    }
                    if (await _coupon.IsEligibleAsync(coupon.Id, totalAmount))
                    {
                        if (coupon.DiscountType == "Percentage")
                        {
                            discountAmount = totalAmount * (coupon.DiscountValue / 100);
                            if (discountAmount > coupon.MaxDiscountValue)
                            {
                                discountAmount = coupon.MaxDiscountValue;
                            }
                        }
                        else
                        {
                            discountAmount = coupon.DiscountValue;
                        }
                        couponId = coupon.Id;
                        _coupon.IncrementUsageCount(coupon);
                    }
                }
                decimal shippingFee = 30000;
                decimal finalAmount = totalAmount - discountAmount + shippingFee;

                var newOrder = new Order()
                {
                    UserId = userId,
                    Status = "Pending",
                    TotalAmount = totalAmount,
                    DiscountAmount = discountAmount,
                    ShippingFee = shippingFee,
                    FinalAmount = finalAmount,
                    ShippingAddressId = createOrderDto.ShippingAddressId,
                    ShippingAddress = address
                };
                await _order.AddAsync(newOrder);
                await _unitOfWork.SaveChangesAsync(); 

                oderItem.OrderId = newOrder.Id;
                await _orderItem.AddAsync(oderItem);

                newOrder.Items = new List<OrderItem> { oderItem };

                var payment = new Payment
                {
                    OrderId = newOrder.Id,
                    Amount = finalAmount,
                    PaymentMethod = createOrderDto.PaymentMethod,
                    Status = "pending",
                    TransactionDate = null
                };
                await _payment.AddAsync(payment);

                var shipment = new Shipment
                {
                    OrderId = newOrder.Id,
                    Carrier = "Pending",
                    Status = "PendingPreparation",
                };
                await _shipment.AddAsync(shipment);

                if (couponId.HasValue) 
                {
                    var couponUsage = new CouponUsage
                    {
                        CouponId = couponId,
                        UserId = userId,
                        OrderId = newOrder.Id
                    };
                    await _couponUsage.AddAsync(couponUsage);
                }

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();

                string? paymentUrl = null;
                if (createOrderDto.PaymentMethod == "VNPay")
                {
                    paymentUrl = await _vnPayService.CreatePaymentUrl(newOrder.Id, ipAddress);
                }

                var responseDto = new CreateOrderResponseDTO
                {
                    OrderDetail = _mapper.Map<OrderDetailDTO>(newOrder),
                    PaymentUrl = paymentUrl
                };

                return Result.Ok(responseDto);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return Result.Fail($"An error occurred while creating the order {ex.Message}.");
            }
        }

        public async Task<Result<List<OrderDetailDTO>>> GetOrdersByUserIdAsync(long userId)
        {
            var user = await _user.GetUserByIdAsync(userId);
            if (user == null)
            {
                return Result.Fail(new NotFoundError($"User not found with Id {userId}"));
            }
            var orders = await _order.GetOrdersByUserIdAsync(userId);
            var orderDetails = _mapper.Map<List<OrderDetailDTO>>(orders);
            return Result.Ok(orderDetails);
        }
    }
}