using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunTzu.Web.Login
{
    public class LoginAttribute : Attribute
    {
        public int RoleId { get; set; }
        public bool IsRequired { get; set; }
        public LoginAttribute()
        {
            IsRequired = true;
        }

        public LoginAttribute(bool isRequired)
        {
            IsRequired = isRequired;
        }

        public LoginAttribute(int roleId)
        {
            RoleId = roleId;
            IsRequired = true;
        }

        public LoginAttribute(bool isRequired, int roleId)
        {
            RoleId = roleId;
            IsRequired = isRequired;
        }
    }
}
