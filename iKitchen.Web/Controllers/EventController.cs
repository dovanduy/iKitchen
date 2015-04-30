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
using System.Net;

namespace iKitchen.Web.Controllers
{
    public class EventController : BaseController<Event>
    {
        //
        // GET: /Event/
        public ActionResult Index(int? pageIndex)
        {
            var events = CacheHelper<Event>.GetAll()
                                            .Where(c => c.State != -1)
                                            .OrderByDescending(c => c.Id)
                                            .ToPagedList(pageIndex.GetValueOrDefault());

            ViewData.Model = events;
            return View();
        }

        [Login]
        public ActionResult MyEvents(int? pageIndex)
        {
            var currentUserId = User.Identity.GetUserId();
            var events = CacheHelper<Event>.GetAll()
                                            .Where(c => c.UserId == currentUserId && c.State != -1)
                                            .OrderByDescending(c => c.Id)
                                            .ToPagedList(pageIndex.GetValueOrDefault());
            ViewData.Model = events;
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

        [Login]
        [HttpPost]
        public JsonResult Remove(int id)
        {
            var @event = db.Event.Find(id);
            if (@event == null || @event.State == -1)
            {
                var result = ReturnResultFactory.Failed;
                result.message += " Event doesn't exist.";
                return result.ToJsonResult();
            }
            if (@event.UserId != User.Identity.GetUserId())
            {
                var result = ReturnResultFactory.Failed;
                result.message += " You don't have the permission to delete this event.";
                return result.ToJsonResult();
            }

            if (@event.Delete())
            {
                CacheHelper<Event>.Clear();
                var result = ReturnResultFactory.DeleteSuccess;
                return result.ToJsonResult();
            }
            else
            {
                var result = ReturnResultFactory.Failed;
                return result.ToJsonResult();
            }
        }

        [HttpPost]
        public JsonResult Join(int id)
        {
            var @event = db.Event.Find(id);
            if (@event == null || @event.State == -1)
            {
                var result = ReturnResultFactory.Failed;
                result.message = "Failed to join... Event doesn't exist.";
                return result.ToJsonResult();
            }

            var currentUserId = User.Identity.GetUserId();
            if (db.EventUser.Any(c => c.EventId == id && c.UserId == currentUserId))
            {
                var result = ReturnResultFactory.Failed;
                result.message = "Failed to join... You have already joined in this Event.";
                return result.ToJsonResult();
            }

            if (@event.GuestLimitCount > 0)
            {
                var joinedCount = db.EventUser.Count(c => c.EventId == id);
                if (joinedCount >= @event.GuestLimitCount)
                {
                    var result = ReturnResultFactory.Failed;
                    result.message = "Failed to join... No vacancy for this Event at this moment.";
                    return result.ToJsonResult();
                }
            }

            var eventUser = new EventUser();
            eventUser.UserId = User.Identity.GetUserId();
            eventUser.EventId = id;
            eventUser.UnitPrice = @event.Price;
            eventUser.IsPaid = @event.Price == 0;
            if (eventUser.SaveOrUpdate())
            {
                var result = ReturnResultFactory.Success;
                result.message = "Joined successfully!";
                result.action = ReturnResultFactory.Redirect;
                result.url = "/Event/Detail/" + id;
                var message = "You have joined this event successfully.";
                SetSuccessMessage(message);
                CacheHelper<EventUser>.Clear();
                return result.ToJsonResult();
            }
            else
            {
                var result = ReturnResultFactory.Failed;
                result.message = "Failed to join... ";
                return result.ToJsonResult();
            }
        }
    }
}