using AutoMapper;
using FluentResults;
using VTT_SHOP_CORE.Errors;
using VTT_SHOP_DATABASE.Entities;
using VTT_SHOP_DATABASE.Repositories;
using VTT_SHOP_SHARED.DTOs;
using VTT_SHOP_SHARED.Interfaces.UnitOfWork;
using VTT_SHOP_SHARED.Services;

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
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(OrderRepository order,
            OrderItemRepository orderItem,
            UserRepository user,
            AddressRepository address,
            CartRepository cart,
            CartItemRepository cartItem,
            ProductRepository product,
            CouponRepository coupon, 
            CouponUsageRepository couponUsage, 
            IMapper mapper,
            IUnitOfWork unitOfWork) : base(order)
        {
            _order = order;
            _orderItem = orderItem;
            _user = user;
            _address = address;
            _cart = cart;
            _cartItem = cartItem;
            _product = product;
            _coupon = coupon; // Thêm
            _couponUsage = couponUsage; // Thêm
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<OrderDetailDTO>> CreateOrderFromCartAsync(long userId, CreateOrderDTO createOrderDto)
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
                long couponId = 0;
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
                    }
                    couponId = coupon.Id;
                    _coupon.IncrementUsageCount(coupon);
                }
                
                
                var oderItems = _mapper.Map<List<OrderItem>>(cartItems);
                var order = new Order
                {
                    UserId = userId,
                    ShippingAddressId = address.Id,
                    Status = "Pending",
                    TotalAmount = totalAmount,
                    DiscountAmount = discountAmount,
                    FinalAmount = totalAmount - discountAmount,
                    ShippingAddress = address,
                    Items = oderItems
                };
                await _order.AddAsync(order);
                await _unitOfWork.SaveChangesAsync();
                if (couponId != 0)
                {
                    var couponUsage = new CouponUsage
                    {
                        CouponId = couponId,
                        UserId = userId,
                        OrderId = order.Id
                    };
                    await _couponUsage.AddAsync(couponUsage);
                    await _unitOfWork.SaveChangesAsync();
                }
                _cartItem.DeleteCartItemRange(cartItems);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
                return Result.Ok(_mapper.Map<OrderDetailDTO>(order));
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return Result.Fail($"An error occurred while creating the order {ex.Message}.");
            }
        }

        public async Task<Result<OrderDetailDTO>> CreateOrderFromProductAsync(long userId, OrderItemCreateDTO orderItemCreateDTO, CreateOrderDTO createOrderDto)
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
                long couponId = 0;
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
                    }
                    couponId = coupon.Id;
                    _coupon.IncrementUsageCount(coupon);
                }
                var newOrder = new Order()
                {
                    UserId = userId,
                    Status = "Pending",
                    TotalAmount = totalAmount,
                    DiscountAmount = discountAmount,
                    FinalAmount = totalAmount - discountAmount,
                    ShippingAddressId = createOrderDto.ShippingAddressId
                };
                await _order.AddAsync(newOrder);
                await _unitOfWork.SaveChangesAsync();
                oderItem.OrderId = newOrder.Id;
                await _orderItem.AddAsync(oderItem);
                await _unitOfWork.SaveChangesAsync();

                if(couponId != 0)
                {
                    var couponUsage = new CouponUsage
                    {
                        CouponId = couponId,
                        UserId = userId,
                        OrderId = newOrder.Id
                    };
                    await _couponUsage.AddAsync(couponUsage);
                    await _unitOfWork.SaveChangesAsync();
                    
                }
                await _unitOfWork.CommitAsync();
                return Result.Ok(_mapper.Map<OrderDetailDTO>(newOrder));
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

