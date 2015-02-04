using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunTzu.Web.EntityValidate
{
    class LengthRule : AbstractValidateRule
    {
        private int minLength = int.MinValue;
        public int MinLength
        {
            get { return minLength; }
            set { minLength = value; }
        }

        private int maxLength = int.MinValue;
        public int MaxLength
        {
            get { return maxLength; }
            set { maxLength = value; }
        }

        public override bool DoValidate(object value)
        {
            if (value != null)
            {
                var length = value.ToString().Length;
                return minLength != int.MinValue && length >= minLength
                    && maxLength != int.MinValue && length <= maxLength;
            }
            return true;
        }

        public override string GetValidateString()
        {
            if (minLength != int.MinValue && maxLength != int.MinValue)
            {
                return string.Format("rangelength:[{0},{1}]", minLength.ToString(), maxLength.ToString());
            }
            else if (minLength != int.MinValue)
            {
                return "minlength:" + minLength.ToString();
            }
            else if (maxLength != int.MinValue)
            {
                return "maxlength:" + maxLength.ToString();
            }
            return "";
        }
    }
}
