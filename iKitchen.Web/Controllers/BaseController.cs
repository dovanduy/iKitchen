using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using System.Web.Mvc;
using iKitchen.Linq;
using iKitchen.Web.Models;
using log4net;
using Microsoft.Practices.Unity;
using SunTzu.Core.Authorization;
using SunTzu.Core.Data;
using SunTzu.Utility;
using SunTzu.Web;
using SunTzu.Web.Login;
using System.Text;
using System.Collections.Specialized;

namespace iKitchen.Web.Controllers
{
    public class BaseController<T> : iKitchenController where T : class, IEntity, new()
    {
        private int pageSize = ConfigurationManager.AppSettings["PageSize"].ParseToInt(20);
        protected int numPerPage
        {
            get
            {
                // 从 cookie 读入列表行数
                var cookie = System.Web.HttpContext.Current.Request.Cookies["numPerPage"];
                return cookie == null ? pageSize : cookie.Value.ParseToInt(20);
            }
        }

        protected IQueryable<T> table
        {
            get
            {
                return db.Set<T>();
            }
        }

        #region Base Action

        public virtual ActionResult List(int? page)
        {
            if (Request["keywords"].IsNotNullOrEmpty() && (typeof(T).GetInterface("ITitle") != null))
            {
                ViewData.Model = table
                    .Where("State > -1 && Title.Contains(@0)", Request["keywords"]) // 过滤被逻辑删除的数据
                    .OrderByDescending(c => c.Id)
                    .ToPagedList(page.GetValueOrDefault(), numPerPage);
            }
            else
            {
                ViewData.Model = table
                    .Where(c => c.State > -1) // 过滤被逻辑删除的数据
                    .OrderByDescending(c => c.Id)
                    .ToPagedList(page.GetValueOrDefault(), numPerPage);
            }
            return View();
        }

        public virtual ActionResult Edit(int? id)
        {
            T entity = id > 0
                ? table.FirstOrDefault(c => c.Id == id) // 编辑
                : new T(); // 添加
            ViewData.Model = entity ?? new T();
            return View();
        }

