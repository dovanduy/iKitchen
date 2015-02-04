using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunTzu.Web.EntityValidate
{
    class DateTimeRule : AbstractValidateRule
    {
        public override bool DoValidate(object value)
        {
            try
            {
                var dateTime = Convert.ToDateTime(value);
                return true;
            }
            catch { return false; }
        }

        public override string GetValidateString()
        {
            return "date:true";
        }
    }
}
