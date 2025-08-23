using App.Abstrations;
using App.Models.Autentication;
using System.Linq;
using Domain.Context;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Models;
using Domain.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using IdentityRole = Domain.Entities.IdentityRole;

namespace App.Services
{
    public class AuthService(IOptions<AuthOptions> authOptions,UserManager<UserEntity>userManager) : IAuthService
    {
        private readonly AuthOptions _authOptions=authOptions.Value;
        public async Task<UserResponse> Login(UserLoginDto userLoginDto)
        {
            var user = await userManager.FindByEmailAsync(userLoginDto.Email);
            if (user == null)
            {
                throw new EntityNotFound($"User with Email {userLoginDto.Email} not found");
            }
            var chechPasswordResult =await userManager.CheckPasswordAsync(user,userLoginDto.Password);
            if (chechPasswordResult)
            {
                var userRoles =await userManager.GetRolesAsync(user);
                var response = new UserResponse
                {
                    Id = user.Id,
                    Email = user.Email,
                    Roles = userRoles.ToArray(),
                    UserName = user.UserName,
                    Phone = user.PhoneNumber
                };
                return GenerateToken(response);
            }

            throw new AuthenticationException();

            
        }

        public async Task<UserResponse> Register(UserRegisterDto userRegisterDto)
        {
            if (await userManager.FindByEmailAsync(userRegisterDto.Email)==null)
            {
                throw new DuplicateWaitObjectException($"Email:{userRegisterDto.Email} already exist");
            }
            var createUserResult = await userManager.CreateAsync(new UserEntity
            {
                Email = userRegisterDto.Email,
                PhoneNumber = userRegisterDto.Phone,
                UserName=userRegisterDto.UserName
            },userRegisterDto.Password);

            if (createUserResult.Succeeded)
            {
                var user =await userManager.FindByEmailAsync(userRegisterDto.Email);
                if (user == null)
                {
                    throw new EntityNotFound($"User with Email{userRegisterDto.Email} not register");

                }
                var result = await userManager.AddToRoleAsync(user, RoleConsts.User);
                if (result.Succeeded)
                {
                    var response = new UserResponse
                    {
                        Id=user.Id,
                        Email=user.Email,
                        Roles = [RoleConsts.User],
                        UserName=user.UserName,
                        Phone=user.PhoneNumber
                    };
                    return GenerateToken(response);
                }

                throw new Exception($"Errors:{string.Join(";",result.Errors.Select(x => $"{x.Code} {x.Description}"))}");
            }

            throw new Exception();
        }
        public UserResponse GenerateToken(UserResponse userRegisterModel)
        {
            var handler = new JwtSecurityTokenHandler();
            var key=Encoding.ASCII.GetBytes(_authOptions.TokenPrivateKey);
            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GenerateClaims(userRegisterModel),
                Expires = DateTime.UtcNow.AddMinutes(_authOptions.ExpireIntervalMinutes),
                SigningCredentials = credentials
            };
            var token = handler.CreateToken(tokenDescriptor);
            userRegisterModel.Token=handler.WriteToken(token);

            return userRegisterModel;

        }
        private static ClaimsIdentity GenerateClaims(UserResponse userRegisterModel)
        {
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.Name, userRegisterModel.Email!));
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, userRegisterModel.Id.ToString()));
            claims.AddClaim(new Claim(JwtRegisteredClaimNames.Aud, "test"));
            claims.AddClaim(new Claim(JwtRegisteredClaimNames.Iss, "test"));

            foreach (var role in userRegisterModel.Roles!)
                claims.AddClaim(new Claim(ClaimTypes.Role, role));

            return claims;
        }
    }
}
