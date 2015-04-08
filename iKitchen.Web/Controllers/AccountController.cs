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
    [Authorize]
    public class AccountController : iKitchenController
    {
        public AccountController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
            UserManager.UserValidator = new UserValidator<ApplicationUser>(UserManager) { AllowOnlyAlphanumericUserNames = false };
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }
        [Login(RoleId = WebConstants.RoleId_Admin)]
        public ActionResult Index()
        {
            var db = new ApplicationDbContext();
            var users = db.Users.AsQueryable().Where(c => c.RoleId != WebConstants.RoleId_System);

            var keyword = Request["Keyword"];
            if (keyword.IsNotNullOrEmpty())
            {
                users = users.Where(c => c.UserName.Contains(keyword)
                                      || c.Email.Contains(keyword)
                                      || c.Mobile.Contains(keyword));
            }

            var roleId = Request["RoleId"].ParseToInt();
            if (roleId > 0)
            {
                users = users.Where(c => c.RoleId == roleId);
            }

            var state = Request["State"].ParseToInt(-1);
            if (state >= 0)
            {
                users = users.Where(c => c.State == state);
            }

            ViewData.Model = users.OrderByDescending(c => c.CreateOn).ToPagedList(Request["page"].ParseToInt());
            return View();
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.UserName, model.Password);
                if (user != null)
                {
                    await SignInAsync(user, model.RememberMe);
                    Session[WebConstants.CurrentUser] = user;
                    var log = new SignInLog();
                    log.UserId = model.UserName;
                    log.IP = Request.UserHostAddress;
                    log.IsSuccess = true;
                    log.SaveOrUpdate();
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    var log = new SignInLog();
                    log.UserId = model.UserName;
                    log.IP = Request.UserHostAddress;
                    log.IsSuccess = false;
                    log.SaveOrUpdate();
                    ModelState.AddModelError("", "The username and password don't match");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [Login]
        public ActionResult EditProfile()
        {
            return View(AccountHelper.GetCurrentUser());
        }

        [Login(RoleId = WebConstants.RoleId_Admin)]
        public ActionResult Edit(string uid)
        {
            var db = new ApplicationDbContext();
            var user = db.Users.FirstOrDefault(c => c.Id == uid);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Login(RoleId = WebConstants.RoleId_Admin)]
        public ActionResult Edit()
        {
            var db = new ApplicationDbContext();
            string uid = Request["uid"];
            var user = db.Users.FirstOrDefault(c => c.Id == uid);
            HTMLHelper.BindModel(user);
            try
            {
                db.SaveChanges();
                var password = Request["Password"];
                if (password.IsNotNullOrEmpty())
                {
                    UserManager.RemovePassword(user.Id);
                    UserManager.AddPassword(user.Id, password);
                    SetSuccessMessage("保存成功！密码已重置！");
                }
                else
                {
                    SetSuccessMessage();
                }
            }
            catch (Exception e)
            {
                logger.Debug("修改用户失败！", e);
                SetErrorMessage();
            }

            return View(user);
        }

        // POST: /Account/Register
        [HttpPost]
        [Login]
        [ValidateAntiForgeryToken]
        public ActionResult SaveProfile()
        {
            var db = new ApplicationDbContext();
            var userId = AccountHelper.GetCurrentUser().Id;
            var user = db.Users.FirstOrDefault(c => c.Id == userId);
            if (TryUpdateModel(user))
            {
                db.SaveChanges();
                Session[WebConstants.CurrentUser] = user;
                SetSuccessMessage("保存成功");
            }
            else
            {
                SetErrorMessage("保存失败");
            }

            return RedirectToAction("EditProfile");
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() { UserName = model.UserName };
                user.Email = "zihao.chen31@gmail.com";
                user.RoleId = 1;
                user.Mobile = "13899032456";
                user.Sex = 0;
                user.CreateOn = DateTime.Now;
                user.UpdateOn = DateTime.Now;
                var result = UserManager.Create(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInAsync(user, isPersistent: false);
                    Session[WebConstants.CurrentUser] = user;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    AddErrors(result);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        //
        // GET: /Account/Register
        [Login(RoleId = WebConstants.RoleId_Admin)]
        public ActionResult Create()
        {
            return View(new CreateUserViewModel() { CreateOn = DateTime.Now });
        }

        //
        // POST: /Account/Create
        [HttpPost]
        [Login(RoleId = WebConstants.RoleId_Admin)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser();
                user.Email = model.Email;
                user.Mobile = model.Mobile;
                user.RoleId = model.RoleId;
                user.Sex = 0;
                user.UserName = model.UserName;
                user.CreateOn = model.CreateOn;
                user.UpdateOn = DateTime.Now;
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    SetSuccessMessage(model.UserName + "已经创建");
                    CacheHelperApplicationUser.Clear();
                    return RedirectToAction("Create", "Account");
                }
                else
                {
                    AddErrors(result);
                    SetErrorMessage();
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Password
        [Login]
        public ActionResult Password(ManageMessageId? message)
        {
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Password");
            return View();
        }

        //
        // POST: /Account/Password
        [Login]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Password(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Password");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = UserManager.ChangePassword(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        SetSuccessMessage("密码已经更改");
                        return RedirectToAction("Password");
                    }
                    else
                    {
                        SetErrorMessage("更改密码失败，请确认旧密码输入是否正确");
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [Login]
        public ActionResult Log(int? page)
        {
            var logList = db.SignInLog.Where(c => c.UserId == Authorization.CurrentUserName)
                                        .OrderByDescending(c => c.Id)
                                        .ToPagedList(page.GetValueOrDefault(), SystemConfiguration.PageSize);
            return View(logList);
        }

        [Login(RoleId = WebConstants.RoleId_Admin)]
        public ActionResult AllLog()
        {
            return View();
        }


        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Profile()
        {
            return View();
        }


        #region ExternalLogin
        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
            }
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
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
            newEvent.UserId = "2";
            newEvent.Summary = "nice";
            newEvent.IsOneTime = true;
            newEvent.GuestLimitCount = 5;
            newEvent.State = 1;
            newEvent.Price = 100;
            newEvent.SaveOrUpdate();

            return View();
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser() { UserName = model.UserName };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId=allentranks";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}