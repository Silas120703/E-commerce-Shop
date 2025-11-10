using AutoMapper;
using FluentResults;
using VTT_SHOP_DATABASE.Entities;
using VTT_SHOP_DATABASE.Repositories;
using VTT_SHOP_SHARED.Interfaces.UnitOfWork;
using VTT_SHOP_SHARED.Services;

namespace VTT_SHOP_CORE.Services
{
    public class OrderService : ServiceBase<Order>
    {
        private readonly OrderRepository _order;
        private readonly OrderItemRepository _orderItem;
        private readonly UserRepository _user;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(OrderRepository order, OrderItemRepository orderItem, UserRepository user, IMapper mapper, IUnitOfWork unitOfWork) : base(order)
        {
            _order = order;
            _orderItem = orderItem;
            _user = user;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<>>
    }
}
