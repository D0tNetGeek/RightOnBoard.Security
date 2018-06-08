using System.Collections.Generic;

namespace RightOnBoard.Security.Service.Models
{
    public class RegOptionsModel
    {
        public string RegistrationOptionsCompanyId { get; internal set; }
        public string CompanyId { get; set; }
        public string RegistrationOptionId { get; set; }
        public RegOptLabel RegOptLabels { get; set; }
        public int OrderValue { get; set; }
    }

    public class RegOptLabel
    {
        public string RegistrationOptionId { get; set; }
        public string DisplayLabel { get; set; }
        public List<RegOptVal> RegOptVals { get; set; }
        public bool IsRequired { get; internal set; }

    }

    public class RegOptVal
    {
        public string RegistrationOptionValueId { get; set; }
        public string RegistrationOptionId { get; set; }
        public string DisplayValue { get; set; }
        public string OptionValue { get; set; }
        public int OrderValue { get; set; }
    }
}
