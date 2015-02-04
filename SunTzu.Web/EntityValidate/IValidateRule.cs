using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunTzu.Web.EntityValidate
{
    public interface IValidateRule
    {
        string FieldName { get; set; }
        string ErrorMessage { get; set; }
        ValidateConfigRule ValidateConfigRule { get; set; }
        bool DoValidate(object value);
        string GetValidateString();
    }
}
