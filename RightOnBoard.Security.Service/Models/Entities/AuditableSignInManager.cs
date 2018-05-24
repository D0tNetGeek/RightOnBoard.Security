using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RightOnBoard.Security.Service.DbContext;

namespace RightOnBoard.Security.Service.Models.Entities
{
    public class AuditableSignInManager<TUser> : SignInManager<TUser> where TUser : class
    {
        private readonly UserManager<TUser> _userManager;
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IAuthenticationSchemeProvider _scheme;

        public AuditableSignInManager(UserManager<TUser> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<TUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor, 
            ILogger<SignInManager<TUser>> logger, ApplicationDbContext dbContext, IAuthenticationSchemeProvider scheme)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, scheme)
        {
            if (userManager == null)
                throw new ArgumentNullException(nameof(userManager));

            if (dbContext == null)
                throw new ArgumentNullException(nameof(dbContext));

            if (contextAccessor == null)
                throw new ArgumentNullException(nameof(contextAccessor));

            _userManager = userManager;
            _contextAccessor = contextAccessor;
            _db = dbContext;
            _scheme = scheme;
        }

        //public override async Task<SignInResult> PasswordSignInAsync(TUser username, string password, bool isPersistent, bool lockoutOnFailure)
        //{
        //    string uri = "http://localhost:52515/api/token";
        //    string clientId = "RightOnBoard"; // "<client id / audience id from authorization server";
        //    var jwtProvider = Providers.JwtProvider.Create(uri);
        //    string token = await jwtProvider.GetTokenAsync(username.ToString(), password, clientId, Environment.MachineName);

        //    var result = await base.PasswordSignInAsync(username, password, isPersistent, lockoutOnFailure);

        //    if (token == null && !result.Succeeded)
        //    {
        //        result =  SignInResult.Failed; //"Failure";
        //    }
        //    else
        //    {
        //        //decode payload
        //        dynamic payload = jwtProvider.DecodePayload(token);
        //        //create an Identity Claim
        //        ClaimsIdentity claims = jwtProvider.CreateIdentity(true, username.ToString(), payload);

        //        //sign in
        //        //var context = HttpContext.Current.Request.GetOwinContext();
        //        //var authenticationManager = context.Authentication;
        //        //authenticationManager.SignIn(claims);

        //        result = SignInResult.Success; //"Success";  //return SignInStatus.Success;
        //    }

        //    var appUser = username as IdentityUser;

        //    if (appUser != null) // We can only log an audit record if we can access the user object and it's ID
        //    {
        //        var ip = _contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

        //        UserAudit auditRecord = null;

        //        switch (result.ToString())
        //        {
        //            case "Succeeded":
        //                auditRecord = UserAudit.CreateAuditEvent(appUser.Id, UserAuditEventType.Login, ip);
        //                break;

        //            case "Failed":
        //                auditRecord = UserAudit.CreateAuditEvent(appUser.Id, UserAuditEventType.FailedLogin, ip);
        //                break;
        //        }

        //        if (auditRecord != null)
        //        {
        //            _db.UserAuditEvents.Add(auditRecord);
        //            await _db.SaveChangesAsync();
        //        }
        //    }

        //    return result;
        //}

        //public override async Task<SignInStatus> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout)
        //{
        //    string uri = "http://localhost:52515/api/token";
        //    string clientId = "RightOnBoard"; // "<client id / audience id from authorization server";
        //    var jwtProvider = Providers.JwtProvider.Create(uri);
        //    string token = await jwtProvider.GetTokenAsync(userName, password, clientId, Environment.MachineName);

        //    if (token == null)
        //    {
        //        return SignInStatus.Failure;
        //    }
        //    else
        //    {
        //        //decode payload
        //        dynamic payload = jwtProvider.DecodePayload(token);
        //        //create an Identity Claim
        //        ClaimsIdentity claims = jwtProvider.CreateIdentity(true, userName, payload);

        //        //sign in
        //        var context = HttpContext.Current.Request.GetOwinContext();
        //        var authenticationManager = context.Authentication;
        //        authenticationManager.SignIn(claims);

        //        return SignInStatus.Success;
        //    }
        //}

        public override async Task SignOutAsync()
        {
            await base.SignOutAsync();

            //var user = await _userManager.FindByIdAsync(_contextAccessor.HttpContext.User.GetUserId()) as IdentityUser;

            //if (user != null)
            //{
            //    var ip = _contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            //    var auditRecord = UserAudit.CreateAuditEvent(user.Id, UserAuditEventType.LogOut, ip);
            //    _db.UserAuditEvents.Add(auditRecord);
            //    await _db.SaveChangesAsync();
            //}
        }
    }
}
