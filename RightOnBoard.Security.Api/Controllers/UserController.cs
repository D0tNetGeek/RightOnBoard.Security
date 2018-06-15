using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using RightOnBoard.Security.Api.Helpers;
using RightOnBoard.Security.Api.Models;
using RightOnBoard.Security.Api.ViewModels;
using RightOnBoard.Security.Service.Interfaces;

namespace RightOnBoard.Security.Api.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUsersService _userService;
        private readonly IMapper _mapper;

        public UserController(IUsersService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public List<UserViewModel> Get()
        {
            var userList = _userService.GetUsersList();

            return (from ul in userList
                select new UserViewModel
                {
                    CompanyId = ul.CompanyId,
                    Email = ul.Email,
                    FirstName = ul.FirstName,
                    LastName = ul.LastName,
                    CompanyName = ul.CompanyName,
                    RoleName = ul.RoleName

                }).ToList();
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        [Route("createuserbyadmin")]
        public async Task<IActionResult> CreateUser([FromBody] UserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Service.Models.UserModel userModel = new Service.Models.UserModel
            {
                CompanyId = user.CompanyId,
                CompanyName = user.CompanyName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Location = user.Location,
                Password = user.Password,
                RoleId = user.RoleId,
                RoleName = user.RoleName,
                RegOptions = (from reg in user.RegOptions
                              select new Service.Models.RegOption
                              {
                                  RegistrationOptionId = reg.Split(":")[0],
                                  RegistrationOptionValueId = reg.Split(":")[1]
                              }
                ).ToList()
            };

            //var userModel = _mapper.Map<Service.Models.UserModel>(user);

            var result = await _userService.CreateUser(userModel);

            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));


            if (result.Succeeded)
                return Ok(result);
            else
                return
                    BadRequest();
            //return new OkObjectResult("Account created successfully !!!");
        }

        [HttpGet]
        [Route("getrolesforadmin")]
        public List<RoleModel> GetRolesForAdmin()
        {
            var roles = _userService.GetRoles();

            return (from r in roles
                    select new RoleModel
                    {
                        RoleId = r.RoleId,
                        RoleName = r.RoleName
                    }).ToList();
        }

        [HttpGet]
        [Route("getregistrationoptions")]
        public List<RegistrationOptions> GetRegistrationOptions(string companyId)
        {
            var regOptions = _userService.GetRegistrationOptions(companyId);

            var regOptionsNew = _mapper.Map<List<RegistrationOptions>>(regOptions);
            return regOptionsNew;
        }
    }
}