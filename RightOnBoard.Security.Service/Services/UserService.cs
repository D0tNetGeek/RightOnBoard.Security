using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using RightOnBoard.Security.Service.DbContext;
using RightOnBoard.Security.Service.Interfaces;
using RightOnBoard.Security.Service.Models;
using RightOnBoard.Security.Service.Models.Entities;

namespace RightOnBoard.Security.Service.Services
{
    public class UserService : IUserService
    {
        //private readonly ApplicationDbContext _appDbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager) //ApplicationDbContext appDbContext)
        {
            _userManager = userManager;
            // _appDbContext = appDbContext;
        }

        public List<UserModel> GetUsersList()
        {
            var userList = _userManager.Users.ToList();

            return (from ul in userList
                select new UserModel
                {
                    Email = ul.Email,
                    FirstName = ul.FirstName,
                    LastName = ul.LastName

                }).ToList();
        }
    }
}
