using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using RightOnBoard.Security.Service.Models;

namespace RightOnBoard.Security.Service.Interfaces
{
    public interface IAccountsService
    {
        Task<IdentityResult> Register(RegistrationModel model);
    }
}
