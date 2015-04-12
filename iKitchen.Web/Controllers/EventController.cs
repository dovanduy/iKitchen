using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using iKitchen.Web.Models;
using SunTzu.Web.Login;
using SunTzu.Core.Data;
using SunTzu.Utility;
using iKitchen.Linq;
using SunTzu.Web;

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

        [Login]
        public ActionResult MyEvents()
        {
            var currentUserId = User.Identity.GetUserId();
            ViewData.Model = db.Event.Where(d => d.UserId == (string)currentUserId).ToList();
            ViewData["UserName"] = User.Identity.GetUserName();
            ViewData["Counter"] = 0;
            return View();
        }

        [Login]
        public ActionResult CreateEvent()
        {
            return View();
        }

        [HttpPost]
        [Login]
        // TODO
        public ActionResult CreateEvent(CreateEventViewModel model)
        {
            var newEvent = new Event();
            newEvent.Address = model.Address;
            newEvent.Description = model.Description;
            newEvent.EventTime = DateTime.Now;
            newEvent.Title = model.Title;
            newEvent.UserId = User.Identity.GetUserId();
            newEvent.Summary = "nice";
            newEvent.IsOneTime = true;
            newEvent.GuestLimitCount = 5;
            newEvent.State = 1;
            newEvent.Price = 100;
            newEvent.SaveOrUpdate();

            return View();
        }
	}
}