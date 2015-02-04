using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iKitchen.Web.Controllers;

namespace iKitchen.Web.Areas.iKitchen.Controllers
{
    public class ErrorController : iKitchenController
    {
        //
        // GET: /Error/

        public ActionResult Index()
        {
            return View("Error");
        }

        public ActionResult NotFound()
        {

            return View();
        }

        public ActionResult NotAllow()
        {

            return View();
        }
    }
}
