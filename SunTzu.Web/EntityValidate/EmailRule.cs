using System.Text.RegularExpressions;

namespace SunTzu.Web.EntityValidate
{
    class EmailRule : AbstractValidateRule
    {
        public override bool DoValidate(object value)
        {
            if (value != null)
            {
                //验证IPv4 - todo: 验证IPv6
                return Regex.IsMatch(value.ToString(), Constants.EXPRESSION_EMAIL);
            }
            return false;
        }

        public override string GetValidateString()
        {
            return "email:true";
        }
    }
}
