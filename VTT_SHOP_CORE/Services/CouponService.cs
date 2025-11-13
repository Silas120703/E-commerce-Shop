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

        public async Task<Result<CouponDetailDTO>> GetCouponByCodeAsync(string code)
        {
            var coupon = await _coupon.GetByCodeAsync(code);
            if (coupon == null)
            {
                return Result.Fail(new NotFoundError($"No coupon found with code {code}"));
            }
            return Result.Ok(_mapper.Map<CouponDetailDTO>(coupon));
        }

        public async Task<Result<CouponDetailDTO>> CreateCouponAsync(CouponDTO couponDto)
        {
            var coupon = _mapper.Map<Coupon>(couponDto);
            coupon.IsActive = true;
            var createdCoupon = await _coupon.CreateCoupon(coupon);
            await _unitOfWork.SaveChangesAsync();
            return Result.Ok(_mapper.Map<CouponDetailDTO>(createdCoupon));
        }

        public async Task<Result<CouponDetailDTO>> UpdateCouponAsync(long id, CouponDTO couponDto)
        {
            var existingCoupon = await base.GetByIdAsync(id);
            if (existingCoupon == null)
            {
                return Result.Fail(new NotFoundError($"No coupon found with Id {id}"));
            }
            _mapper.Map(couponDto, existingCoupon);
            var updatedCoupon = _coupon.UpdateCoupon(existingCoupon);
            await _unitOfWork.SaveChangesAsync();
            return Result.Ok(_mapper.Map<CouponDetailDTO>(updatedCoupon));
        }

        public async Task<Result> ActiveCouponAsync(long couponId)
        {
            var coupon = await base.GetByIdAsync(couponId);
            if (coupon == null)
            {
                return Result.Fail(new NotFoundError($"No coupon found with Id {couponId}"));
            }
            coupon.IsActive = true;
            _coupon.UpdateCoupon(coupon);
            await _unitOfWork.SaveChangesAsync();
            return Result.Ok();
        }

        public async Task<Result> DeactiveCouponAsync(long couponId)
        {
            var coupon = await base.GetByIdAsync(couponId);
            if (coupon == null)
            {
                return Result.Fail(new NotFoundError($"No coupon found with Id {couponId}"));
            }
            coupon.IsActive = false;
            _coupon.UpdateCoupon(coupon);
            await _unitOfWork.SaveChangesAsync();
            return Result.Ok();
        }
    }
}
