using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using RightOnBoard.Security.Service.Models;

namespace RightOnBoard.Security.Service.Interfaces
{
    public interface IUsersService
    {
        List<UserModel> GetUsersList();
        Task<IdentityResult> CreateUser(UserModel user);
        List<RoleModel> GetRoles();
        List<RegOptionsModel> GetRegistrationOptions(string companyId);
    }
}
