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
            var oldNumber = iKitchenConfig.SystemConfiguration.AssessmentSequenceNumber;
            HTMLHelper.BindModel(systemConfiguration);
            if (oldNumber > systemConfiguration.AssessmentSequenceNumber)
            {
                ModelState.AddModelError("AssessmentSequenceNumber", "新流水号不能够比当前流水号小");
                SetErrorMessage("保存失败！新流水号不能够比当前流水号小");
            }
            else
            {
                systemConfiguration.UpdateOn = DateTime.Now;

                db.SaveChanges();
                iKitchenConfig.SystemConfiguration = systemConfiguration;
                SetSuccessMessage();
            }
            return RedirectToAction("Configuration");
        }

        /// <summary>
        /// 挂牌量统计
        /// </summary>
        /// <returns></returns>
        public ActionResult InquiryConfiguration()
        {
            return View(SystemConfiguration);
        }

        [HttpPost]
        public ActionResult InquiryConfiguration(int? id)
        {
            // 系统参数
            var systemConfiguration = db.SystemConfiguration.Single();
            var oldNumber = iKitchenConfig.SystemConfiguration.AssessmentSequenceNumber;
            HTMLHelper.BindModel(systemConfiguration);

            systemConfiguration.UpdateOn = DateTime.Now;

            db.SaveChanges();
            iKitchenConfig.SystemConfiguration = systemConfiguration;
            SetSuccessMessage();
            return View(SystemConfiguration);
        }
    }
}
