using FundoNotApplication.Entities;
using FundoNotApplication.Models;

namespace FundoNotApplication.Interface
{
    public interface IUser
    {
        string LogIn(string email, string password);
        UserEntity Register(UserRegistration usermodel);
        bool ForgetPassword(string email);
        bool ResetPassword(string userId,string newPassword, string ComfirmPassword);
    }
}
