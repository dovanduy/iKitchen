using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunTzu.Web.EntityValidate
{
    class Constants
    {
        /// <summary>
        /// 验证IP的正则表达式字符串
        /// </summary>
        public const string EXPRESSION_IP = @"^(d{1,2}|1dd|2[0-4]d|25[0-5]).(d{1,2}|1dd|2[0-4]d|25[0-5]).(d{1,2}|1dd|2[0-4]d|25[0-5]).(d{1,2}|1dd|2[0-4]d|25[0-5])$";

        /// <summary>
        /// 验证Email的正则表达式字符串
        /// </summary>
        public const string EXPRESSION_EMAIL = @"^([w-.]+)@(([[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}.)|(([w-]+.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(]?)$";
    }
}
