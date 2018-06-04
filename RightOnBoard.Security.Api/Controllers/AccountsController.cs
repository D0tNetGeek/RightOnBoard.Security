using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RightOnBoard.Security.Api.Helpers;
using RightOnBoard.Security.Api.ViewModels;
using RightOnBoard.Security.Service.Interfaces;
using RightOnBoard.Security.Service.Models;

namespace RightOnBoard.Security.Api.Controllers
{
    [Route("api/[controller]")]
    public class AccountsController : Controller
    {
        private readonly IAccountsService _authService;
        private readonly IMapper _mapper;

        public AccountsController(IAccountsService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        //POST api/accounts
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var registrationModel = _mapper.Map<UserModel>(model);

            //var registrationModel = _mapper.Map<RegistrationModel>(model);

            var result = await _authService.Register(registrationModel);

            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            return new OkObjectResult("Account created successfully !!!");
        }
    }
}
