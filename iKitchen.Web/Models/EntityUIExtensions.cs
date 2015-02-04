using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iKitchen.Linq;
using SunTzu.Utility;

namespace iKitchen.Web.Models
{
    public static class EntityUIExtensions
    {
        #region User

        public static string GetRole(this ApplicationUser user)
        {
            return CacheHelper<Role>.GetById(user.RoleId).Title;
        }

        public static string GetUserNameAndMobile(this ApplicationUser user)
        {
            return user.UserName + " " + user.Mobile;
        }
        #endregion
    }
}