using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iKitchen.Linq;

namespace iKitchen.Web.Controllers
{
    public class EventController : BaseController<Event>
    {
        //
        // GET: /Event/
        public ActionResult Index()
        {
            return View();
        }
	}
}