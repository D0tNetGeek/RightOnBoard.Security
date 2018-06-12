using System.Collections.Generic;

namespace RightOnBoard.Security.Api.ViewModels
{
    public class UserViewModel
    {
        public string CompanyId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public string[] RegOptions { get; set; }
        public string CompanyName { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
    }

    public class RegOption
    {
        public string RegistrationOptionId { get; set; }
        //public string RegistrationOptionValueId { get; set; }
    }
}