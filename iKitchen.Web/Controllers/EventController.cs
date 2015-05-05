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

        [Authorize]
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

        [Authorize]
        public ActionResult Create()
        {
            return View(new Event());
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(Event @event)
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
                    return Redirect("/Event/Create");
                }
            }
            SetErrorMessage("Failed to create your event...");
            return View(@event);
        }


        [Authorize]
        public ActionResult Edit(int id)
        {
            var @event = db.Event.Find(id);
            if (@event == null || @event.UserId != User.Identity.GetUserId() || @event.State == -1)
            {
                return HttpNotFound();
            }
            var eventImages = db.EventImage.Where(c => c.EventId == id).ToList();
            ViewBag.EventImages = eventImages;
            ViewData.Model = @event;
            return View("Create");
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(int id, Event @event)
        {
            ModelState.Remove("UserId");
            if (ModelState.IsValid)
            {
                var newEvent = db.Event.Find(id);
                if (newEvent == null || newEvent.UserId != User.Identity.GetUserId() || newEvent.State == -1)
                {
                    return HttpNotFound();
                }
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
                    SetSuccessMessage("Event Saved!");
                    CacheHelper<Event>.Clear();
                    return Redirect("/Event/MyEvents");
                }
            }
            SetErrorMessage("Failed to save your event...");
            ViewData.Model = @event;
            return View("Create");
        }

        [Authorize]
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
        [Authorize]
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
            if (@event.UserId == currentUserId)
            {
                var result = ReturnResultFactory.Failed;
                result.message = "Failed to join... You are the host of this event.";
                return result.ToJsonResult();
            }

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

        [HttpPost]
        [Authorize]
        public JsonResult RemoveImage(int id)
        {
            var eventImage = db.EventImage.Find(id);

            if (eventImage == null)
            {
                var result = ReturnResultFactory.Failed;
                result.message = "Failed to delete... The image doesn't exist.";
                return result.ToJsonResult();
            }
            if (eventImage.Delete(false))
            {
                ImageHelper.DeleteImage(eventImage.ImagePath);
                ImageHelper.DeleteImage("360x270/" + eventImage.ImagePath); // todo: get rid of the hardcoded compress rate
                CacheHelper<EventImage>.Clear();    // todo: refactor by introducing a better Cache machanism
                var result = ReturnResultFactory.Success;
                result.message = "Delete successfully!";
                result.action = ReturnResultFactory.Reload;
                return result.ToJsonResult();
            }
            else
            {
                var result = ReturnResultFactory.Failed;
                result.message = "Failed to delete... ";
                return result.ToJsonResult();
            }
        }

        [HttpPost]
        [Authorize]
        public JsonResult RemoveGuest(int id)
        {
            var eventUser = db.EventUser.Find(id);

            if (eventUser == null)
            {
                var result = ReturnResultFactory.Failed;
                result.message = "Failed to remove guest... The guest doesn't exist.";
                return result.ToJsonResult();
            }

            var @event = db.Event.Find(eventUser.EventId);
            if (@event.UserId != User.Identity.GetUserId())
            {
                var result = ReturnResultFactory.Failed;
                result.message = "Failed to remove guest... Only the host has the permission.";
                return result.ToJsonResult();
            }

            if (eventUser.Delete(false)) 
            {
                // todo: handle any money refund request if any
                CacheHelper<EventUser>.Clear();    // todo: refactor by introducing a better Cache machanism
                var result = ReturnResultFactory.Success;
                result.message = "Guest removed successfully!";
                result.action = ReturnResultFactory.Reload;
                return result.ToJsonResult();
            }
            else
            {
                var result = ReturnResultFactory.Failed;
                result.message = "Failed to remove guest... ";
                return result.ToJsonResult();
            }
        }
    }
}