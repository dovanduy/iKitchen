using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iKitchen.Web.Models
{
    public static class WebConstants
    {
        public static readonly string CurrentUser = "CurrentUser";
        public static readonly string CheckCode = "CheckCode";
        /// <summary>
        /// 角色Id - 普管用户
        /// </summary>
        public const int RoleId_User = 1;
        /// <summary>
        /// 角色Id - 未启用用户
        /// </summary>
        public const int RoleId_Admin = 2;
        /// <summary>
        /// 角色Id - 超管用户
        /// </summary>
        public const int RoleId_System = 3;

        /// <summary>
        /// 首页广告位宽
        /// </summary>
        public const int HomepageAdWidth = 215;
        /// <summary>
        /// 首页广告位高
        /// </summary>
        public const int HomepageAdHeight = 300;
        /// <summary>
        /// 详情页广告位宽
        /// </summary>
        public const int DetailPageAdWidth = 630;
        /// <summary>
        /// 详情页广告位高
        /// </summary>
        public const int DetailPageAdHeight = 100;

    }
}