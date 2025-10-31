using System.Security.Claims;
using AutoMapper;
using FluentResults;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using VTT_SHOP_CORE.DTOs;
using VTT_SHOP_DATABASE.Entities;
using VTT_SHOP_DATABASE.Repositories;
using VTT_SHOP_SHARED.Services;
using VTT_SHOP_SHARED.Interfaces.UnitOfWork;
using VTT_SHOP_CORE.Errors;

namespace VTT_SHOP_CORE.Services.AuthService
{
    public class UserService : ServiceBase<User>
    {
        private readonly UserRepository _user;
        private readonly EmailService _email;
        private readonly JWTService _jwt;
        private readonly EmailVerificationRepository _verify;
        private readonly IMapper _mapper;
        private readonly UserRoleRepository _userRole;
        private readonly RoleRepository _role;
        private readonly RefreshTokenRepository _refresh;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(UserRepository user, EmailVerificationRepository verify, UserRoleRepository userRole, RoleRepository role,RefreshTokenRepository refresh, EmailService email, JWTService jwt, IMapper mapper, IUnitOfWork unitOfWork) : base(user)
        {
            _user = user;
            _email = email;
            _jwt = jwt;
            _verify = verify;
            _mapper = mapper;
            _userRole = userRole;
            _role = role;
            _refresh = refresh;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<UserDTO>> Register(UserCreateDTO user)
        {
            var errors = new List<IError>();
            if (await _user.GetUserByEmail(user.Email) != null)
            {
                errors.Add(new Error("Email already exists"));
            }
            if (await _user.GetUserByPhone(user.Phone) != null)
            {
                errors.Add(new Error("Phone number already exists"));
            }
            if(user.Password.Length < 8)
            {
                errors.Add(new Error("Password must be at least 8 characters long"));
            }
            if (errors.Any())
            {
                return Result.Fail(errors);
            }
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var newUser = _mapper.Map<User>(user);
                newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
                await _user.AddUserAsync(newUser);
                var role = await _role.GetRoleByName("User");
                var newUserRole = new UserRole()
                {
                    RoleId = role.Id,
                    User = newUser 
                };
                await _userRole.AddAsync(newUserRole);

                var newVerify = new EmailVerificationToken
                {
                    IsUsed = false,
                    Token = RandomNumberGenerator.GetInt32(100000, 1000000).ToString(),
                    ExpiredAt = DateTime.UtcNow.AddMinutes(2),
                    User = newUser
                };
                await _verify.AddAsync(newVerify);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
                await _email.SendEmailAsync(newUser.Email, "Verifycation Your Account Form VTTShops", newVerify.Token);
                return Result.Ok(_mapper.Map<UserDTO>(newUser)).WithSuccess("New user created successfully");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return Result.Fail("An error occurred during registration"+ex.Message);
            }
        }

        public async Task<Result<AuthResponseDto>> Login(LoginDTO login)
        {
            var user = await _user.GetUserByPhoneOrEmail(login.Credential);
            if (user != null && user.IsEmailVerified == false)
            {
                return Result.Fail(new AccountNotVerifiedError());
            }
            if (user == null || !BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash)) 
            {
                return Result.Fail(new InvalidCredentialsError());
            }
            var accessToken = _jwt.GenerateAccessToken(user);
            var refreshToken = _jwt.GenerateRefreshToken();
            var newRefreshToken = new RefreshToken()
            {
                UserId = user.Id,
                Token = refreshToken,
                ExpiryAt = DateTime.UtcNow.AddDays(7),
            };
            await _refresh.AddAsync(newRefreshToken);
            await _unitOfWork.SaveChangesAsync();
            var authResponse = new AuthResponseDto() 
            { 
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
            return Result.Ok(authResponse);
        }

