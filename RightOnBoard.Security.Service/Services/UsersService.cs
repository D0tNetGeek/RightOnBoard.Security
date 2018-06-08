using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using RightOnBoard.JwtAuthTokenServer.Service.Interfaces;
using RightOnBoard.Security.Service.DbContext;
using RightOnBoard.Security.Service.Interfaces;
using RightOnBoard.Security.Service.Models;
using RightOnBoard.Security.Service.Models.Entities;

namespace RightOnBoard.Security.Service.Services
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UsersService(UserManager<ApplicationUser> userManager, ApplicationDbContext appDbContext, IUserService userService)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
            _userService = userService;
        }

        public List<UserModel> GetUsersList()
        {
            var userList = _userManager.Users.ToList();

            return (from ul in userList
                    join cust in _appDbContext.Customers on ul.Id equals cust.UserId
                    join c in _appDbContext.Company on cust.CompanyId equals c.CompanyId
                    join ur in _appDbContext.UserRoles on cust.UserId equals ur.UserId
                    join r in _appDbContext.Roles on ur.RoleId equals r.Id
                select new UserModel
                {
                    Email = ul.Email,
                    FirstName = ul.FirstName,
                    LastName = ul.LastName,
                    CompanyName = c.CompanyName,
                    RoleName = r.Name

                }).ToList();
        }

        public async Task<IdentityResult> CreateUser(UserModel user)
        {
            var userIdentity = _mapper.Map<ApplicationUser>(user);

            var result = await _userManager.CreateAsync(userIdentity, user.Password);

            if (result.Succeeded)
            {
                var res = await _appDbContext.Customers.AddAsync(new Customers
                {
                    //IdentityId = userIdentity.Id,
                    UserId = userIdentity.Id,
                    Location = user.Location,

                });

                if(res.State == Microsoft.EntityFrameworkCore.EntityState.Added)
                {
                    await _appDbContext.UserRoles.AddAsync(new IdentityUserRole<string>
                    {
                        UserId = "",
                        RoleId = user.RoleId
                    });
                }
                else
                {
                    return IdentityResult.Failed();
                }
            }

            return result;
        }

        public List<RoleModel> GetRoles()
        {
            var rolesList = (from roles in _appDbContext.Roles
                             select new RoleModel
                             {
                                 RoleId = roles.Id,
                                 RoleName = roles.Name
                             }).ToList();

            return rolesList;
        }

        public List<RegOptionsModel> GetRegistrationOptions(string companyId)
        {
            //var userId = _userService.GetCurrentUserId();

            var regOptions = (from roc in _appDbContext.RegistrationOptionCompany
                              join roo in _appDbContext.RegistrationOptions on roc.RegistrationOptionId.ToString() equals roo.Id.ToString()
                              //join rov in _appDbContext.RegistrationOptionValues on ro.Id equals rov.RegistrationOptionId
                              where roc.CompanyId.ToString() == companyId
                              select new RegOptionsModel
                              {
                                  RegistrationOptionsCompanyId = roc.Id.ToString(),
                                  CompanyId = roc.CompanyId.ToString(),
                                  RegistrationOptionId = roc.RegistrationOptionId.ToString(),
                                  RegOptLabels =new RegOptLabel
                                  {
                                      RegistrationOptionId = roo.Id.ToString(),
                                      DisplayLabel = roo.DisplayLabel,
                                      RegOptVals = (from ro in _appDbContext.RegistrationOptions
                                                    join rov in _appDbContext.RegistrationOptionValues on ro.Id equals rov.RegistrationOptionId
                                                    where ro.Id == rov.RegistrationOptionId && rov.RegistrationOptionId == roo.Id
                                                    select new RegOptVal
                                                    {
                                                        RegistrationOptionValueId = rov.Id.ToString(),
                                                        RegistrationOptionId = ro.Id.ToString(),
                                                        DisplayValue = rov.DisplayValue,
                                                        OptionValue = rov.OptionValue,
                                                        OrderValue = rov.OrderValue
                                                    }).ToList(),

                                      IsRequired = roo.IsRequired
                                  },
                                  OrderValue = roc.OrderValue
                              }).ToList();

            return regOptions;
        }
    }
}
