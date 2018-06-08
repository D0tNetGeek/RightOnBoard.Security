using System;

namespace RightOnBoard.Security.Service.Models.Entities
{
    public class RegistrationOptionCompany
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public Guid RegistrationOptionId { get; set; }
        public int OrderValue { get; set; }
    }
}
