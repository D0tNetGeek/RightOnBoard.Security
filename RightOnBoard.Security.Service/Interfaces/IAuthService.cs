using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RightOnBoard.Security.Service.Models;

namespace RightOnBoard.Security.Service.Interfaces
{
    public interface IAuthService
    {
        Task<string> Login(LoginModel model);
        Task<string> FindRefreshToken(string token);
        Task<bool> Logout(string refreshToken);
        bool IsAuthenticated();
        string GetUserInfo();
    }
}
