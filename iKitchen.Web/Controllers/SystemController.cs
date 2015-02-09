using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SunTzu.Web.Login;
using iKitchen.Linq;
using iKitchen.Web.Models;
using SunTzu.Utility;
using SunTzu.Web;

namespace iKitchen.Web.Controllers
{
    [Login(RoleId = WebConstants.RoleId_Admin)]
    public class SystemController : BaseController<SystemConfiguration>
    {
        //
        // GET: /System/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 参数设置
        /// </summary>
        /// <returns></returns>
        public ActionResult Configuration()
        {
            return View(SystemConfiguration);
        }

        [HttpPost]
        public ActionResult SaveConfig()
        {
            // 系统参数
            var systemConfiguration = db.SystemConfiguration.Single();
            HTMLHelper.BindModel(systemConfiguration);

            systemConfiguration.UpdateOn = DateTime.Now;

            db.SaveChanges();
            iKitchenConfig.SystemConfiguration = systemConfiguration;
            SetSuccessMessage();

            return RedirectToAction("Configuration");
        }
    }
}