        public virtual ActionResult Detail(int? id)
        {
            T entity = id > 0
                ? table.FirstOrDefault(c => c.Id == id)
                : new T();
            ViewData.Model = entity ?? new T();
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Save(int? id)
        {
            T entity = id > 0
                ? table.FirstOrDefault(c => c.Id == id) // 编辑
                : new T(); // 添加

            HTMLHelper.BindModel(entity);

            if (entity.SaveOrUpdate())
            {
                CacheHelper<T>.Clear();
                SetMessage("保存成功！");
                return RedirectToAction("List");
            }
            else
            {
                SetMessage("保存失败……");
                return View("Edit", entity);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual JsonResult Delete(string ids)
        {
            if (ids.IsNotNullOrEmpty())
            {
                var entityList = new List<IEntity>();
                int did;
                foreach (var id in ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    did = id.ParseToInt();
                    T entity = table.FirstOrDefault(c => c.Id.Equals(did));
                    //CacheManager<T>.ClearCache();
                    if (entity != null)
                    {
                        entityList.Add(entity);
                    }
                }
                if (entityList.Delete(false))
                {
                    CacheHelper<T>.Clear();
                    return ReturnResultFactory.DeleteSuccess.ToJsonResult();
                }
            }
            return ReturnResultFactory.Failed.ToJsonResult();
        }
        #endregion
    }
    public class iKitchenController : Controller
    {
        protected ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 系统配置参数
        /// </summary>
        protected SystemConfiguration SystemConfiguration
        {
            get { return iKitchenConfig.SystemConfiguration; }
        }

        [Dependency]
        public IAuthorization Authorization
        {
            get;
            set;
        }

        protected ApplicationUser CurrentUser
        {
            get { return AccountHelper.GetCurrentUser(); }
        }

        protected iKitchenDataContext db
        {
            get { return DataContextManager.GetContext(); }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var container = HttpContext.Application["Container"] as UnityContainer;
            container.BuildUp(this);

            ViewBag.IsLogin = Authorization.IsLogin;
            ViewBag.IsAdmin = Authorization.IsAdmin;
            ViewBag.IsSuperAdmin = Authorization.IsSuperAdmin;
            ViewBag.CurrentUserName = Authorization.CurrentUserName;
            ViewBag.RoleId = AccountHelper.GetCurrentUser().RoleId; // iKitchen only

            CheckAuthorization(filterContext);

            ViewBag.SystemConfiguration = SystemConfiguration;
        }

        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            DataContextManager.Clear();
            base.OnResultExecuted(filterContext);
        }


        protected void SetSuccessMessage(string message = "保存成功！")
        {
            SetMessage(message, "success");
        }

        protected void SetMessage(string message)
        {
            SetMessage(message, "info");
        }

        protected void SetErrorMessage(string message = "保存失败...")
        {
            SetMessage(message, "danger");
        }

        private void SetMessage(string message, string type)
        {
            TempData["Message"] = message;
            TempData["MessageType"] = type;
        }

        protected void SaveOpLog(string opType, string opDescription, string opRemark, bool isSuccess = true)
        {
            if (Authorization.CurrentUserName == "allentranks")
                return;

            var opLog = new OpLog()
            {
                OpType = opType,
                OpDescription = opDescription,
                OpRemark = opRemark,
                OpResult = isSuccess ? "成功" : "失败",
                UserName = Authorization.CurrentUserName
            };
            opLog.SaveOrUpdate();

        }
        #region Authoriztion

        /// <summary>
        /// 检查登录权限
        /// </summary>
        /// <param name="filterContext"></param>
        private void CheckAuthorization(ActionExecutingContext filterContext)
        {
            LoginAttribute[] actionLoginAttributes = (LoginAttribute[])filterContext.ActionDescriptor.GetCustomAttributes(typeof(LoginAttribute), false);
            LoginAttribute[] controllerLoginAttributes = (LoginAttribute[])filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(LoginAttribute), false);
            bool isLoginRequired = CheckIsLoginRequired(actionLoginAttributes, controllerLoginAttributes);
            if (isLoginRequired)
            {
                // 要求登录
                if (!Authorization.IsLogin)
                {
                    if (IsAjax())
                    {
                        filterContext.Result = ReturnResultFactory.SessionTimeOut.ToJsonResult();
                    }
                    else
                    {
                        Response.Redirect("/Account/Login?returnUrl=" + Request.RawUrl);
                    }
                }
                else
                {
                    int requiredRole = CheckRoleId(actionLoginAttributes);
                    if (requiredRole == 0)
                    {
                        requiredRole = CheckRoleId(controllerLoginAttributes);
                    }
                    if (!Authorization.HasAuthority(requiredRole))
                    {
                        Response.Redirect("/Error/NotAllow");
                    }
                }
            }
        }

        /// <summary>
        /// 检查是否需要登录
        /// </summary>
        /// <param name="loginAttributes"></param>
        /// <returns></returns>
        private bool CheckIsLoginRequired(LoginAttribute[] actionLoginAttributes, LoginAttribute[] controllerAttributes)
        {
            if (actionLoginAttributes != null && actionLoginAttributes.Length > 0)
            {
                return actionLoginAttributes[0].IsRequired;
            }
            if (controllerAttributes != null && controllerAttributes.Length > 0)
            {
                return controllerAttributes[0].IsRequired;
            }
            return false;
        }

        /// <summary>
        /// 检查角色Id
        /// </summary>
        /// <param name="loginAttributes"></param>
        /// <returns></returns>
        private int CheckRoleId(LoginAttribute[] loginAttributes)
        {
            if (loginAttributes != null && loginAttributes.Length > 0)
            {
                return loginAttributes[0].RoleId;
            }
            return 0;
        }

        private bool IsAjax()
        {
            return Request.Headers["ajax"].IsNotNullOrEmpty();
        }
        #endregion

        protected void LogHttpRequest(string title, bool terminal)
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine(title);

            message.AppendLine("Request URL=" + Request.RawUrl);
            message.AppendLine("Request Method=" + Request.HttpMethod);

            NameValueCollection headers = Request.Headers;
            foreach (string header in headers)
            {
                message.AppendLine("Http Header, name=" + header + ", value=" + headers[header]);
            }
            NameValueCollection param = Request.Params;
            foreach (string paramItem in param)
            {
                message.AppendLine("Http Param, name=" + paramItem + ", value=" + param[paramItem]);
            }
            message.AppendLine("Current User: " + AccountHelper.GetCurrentUser());
            logger.Error(message);

            if (terminal)
                throw new Exception(title);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            logger.Error("未处理的异常！", filterContext.Exception);
            LogHttpRequest("异常信息", false);
            base.OnException(filterContext);
        }
    }
}
