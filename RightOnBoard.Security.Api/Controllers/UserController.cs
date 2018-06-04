using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using RightOnBoard.Security.Api.ViewModels;
using RightOnBoard.Security.Service.Interfaces;

namespace RightOnBoard.Security.Api.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public List<UserViewModel> Get()
        {
            var userList = _userService.GetUsersList();

            return (from ul in userList
                select new UserViewModel
                {
                    Email = ul.Email,
                    FirstName = ul.FirstName,
                    LastName = ul.LastName

                }).ToList();
        }
    }
}