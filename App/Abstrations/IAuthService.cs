using App.Models.Autentication;

namespace App.Abstrations
{
    public interface IAuthService
    {
        Task<UserResponse> Register(UserRegisterDto userRegisterDto);
        Task<UserResponse>Login(UserLoginDto userLoginDto);
    }
}
