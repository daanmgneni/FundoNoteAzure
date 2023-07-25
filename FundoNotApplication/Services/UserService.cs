using FundoNotApplication.Context;
using FundoNotApplication.Entities;
using FundoNotApplication.Interface;
using FundoNotApplication.Models;
using FundoNoteswithAzure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace FundoNotApplication.Services;

public class UserService : IUser
{
    private readonly IConfiguration _config;
    private readonly FundooContext _db;

    public UserService(IConfiguration config, FundooContext db)
    {
        _config = config;
        _db = db;
        _db.Database.EnsureCreated();
    }
    public UserEntity Register(UserRegistration userModel)
    {
        UserEntity newUser = new()
        {
            
            EmailId = userModel.EmailId.ToLower(),
            FirstName = userModel.FirstName,
            LastName = userModel.LastName,
            Password = userModel.Password,
            RegisteredAt = DateTime.Now,
        };

        _db.Users.Add(newUser);
        int result = _db.SaveChanges();

        if (result > 0)
            return newUser;
        else
            return null;
    }

    public string LogIn(string email, string password)
    {
        email = email.ToLower();
        UserEntity someUser = _db.Users.FirstOrDefault( x => x.EmailId == email && x.Password == password);

        if (someUser != null)
        {
            string token = GenerateToken(someUser.EmailId);
            return token;
        }
        else
            return null;
    }

    public bool ForgetPassword(string email)
    {
        email = email.ToLower(); //to remove case sensitivity
        UserEntity someUser = _db.Users.FirstOrDefault(x => x.EmailId == email);

        if (someUser != null)
        {
            string token = GenerateToken(someUser.EmailId);
            MessageServices ms = new MessageServices(_config);
            ms.SendMessageToQueue(someUser.EmailId, token);
            return true;
        }
        return false;
    }

    public bool ResetPassword(string newPassword,string emailId, string comfirmPassword)
    {
        UserEntity someUser = _db.Users.FirstOrDefault(x => x.EmailId == emailId);
        if (someUser != null && newPassword == comfirmPassword)
        {
            someUser.Password = newPassword;
            _db.Users.Update(someUser);
            _db.SaveChanges();
            return true;
        }
        else
            return false;
    }

    private string GenerateToken(string email)
    {
        byte[] key = Encoding.ASCII.GetBytes(_config["JWT-Key"]);

        SecurityTokenDescriptor tokenDescripter = new()
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim (ClaimTypes.Email, email),
            }),
            Expires = DateTime.UtcNow.AddMinutes(60),
            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        JwtSecurityTokenHandler tokenHandler = new();
        SecurityToken token = tokenHandler.CreateToken(tokenDescripter);

        return tokenHandler.WriteToken(token);
    }
}
