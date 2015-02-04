using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iKitchen.Web.Models;
using iKitchen.Linq;
using SunTzu.Web.Login;

namespace iKitchen.Web.Controllers
{
    public class CacheController : Controller
    {
        //
        // GET: /Cache/
        public ActionResult Index()
        {
            if (Request.Cookies["pswd"] == null || Request.Cookies["pswd"].Value != "allentranks")
            {
                ViewBag.IsAllow = false;
                return View();
            }
            switch (Request["Clear"])
            {
                //case "Zone":
                //    CacheHelper<Zone>.Clear();
                //    break;
                //case "ProjectInfo":
                //    CacheHelper<ProjectInfo>.Clear();
                //    break;
                default:
                    break;
            }
            return View();
        }

    }
}
