using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunTzu.Core.Authorization;
using iKitchen.Linq;
using SunTzu.Utility;
using log4net;
using System.Reflection;

namespace iKitchen.Web.Models
{
    public class AccountHelper : IAuthorization
    {
        protected ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ApplicationUser CurrentUser
        {
            get
            {
                var user = HttpContext.Current.Session[WebConstants.CurrentUser] as ApplicationUser;
                return user ?? new ApplicationUser() { Id = "" };
            }
            private set
            {
                HttpContext.Current.Session[WebConstants.CurrentUser] = value;
            }
        }

        #region IAuthorization Members

        public bool Login(string username, string password)
        {
            //var db = DataContextManager.GetContext();
            //var user = db.User.FirstOrDefault(c => c.Username == username
            //    && c.Password == Encryption.MD5Encrypt(password) && c.State == 0);
            //if (user != null)
            //{
            //    CurrentUser = user;

            //    #region 校验业务来源（REA/iKitchen only）

            //    if (this.IsSuperAdmin)
            //    {
            //        var customers = CacheHelper<Assessment>.GetAll().Select(c => c.CustomerName).Distinct();
            //        var unusedCustomers = CacheHelper<Customer>.GetAll().Where(c => !customers.Contains(c.Title)).ToList();
            //        if (unusedCustomers != null && unusedCustomers.Count > 0)
            //        {
            //            var unusedCustomerIds = "";
            //            foreach (var item in unusedCustomers)
            //            {
            //                unusedCustomerIds += "'" + item.Title + "',";
            //            }
            //            unusedCustomerIds = unusedCustomerIds.TrimEnd(',');
            //            logger.InfoFormat("删除无效的Customer: " + unusedCustomerIds);
            //            db.ExecuteCommand("update customer set [state] = -1 where title in (" + unusedCustomerIds + ")");
            //            CacheHelper<Customer>.Clear();
            //        }
            //    }

            //    #endregion
            //    return true;
            //}
            return false; ;
        }

        public bool IsLogin
        {
            get { return HttpContext.Current.User.Identity.IsAuthenticated && !string.IsNullOrEmpty(CurrentUser.Id); }
        }

        /// <summary>
        /// 是否是网管用户
        /// </summary>
        public bool IsAdmin
        {
            get { return CurrentUser.RoleId == 2; }
        }

        /// <summary>
        /// 是否是超管用户
        /// </summary>
        public bool IsSuperAdmin
        {
            get { return CurrentUser.RoleId == 3; }
        }

        public bool HasAuthority(string authorCode)
        {
            return true;
        }

        public bool HasAuthority(int authorCode)
        {
            return CurrentUser.RoleId >= authorCode;
        }

        public void Logout()
        {
            // HttpContext.Current.Session.Clear();
        }

        public string CurrentLoginUserId
        {
            get { return CurrentUser.Id; }
        }

        // not in use
        public int CurrentUserId
        {
            get { return 0; }
        }

        public string CurrentUserName
        {
            get { return CurrentUser.UserName; }
        }

        public void Refresh()
        {
            // CurrentUser = DataContextManager.GetContext().User.FirstOrDefault(c => c.Id == CurrentUserId);
        }
        #endregion
        
        public static ApplicationUser GetCurrentUser()
        {
            var user = HttpContext.Current.Session[WebConstants.CurrentUser] as ApplicationUser;
            return user ?? new ApplicationUser();
        }
    }
}