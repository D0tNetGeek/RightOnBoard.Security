//using System;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.Owin;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.Owin;
//using Microsoft.Owin.Security;
//using RightOnBoard.Security.Service.DbContext;

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace RightOnBoard.Security.Service.Models.Entities
{
    //public class ApplicationSignInManager : SignInManager<ApplicationUser>
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
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

        //public ApplicationSignInManager(UserManager<ApplicationUser> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<ApplicationUser>> logger, IAuthenticationSchemeProvider schemes) : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes)
        //{
        //}
        public ApplicationUserManager(IUserStore<ApplicationUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<ApplicationUser> passwordHasher, IEnumerable<IUserValidator<ApplicationUser>> userValidators,
            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, 
            ILogger<UserManager<ApplicationUser>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger) 
            
        {
        }

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
    }
    //    //public class EmailService : IIdentityMessageService
    //    //{
    //    //    public Task SendAsync(IdentityMessage message)
    //    //    {
    //    //        // Plug in your email service here to send an email. 
    //    //        return Task.FromResult(0);
    //    //    }
    //    //}

    //    //public class SmsService : IIdentityMessageService
    //    //{
    //    //    public Task SendAsync(IdentityMessage message)
    //    //    {
    //    //        // Plug in your SMS service here to send a text message. 
    //    //        return Task.FromResult(0);
    //    //    }
    //    //}

    //    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application. 
    //    public class ApplicationUserManager : UserManager<ApplicationUser>, IUser<string>
    //    {
    //        private UserStore<ApplicationUser> userStore;

    //        public ApplicationUserManager(IUserStore<ApplicationUser> store)
    //            : base(store)
    //        {
    //        }

    //        public ApplicationUserManager(UserStore<ApplicationUser> userStore)
    //        {
    //            this.userStore = userStore;
    //        }

    //        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
    //        {
    //            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
    //            // Configure validation logic for usernames 
    //            //manager.UserValidator = new Microsoft.AspNetCore.Identity.UserValidator<ApplicationUser>(manager)
    //            //{
    //            //    AllowOnlyAlphanumericUserNames = false,
    //            //    RequireUniqueEmail = true
    //            //};

    //            // Configure validation logic for passwords 
    //            //manager.PasswordValidator = new PasswordValidator
    //            //{
    //            //    RequiredLength = 6,
    //            //    RequireNonLetterOrDigit = true,
    //            //    RequireDigit = true,
    //            //    RequireLowercase = true,
    //            //    RequireUppercase = true,
    //            //};

    //            //// Configure user lockout defaults 
    //            //manager.UserLockoutEnabledByDefault = true;
    //            //manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
    //            //manager.MaxFailedAccessAttemptsBeforeLockout = 5;

    //            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user 
    //            // You can write your own provider and plug it in here. 
    //            //manager.RegisterTwoFactorProvider("Phone Code", new Microsoft.AspNetCore.Identity.PhoneNumberTokenProvider<ApplicationUser>
    //            //{
    //            //    MessageFormat = "Your security code is {0}"
    //            //});
    //            //manager.RegisterTwoFactorProvider("Email Code", new Microsoft.AspNetCore.Identity.EmailTokenProvider<ApplicationUser>
    //            //{
    //            //    Subject = "Security Code",
    //            //    BodyFormat = "Your security code is {0}"
    //            //});
    //            //manager.EmailService = new EmailService();
    //            //manager.SmsService = new SmsService();
    //            //var dataProtectionProvider = options.DataProtectionProvider;
    //            //if (dataProtectionProvider != null)
    //            //{
    //            //    manager.UserTokenProvider =
    //            //        new Microsoft.AspNetCore.Identity.DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
    //            //}
    //            return manager;
    //        }

    //        public string Id { get; }
    //        public string UserName { get; set; }
    //    }

    //    // Configure the application sign-in manager which is used in this application. 
    //    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    //    {
    //        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
    //            : base(userManager, authenticationManager)
    //        {
    //        }

    //        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
    //        {
    //            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
    //        }

    //        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
    //        {
    //            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
    //        }
    //    }
}
