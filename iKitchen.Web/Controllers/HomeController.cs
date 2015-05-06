using System.Web.Mvc;
using SunTzu.Utility;
using iKitchen.Web.Models;
using SunTzu.Web;

namespace iKitchen.Web.Controllers
{
    public class HomeController : iKitchenController
    {
        //[RequireHttps]
        public ActionResult Index()
        {
            if(Authorization.IsLogin || !Request["isreturn"].IsNotNullOrEmpty()) // todo: use cookie
            {
                return View();
            }
            return View("NewCustomer");
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

        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact(ContactUsViewModel model)
        {
            SendMailMessage(model.Email, "service@iKitchen.nz", "Customer Contact Message FROM: " + model.UserName, model.Message);
            SetSuccessMessage("Thanks for your message, we will contact you ASAP~");
            return View();
        }
    }
}
