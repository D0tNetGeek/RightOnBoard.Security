using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RightOnBoard.Security.Api.Helpers;
using RightOnBoard.Security.Api.ViewModels;
using RightOnBoard.Security.Service.Interfaces;
using RightOnBoard.Security.Service.Models;
using RightOnBoard.Security.Service.Models.Entities;

namespace RightOnBoard.Security.Api.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        //private readonly UserManager<ApplicationUser> _userManager;
        
        public AuthController(IAuthService authService, IMapper mapper) //, UserManager<ApplicationUser> userManager)
        {
            _authService = authService;
            _mapper = mapper;
           // _userManager = userManager;
        }

        //[AllowAnonymous]
        //[IgnoreAntiforgeryToken]
        //[HttpPost("[action]")]
        //public async Task<IActionResult> Login([FromBody] LoginViewModel loginUser)
        //{
        //    if (loginUser == null)
        //    {
        //        return BadRequest("User is not set.");
        //    }

        //    //var loginModel = _mapper.Map<LoginModel>(loginUser);

        //    var loginModel = new LoginModel
        //    {
        //        Username = loginUser.UserName,
        //        Password = loginUser.Password
        //    };

        //    var result = await _authService.Login(loginModel);

        //    if (result == null) return new BadRequestObjectResult(Errors.AddErrorToModelState("500", "Invalid Credentials", ModelState));

        //    //return Ok(new {access_token = accessToken, refresh_token = refreshToken});
        //    return Ok();
        //}

        // POST api/auth/login
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody]LoginViewModel credentials)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var registrationModel = _mapper.Map<LoginModel>(credentials);
            var registrationModel = new LoginModel
            {
                Username = credentials.UserName,
                Password = credentials.Password
            };

            var result = await _authService.Login(registrationModel);

            if (result == null) return new BadRequestObjectResult(Errors.AddErrorToModelState("500", "Invalid Credentials", ModelState));

            //if (result.StatusCode != 200) return new BadRequestObjectResult(Errors.AddErrorToModelState(result.StatusCode.ToString(), "Login Unsuccessful", ModelState));

            return new OkObjectResult(result);

            //    if (!ModelState.IsValid)
            //    {
            //        return BadRequest(ModelState);
            //    }

            //    var identity = await GetClaimsIdentity(credentials.UserName, credentials.Password);
            //    if (identity == null)
            //    {
            //        return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid username or password.", ModelState));
            //    }

            //    var jwt = await Tokens.GenerateJwt(identity, _jwtFactory, credentials.UserName, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });
            //    return new OkObjectResult(jwt);
        }

        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshToken([FromBody] JToken jsonBody)
        //public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            var refreshToken = jsonBody.Value<string>("refreshToken");

            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return BadRequest("RefreshToken is not set.");
            }

            var token = await _authService.FindRefreshToken(refreshToken);

            if (token == null)
                return Unauthorized();

            return new OkObjectResult(token);
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<bool> Logout(string refreshToken)
        {
            await _authService.Logout(refreshToken);

            return true;
        }

        [HttpGet("[action]"), HttpPost("[action]")]
        public bool IsAuthentication()
        {
            return _authService.IsAuthenticated();
        }

        [HttpGet("[action]"), HttpPost("[action]")]
        public IActionResult GetUserInfo()
        {
            var userInfo = _authService.GetUserInfo();

            return new OkObjectResult(userInfo);
        }
    }
}