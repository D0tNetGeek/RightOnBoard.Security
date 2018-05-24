using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using RightOnBoard.Security.Service.DbContext;
using RightOnBoard.Security.Service.Interfaces;
using RightOnBoard.Security.Service.Models;
using RightOnBoard.Security.Service.Models.Entities;


namespace RightOnBoard.Security.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        //private readonly IUserService _userService;
        //private readonly ITokenStoreService _tokenStoreService;
        //private readonly IAntiForgeryCookieService _antiForgeryCookieService;
        //private readonly IUnitOfWork _unitOfWork;

        public AuthService(UserManager<ApplicationUser> userManager, ApplicationDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            _mapper = mapper;
        }

        //public AuthService(
        //    IUserService userService
        //    //ITokenStoreService tokenStoreService,
        //    //IUnitOfWork unitOfWork,
        //    //IAntiForgeryCookieService antiForgeryCookieService)
        //)
        //{
        //    _userService = userService;
        //    //_usersService.CheckArgumentIsNull(nameof(usersService));

        //    //_tokenStoreService = tokenStoreService;

        //    //_unitOfWork = unitOfWork;

        //    //_antiForgeryCookieService = antiForgeryCookieService;
        //}

        public async Task<string> Login(LoginModel loginUser)
        {
            var user = await _userManager.FindByNameAsync(loginUser.Username);

            bool isCorrectUser = false;

            if (user != null)
            {
                isCorrectUser = await _userManager.CheckPasswordAsync(user, loginUser.Password);
            }                

            var userIdentity = await GetClaimsIdentity(loginUser.Username, loginUser.Password);

            if (userIdentity == null)
            {
                return null;
            }

            //var jwtToken = await GenerateJwt(userIdentity, loginUser.Username, loginUser.Password,
            //    new JsonSerializerSettings
            //    {
            //        Formatting = Formatting.None,
            //        NullValueHandling = NullValueHandling.Ignore
            //    });

            //return jwt;

               var token = await GenerateJwt(loginUser, refreshTokenSource : null);

                //var jwtToken = new { access_token = token , refresh_token = token};

                //return new OkObjectResult(token);
            return token.ToString();
        }

        private async Task<object> GenerateJwt(LoginModel loginUser, string refreshTokenSource) //(string accessToken, string refreshToken, IEnumerable<Claim> Claims
        {
            string uri = "http://localhost:53875/api/token/createtoken";

            string clientId = "RightOnBoard"; // "<client id / audience id from authorization server";

            var jwtProvider = Providers.JwtProvider.Create(uri);

            var token = await jwtProvider.GetTokenAsync(loginUser.Username, loginUser.Password, clientId, Environment.MachineName);

            var mytoken = JsonConvert.DeserializeObject(token);

            string userId = null;

            //var newtoken = JsonConvert.SerializeObject(token,  new JsonSerializerSettings
            //{
            //    Formatting = Formatting.None,
            //    NullValueHandling = NullValueHandling.Ignore
            //});

            return mytoken;
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);

            // get the user to verifty
            var userToVerify = await _userManager.FindByNameAsync(userName);

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

            // check the credentials
            if (await _userManager.CheckPasswordAsync(userToVerify, password))
            {
                return await Task.FromResult(new ClaimsIdentity(
                    new System.Security.Principal.GenericIdentity(userName, "Token"), new[]
                    {
                        new Claim("Id", userToVerify.Id),
                        new Claim("Role", "Api_Access")
                    }));
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }

        private async Task<string> GenerateJwt(ClaimsIdentity identity, string username, string password,
            JsonSerializerSettings serializerSettings)
        {
            string uri = "http://localhost:52515/api/token";
            string clientId = "RightOnBoard"; // "<client id / audience id from authorization server";

            var jwtProvider = Providers.JwtProvider.Create(uri);

            string token = await jwtProvider.GetTokenAsync(username, password, clientId, Environment.MachineName);

            string userId = null;

            bool result = false;

            if (token == null)
            {
            }
            else
            {
                //decode payload
                dynamic payload = jwtProvider.DecodePayload(token);

                userId = identity.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;

                //create an Identity Claim
                ClaimsIdentity claims = jwtProvider.CreateIdentity(true, userId, username, payload);

                //sign in
                //var context = HttpContext.Current.Request.GetOwinContext();
                //var authenticationManager = context.Authentication;
                //authenticationManager.SignIn(claims);

                //return SignInStatus.Success;

                //return await Task.FromResult(token);
            }

            //object response = null;

            //if (token != null)
            //{
            //    token = token.Split('.')[0];

            //    response = new
            //    {
            //        id = userId, //identity.Claims.Single(c => c.Type == "id").Value,
            //        auth_token = token,
            //        expires_in = DateTime.UtcNow.AddMinutes(30)
            //    };
            //}

            //token
            return token;
            //return JsonConvert.SerializeObject(token, serializerSettings);
        }

        public async Task<string> FindRefreshToken(string refreshToken)
        {
            string uri = "http://localhost:53875/api/token/refreshtoken";

            var jwtProvider = Providers.JwtProvider.Create(uri);

            var token = await jwtProvider.FindRefreshTokenAsync(refreshToken);

            //var mytoken = JsonConvert.DeserializeObject(token);

            string userId = null;

            //var newtoken = JsonConvert.SerializeObject(token,  new JsonSerializerSettings
            //{
            //    Formatting = Formatting.None,
            //    NullValueHandling = NullValueHandling.Ignore
            //});

            //return mytoken;
            return token;
        }

        public async Task<bool> Logout(string refreshToken)
        {
            string uri = "http://localhost:53875/api/token/logout/" + refreshToken;

            var jwtProvider = Providers.JwtProvider.Create(uri);

            var token = await jwtProvider.Logout(refreshToken, uri);

            return true;
        }

        public bool IsAuthenticated()
        {
            var isAuthenticated = _userManager.Users.Select(x => x.Id);

            return true;
        }

        public string GetUserInfo()
        {
            var claimsIdentity = new ClaimsIdentity();

            var userInfo = new {Username = claimsIdentity.Name};

            return JsonConvert.SerializeObject(userInfo);
        }
    }
}
