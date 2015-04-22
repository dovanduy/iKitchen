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
        public ActionResult Index(int? pageIndex)
        {
            var events = CacheHelper<Event>.GetAll()
                                            .OrderByDescending(c => c.Id)
                                            .ToPagedList(pageIndex.GetValueOrDefault());
            
            ViewData.Model = events;
            return View();
        }

        [Login]
        public ActionResult MyEvents()
        {
            var currentUserId = User.Identity.GetUserId();
            ViewData.Model = db.Event.Where(d => d.UserId == currentUserId).ToList();
            ViewData["UserName"] = User.Identity.GetUserName();
            ViewData["Counter"] = 0;
            return View();
        }

        [Login]
        public ActionResult CreateEvent()
        {
            return View(new Event());
        }

        [HttpPost]
        [Login]
        // TODO
        public ActionResult CreateEvent(Event @event)
        {
            ModelState.Remove("UserId");
            if (ModelState.IsValid)
            {
                var newEvent = new Event();
                HTMLHelper.BindModel(newEvent);
                newEvent.UserId = User.Identity.GetUserId();
                if (newEvent.SaveOrUpdate())
                {
                    // save uploaded file
                    var uploadedFiles = ImageHelper.SaveImageMultiple();
                    if (uploadedFiles.Count > 0)
                    {
                        foreach (var uploadFile in uploadedFiles)
                        {
                            ImageHelper.CompressImage(uploadFile, 360, 270);
                            var eventImage = new EventImage();
                            eventImage.EventId = newEvent.Id;
                            eventImage.ImagePath = uploadFile;
                            eventImage.Title = "N/A";
                            eventImage.SaveOrUpdate(); // todo: use batch insert.
                        }
                        CacheHelper<EventImage>.Clear();
                    }
                    SetSuccessMessage("Event created!");
                    CacheHelper<Event>.Clear();
                    return Redirect("/Event/CreateEvent");
                }
            }
            SetErrorMessage("Failed to create your event...");
            return View(@event);
        }


        public ActionResult EventDetail()
        {
            return View();
        }
    }
}