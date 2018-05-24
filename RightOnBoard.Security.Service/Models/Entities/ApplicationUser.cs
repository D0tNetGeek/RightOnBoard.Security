using Microsoft.AspNetCore.Identity;

namespace RightOnBoard.Security.Service.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        // Extended Properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long? FacebookId { get; set; }
        public string PictureUrl { get; set; }

        //public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUserManager> manager)
        //{
        //    // Note the authenticationType must match the one 
        //    // defined in CookieAuthenticationOptions.AuthenticationType
        //    var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

        //    // Add custom user claims here
        //    return userIdentity;
        //}
    }
}
