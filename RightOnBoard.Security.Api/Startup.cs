using System.Net;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RightOnBoard.Security.Api.Models;
using RightOnBoard.Security.Service.DbContext;
using RightOnBoard.Security.Service.Interfaces;
using RightOnBoard.Security.Service.Models.Entities;
using RightOnBoard.Security.Service.Services;

namespace RightOnBoard.Security.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.Configure<BearerTokensOptions>(options => Configuration.GetSection("BearerTokens").Bind(options));
            services.Configure<ApiSettings>(options => Configuration.GetSection("ApiSettings").Bind(options));

            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IAccountsService, AccountsService>();            

            services.AddScoped<DbContext, ApplicationDbContext>();

            services.AddScoped<DbContextOptions<ApplicationDbContext>>();

            services.AddScoped<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>();
            services.AddScoped<UserManager<ApplicationUser>>();
            
            //services.AddScoped<IUserService, UserService>();
            //services.AddScoped<ISecurityService, SecurityService>();
            //services.AddScoped<ITokenStoreService, TokenStoreService>();
            //services.AddScoped<ITokenValidatorService, TokenValidatorService>();
            //services.AddScoped<JwtAuthTokenServer.DataLayer>();

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("RightOnBoardConnectionString"), b => b.MigrationsAssembly("RightOnBoard.Security.Api")));

            services.AddScoped<IPasswordHasher<ApplicationUser>, PasswordHasher<ApplicationUser>>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
                //.AddUserManager<AuditableUserManager<ApplicationUser>>();

            //services.AddScoped<SignInManager<ApplicationUser>, AuditableSignInManager<ApplicationUser>>();

            //// Only needed for custom roles.
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy(CustomRoles.Admin, policy => policy.RequireRole(CustomRoles.Admin));
            //    options.AddPolicy(CustomRoles.User, policy => policy.RequireRole(CustomRoles.User));
            //    options.AddPolicy(CustomRoles.Editor, policy => policy.RequireRole(CustomRoles.Editor));
            //});

            //// Needed for jwt auth.
            //services
            //    .AddAuthentication(options =>
            //    {
            //        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //        options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            //        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    })
            //    .AddJwtBearer(cfg =>
            //    {
            //        cfg.RequireHttpsMetadata = false;
            //        cfg.SaveToken = true;
            //        cfg.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidIssuer = Configuration["BearerTokens:Issuer"], // site that makes the token
            //            ValidateIssuer = false, // TODO: change this to avoid forwarding attacks
            //            ValidAudience = Configuration["BearerTokens:Audience"], // site that consumes the token
            //            ValidateAudience = false, // TODO: change this to avoid forwarding attacks
            //            IssuerSigningKey =
            //                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["BearerTokens:Key"])),
            //            ValidateIssuerSigningKey = true, // verify signature to avoid tampering
            //            ValidateLifetime = true, // validate the expiration
            //            ClockSkew = TimeSpan.Zero // tolerance for the expiration date
            //        };
            //        cfg.Events = new JwtBearerEvents
            //        {
            //            OnAuthenticationFailed = context =>
            //            {
            //                var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>()
            //                    .CreateLogger(nameof(JwtBearerEvents));
            //                logger.LogError("Authentication failed.", context.Exception);
            //                return Task.CompletedTask;
            //            },
            //            OnTokenValidated = context =>
            //            {
            //                var tokenValidatorService = context.HttpContext.RequestServices
            //                    .GetRequiredService<ITokenValidatorService>();
            //                return tokenValidatorService.ValidateAsync(context);
            //            },
            //            OnMessageReceived = context => { return Task.CompletedTask; },
            //            OnChallenge = context =>
            //            {
            //                var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>()
            //                    .CreateLogger(nameof(JwtBearerEvents));
            //                logger.LogError("OnChallenge error", context.Error, context.ErrorDescription);
            //                return Task.CompletedTask;
            //            }
            //        };
            //    });

            // add identity
            var builder1 = services.AddIdentityCore<ApplicationUser>(o =>
            {
                // configure identity options
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
            });

            builder1 = new IdentityBuilder(builder1.UserType, typeof(IdentityRole), builder1.Services);
            builder1.AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .WithOrigins("http://localhost:4200") //Note:  The URL must be specified without a trailing slash (/).
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddAntiforgery(x => x.HeaderName = "X-XSRF-TOKEN");
            services.AddMvc(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });           

            services.AddAutoMapper();
            services.AddMvc().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(
                    async context =>
                    {
                        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                        context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            //context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
                        }
                    });

                appBuilder.Use(async (context, next) =>
                {
                    var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;
                    if (error != null && error.Error is SecurityTokenExpiredException)
                    {
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                        {
                            State = 401,
                            Msg = "token expired"
                        }));
                    }
                    else if (error != null && error.Error != null)
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                        {
                            State = 500,
                            Msg = error.Error.Message
                        }));
                    }
                    else
                    {
                        await next();
                    }
                });
            });            

            //app.UseCors(builder => builder
            //    .WithHeaders("Access-Control-Allow-Origin")
            //    //.WithOrigins("http://localhost:4200")
            //    .AllowAnyOrigin()
            //    .AllowAnyMethod()
            //    .AllowAnyHeader()
            //    .AllowCredentials());

            //app.UseCors(builder => builder.AllowAnyOrigin());

            app.UseMiddleware<ExceptionCatchMiddleware>();

            //app.UseCors(options => options.WithHeaders("Access-Control-Allow-Origin").AllowAnyMethod().WithOrigins("http://localhost:4200").AllowAnyHeader());            

            app.UseAuthentication();

            app.UseStatusCodePages();

            app.UseMvcWithDefaultRoute();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            //app.UseMvc();
        }
    }
}
