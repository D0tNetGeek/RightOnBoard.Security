using System.Collections.Generic;
using RightOnBoard.Security.Service.Models;

namespace RightOnBoard.Security.Service.Interfaces
{
    public interface IUserService
    {
        List<UserModel> GetUsersList();
    }
}
