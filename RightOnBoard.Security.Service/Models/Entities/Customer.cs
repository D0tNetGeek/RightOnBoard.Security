namespace RightOnBoard.Security.Service.Models.Entities
{
    public class Customers
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        //public string IdentityId { get; set; }
        //public ApplicationUser Identity { get; set; }  // navigation property
        public string Location { get; set; }
        public string Locale { get; set; }
        public string Gender { get; set; }
        public string CompanyId { get; set; }
    }
}


//public class UserModel
//{
//    public string Email { get; set; }
//    public string Password { get; set; }
//    public string FirstName { get; set; }
//    public string LastName { get; set; }
//    public string Location { get; set; }
//    public List<RegOption> RegOptions { get; set; }
//    public string CompanyName { get; set; }
//    public string RoleId { get; set; }
//    public string RoleName { get; set; }
//}

//public class RegOption
//{
//    public string RegistrationOptionId { get; set; }
//    public string RegistrationOptionValueId { get; set; }
//}