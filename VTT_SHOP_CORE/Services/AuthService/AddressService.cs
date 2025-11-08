using System.Threading.Tasks;
using AutoMapper;
using FluentResults;
using Microsoft.AspNetCore.Http.HttpResults;
using VTT_SHOP_CORE.Errors;
using VTT_SHOP_DATABASE.Entities;
using VTT_SHOP_DATABASE.Repositories;
using VTT_SHOP_SHARED.DTOs;
using VTT_SHOP_SHARED.Interfaces.UnitOfWork;
using VTT_SHOP_SHARED.Services;

namespace VTT_SHOP_CORE.Services.AuthService
{
    public class AddressService : ServiceBase<Address>
    {
        private readonly AddressRepository _address;
        private readonly UserRepository _user;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AddressService(AddressRepository address, UserRepository user, IMapper mapper, IUnitOfWork unitOfWork) : base(address)
        {
            _address = address;
            _user = user;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<AddressDTO>>> GetAddressByUserIdAsync(long userId)
        {
            var existUser = await _user.GetByIdAsync(userId);
            if (existUser == null)
            {
                return Result.Fail(new NotFoundError($"User not found with id {userId}"));
            }
            var addresses = await _address.GetAdressByUserIdAsync(userId);
            return Result.Ok(_mapper.Map<List<AddressDTO>>(addresses));

        }

        public async Task<Result<AddressDTO>> GetAddressById(long id)
        {
            var address = await _address.GetByIdAsync(id);
            if (address == null)
            {
                return Result.Fail(new NotFoundError($"Address not found with Id {id}"));
            }
            return Result.Ok(_mapper.Map<AddressDTO>(address));
        }

        public async Task<Result<AddressDTO>> AddAddressAsync(AddressDTO addressDTO, long userId)
        {
            var existUser = await _user.GetByIdAsync(userId);
            if (existUser == null)
            {
                return Result.Fail(new NotFoundError($"User not found with id {userId}"));
            }
            var newAddress = _mapper.Map<Address>(addressDTO);
            newAddress.UserId = userId;
            if (newAddress.IsDefault)
            {
                await _address.UnsetDefaultAddressesAsync(userId);
            }
            var address = await _address.AddressAsync(newAddress);
            await _unitOfWork.SaveChangesAsync();
            return Result.Ok(_mapper.Map<AddressDTO>(address));

        }

        public async Task<Result<AddressDTO>> UpdateAddress(AddressDTO addressDTO)
        {
            var existAddress = await _address.GetByIdAsync(addressDTO.Id);
            if (existAddress == null)
            {
                return Result.Fail(new NotFoundError($"Address not found with Id {addressDTO.Id}"));
            }
            _mapper.Map(addressDTO, existAddress);
            if (addressDTO.IsDefault)
            {
                await _address.UnsetDefaultAddressesAsync(existAddress.UserId);
            }
            await _unitOfWork.SaveChangesAsync();
            return Result.Ok(_mapper.Map<AddressDTO>(existAddress));
        }
        public async Task<Result> DeleteAddress(long id)
        {
            var existAddress = await _address.GetByIdAsync(id);
            if (existAddress == null)
            {
                return Result.Fail(new NotFoundError($"Address not found with Id {id}"));
            }
            _address.DeleteAddress(existAddress);
            await _unitOfWork.SaveChangesAsync();
            return Result.Ok();

        }
    }
}
