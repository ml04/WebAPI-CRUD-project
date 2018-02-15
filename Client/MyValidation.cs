using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Client
{
    class MyValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            double temp;

            try
            {
                if (Double.TryParse(value.ToString(), out temp))
                {
                    return new ValidationResult(true, null);
                }
                else
                {
                    return new ValidationResult(false, "String characters are not allowed. Salary consists of only numbers.....");
                }
            }
            catch (Exception)
            {
                return new ValidationResult(false, "String characters are not allowed. Salary consists of only numbers.....");
            }
        }
    }
}
