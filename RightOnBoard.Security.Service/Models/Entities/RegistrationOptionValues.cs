using System;

namespace RightOnBoard.Security.Service.Models.Entities
{
    public class RegistrationOptionValues
    {
        public Guid Id { get; set; }
        public Guid RegistrationOptionId { get; set; }
        public string DisplayValue { get; set; }
        public string OptionValue { get; set; }
        public int OrderValue { get; set; }
    }
}
