using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunTzu.Web.EntityValidate
{
    class DummyRule : AbstractValidateRule
    {
        public override bool DoValidate(object value)
        {
            return true;
        }

        public override string GetValidateString()
        {
            return "";
        }
    }
}
