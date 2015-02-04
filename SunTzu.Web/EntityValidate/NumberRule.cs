using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunTzu.Web.EntityValidate
{
    public class NumberRule : AbstractValidateRule
    {
        public override bool DoValidate(object value)
        {
            throw new NotImplementedException();
        }

        public override string GetValidateString()
        {
            return "number:true";
        }
    }
}
