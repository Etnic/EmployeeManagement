using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class ValidEmailDomainAttribute : ValidationAttribute
    {
        private readonly string domain;

        public ValidEmailDomainAttribute(string domain)
        {
            this.domain = domain;
        }
        
        public override bool IsValid(object value)
        {
            string[] result = value.ToString().Split('@');
            return result[1].ToLower() == this.domain.ToLower();
        }
    }
}
