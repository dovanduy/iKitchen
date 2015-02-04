using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SunTzu.Core.Authorization;
using SunTzu.Utility;
using SunTzu.Web.Login;
using SunTzu.Core.Data;
using iKitchen.Linq;
using iKitchen.Web.Models;
using SunTzu.Web;

namespace iKitchen.Web.Controllers
{
    public class HomeController : iKitchenController
    {
        [Login]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RandomImage()
        {
            var validateCode = RandomNumberImage.RndNum(4); // 4位数字验证
            //Session[WebConstants.CheckCode] = validateCode;
            var imageContent = RandomNumberImage.CreateImages(validateCode);
            return File(imageContent, "image/Jpeg");
        }

        public ActionResult AboutUs()
        {
            return View();
        }
    }
}