        public async Task<Result<AuthResponseDto>> RefreshToken(RefreshTokenDTO tokenDto)
        {

            var oldRefreshToken = await _refresh.GetByTokenAsync(tokenDto.RefreshToken);
            if (oldRefreshToken == null)
            {
                return Result.Fail("Refresh token not found.");
            }
            if (!oldRefreshToken.IsActive)
            {
                return Result.Fail("Refresh token is invalid or expired");
            }
            var user = await _user.GetByIdAsync(oldRefreshToken.UserId);
            if (user == null)
            {
                return Result.Fail("No user associated with this refresh token found.");
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var newAccessToken = _jwt.GenerateAccessToken(user);
                var newRefreshTokenString = _jwt.GenerateRefreshToken();
                var newRefreshToken = new RefreshToken()
                {
                    UserId = user.Id,
                    Token = newRefreshTokenString,
                    ExpiryAt = DateTime.UtcNow.AddDays(7), 
                };
                await _refresh.AddAsync(newRefreshToken);

                oldRefreshToken.RevokeAt = DateTime.UtcNow;
                oldRefreshToken.ReplanceByToken = newRefreshToken.Token;
                _refresh.Update(oldRefreshToken);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
                var authResponse = new AuthResponseDto()
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshTokenString,
                };
                return Result.Ok(authResponse);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return Result.Fail("An error occurred while refreshing the token: " + ex.Message);
            }
        }

        public async Task<Result<UserDTO?>> VerifyTokenFromEmail(VerifyTokenDTO token)
        {
            var verifyUser = await _verify.GetValidTokenAsync(token.Token);
            if (verifyUser != null)
            {
                verifyUser.User.IsEmailVerified = true;
                verifyUser.IsUsed = true;
                _verify.Update(verifyUser);
                await _unitOfWork.SaveChangesAsync();
                return Result.Ok(_mapper?.Map<UserDTO>(verifyUser.User));
            }
            return Result.Fail("Invalid token");
        }

        public async Task<Result> ResendVerificationEmail(ResendEmail resend)
        {
            var user = await _user.GetUserByEmail(resend.Email);

            if (user == null)
            {
                return Result.Fail("Email not found.");
            }

            if (user.IsEmailVerified)
            {
                return Result.Fail("Email already verified.");
            }

            var token = user.EmailVerificationToken;
            if (token == null)
            {
                token = new EmailVerificationToken { UserId = user.Id, IsUsed = false };
                await _verify.AddAsync(token);
            }

            token.Token = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
            token.ExpiredAt = DateTime.UtcNow.AddMinutes(10); 
            token.IsUsed = false;

            _verify.Update(token);
            await _unitOfWork.SaveChangesAsync();

            await _email.SendEmailAsync(user.Email, "Resend: Verify Your Account", token.Token);

            return Result.Ok().WithSuccess("Verification email sent");
        }

        public async Task<Result> ForgotPassword(ForgotPassword forgot)
        {
            var findUser = await _user.GetUserByPhoneOrEmail(forgot.Infor);
            if (findUser == null)
            {
                return Result.Fail("User does not exist");
            }
            var newVerify = findUser.EmailVerificationToken;
                newVerify.IsUsed = false;
                newVerify.Token = RandomNumberGenerator.GetInt32(100000,1000000).ToString();
                newVerify.ExpiredAt = DateTime.UtcNow.AddMinutes(10);
                _verify.Update(newVerify);
            await _unitOfWork.SaveChangesAsync();
                await _email.SendEmailForgotPasswordAsync(findUser.Email, "Verify Forgot Password Your Account from VTTShops", newVerify.Token);
            return Result.Ok().WithSuccess("Verification email sent");

        }

        public async Task<Result<string>> VerifyTokenForgotPassword(VerifyTokenDTO token)
        {
            var verifyUser = await _verify.GetValidTokenAsync(token.Token);
            if (verifyUser != null)
            {
                return Result.Ok(_jwt.GeneratePasswordResetToken(verifyUser.User));
            }
            return Result.Fail("Invalid token");
        }
        public async Task<Result> ResetPassword(ResetPasswordDto model)
        {
            try
            {
                var principal = _jwt.ValidatePasswordResetToken(model.ResetToken);
                if (principal == null)
                {
                    return Result.Fail("Token is invalid or expired");
                }

                var userIdString = principal.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdString) || !long.TryParse(userIdString, out long userId))
                {
                    return Result.Fail("Unable to extract user ID from token");
                }

                var user = await _user.GetByIdAsync(userId);
                if (user == null)
                {
                    return Result.Fail("No user associated with this token found");
                }
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
                _user.Update(user);
                await _unitOfWork.SaveChangesAsync();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail("Reset password failed");
            }
        }

        public async Task<UserRoleDTO?> AddRoleAsync(UserRoleDTO role)
        {
            if (role != null)
            {
                var newRole = await _role.AddAsync(_mapper.Map<Role>(role));
                return _mapper.Map<UserRoleDTO>(newRole);
            }
            return null;
        }
    }
}
