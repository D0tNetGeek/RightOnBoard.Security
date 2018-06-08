using System;

namespace RightOnBoard.Security.Service.Models.Entities
{
    public class RegistrationOptions
    {
        public Guid Id { get; set; }
        public string DisplayLabel { get; set; }
        public bool IsRequired { get; set; }
    }
}
