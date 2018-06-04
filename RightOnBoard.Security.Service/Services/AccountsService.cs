using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using RightOnBoard.Security.Service.DbContext;
using RightOnBoard.Security.Service.Interfaces;
using RightOnBoard.Security.Service.Models;
using RightOnBoard.Security.Service.Models.Entities;

namespace RightOnBoard.Security.Service.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public AccountsService(ApplicationDbContext appDbContext, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IdentityResult> Register(UserModel model)
        {
            var userIdentity = _mapper.Map<ApplicationUser>(model);

            var result = await _userManager.CreateAsync(userIdentity, model.Password);

            if (result.Succeeded)
            {
                await _appDbContext.Customers.AddAsync(new Customer
                {
                    IdentityId = userIdentity.Id,
                    Location = model.Location,

                });

            }

            return result;
        }
    }
}
