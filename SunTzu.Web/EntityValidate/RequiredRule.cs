using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunTzu.Web.EntityValidate
{
    class RequiredRule : AbstractValidateRule
    {
        public override bool DoValidate(object value)
        {
            return value != null;
        }

        public override string GetValidateString()
        {
            return "required:true";
        }
    }
}
