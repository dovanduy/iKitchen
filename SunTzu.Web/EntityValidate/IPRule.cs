using System.Text.RegularExpressions;

namespace SunTzu.Web.EntityValidate
{
    class IPRule : AbstractValidateRule
    {
        public override bool DoValidate(object value)
        {
            if (value != null)
            {
                //验证IPv4 - todo: 验证IPv6
                return Regex.IsMatch(value.ToString(), Constants.EXPRESSION_IP);
            }
            return false;
        }

        public override string GetValidateString()
        {
            throw new System.NotImplementedException();
        }
    }
}
