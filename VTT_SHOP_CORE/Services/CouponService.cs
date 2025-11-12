using System.Threading.Tasks;
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
    public class CouponService : ServiceBase<Coupon>
    {
        private readonly CouponRepository _coupon;
        private readonly CouponUsageRepository _couponUsage;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CouponService(CouponRepository coupon, CouponUsageRepository couponUsage, IMapper mapper, IUnitOfWork unitOfWork) : base(coupon) 
        {
            _coupon = coupon;
            _couponUsage = couponUsage;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CouponDetailDTO>> GetCouponByIdAsync(long Id)
        {
            var coupon = await base.GetByIdAsync(Id);
            if (coupon == null)
            {
                return Result.Fail(new NotFoundError($"No coupon found with Id {Id}"));
            }
            return Result.Ok(_mapper.Map<CouponDetailDTO>(coupon));
        }
    }
}
