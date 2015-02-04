using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunTzu.Web.EntityValidate
{
    class GuidRule : AbstractValidateRule
    {
        public override bool DoValidate(object value)
        {
            if (value != null)
            {
                if (value.GetType() == typeof(Guid))
                {
                    return true;
                }
                if (value.GetType() == typeof(string))
                {
                    Guid guid;
                    return Guid.TryParse(value.ToString(), out guid);
                }
            }
            return false;
        }

        public override string GetValidateString()
        {
            throw new NotImplementedException();
        }
    }
}
