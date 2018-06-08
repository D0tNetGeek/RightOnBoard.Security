using System.Collections.Generic;
using System.Linq;
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

            var userModel = _mapper.Map<Service.Models.UserModel>(user);

            var result = await _userService.CreateUser(userModel);

            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            return new OkObjectResult("Account created successfully !!!");
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