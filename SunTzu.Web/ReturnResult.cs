using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SunTzu.Web
{
    //{
    //"statusCode":"200",
    //"message":"操作成功",
    //"navTabId":"",
    //"callbackType":"closeCurrent",
    //"forwardUrl":""
    //}
    [Serializable]
    public class ReturnResult
    {
        public bool success { get; set; }
        public string message { get; set; }
        public string action { get; set; }
        public string callbackType { get; set; }
        public string url { get; set; }

    }

    public static class ReturnResultFactory
    {
        public const string Delete = "delete";
        public const string Redirect = "redirect";
        public const string Reload = "reload";
        public const string None = "none";

        /// <summary>
        /// 
        /// </summary>
        public static ReturnResult AddSuccess
        {
            get
            {
                return new ReturnResult()
                {
                    success = true,
                    message = "Added successfully!",
                    action = Reload,
                    callbackType = ""
                };
            }
        }

        public static ReturnResult EditSuccess
        {
            get
            {
                return new ReturnResult()
                {
                    success = true,
                    message = "Saved successfully!",
                    action = "",
                    callbackType = "closeCurrent"
                };
            }
        }

        public static ReturnResult DeleteSuccess
        {
            get
            {
                return new ReturnResult()
                {
                    success = true,
                    message = "Deleted successfully!",
                    action = Reload
                };
            }
        }

        public static ReturnResult SubmitSuccess
        {
            get
            {
                return new ReturnResult()
                {
                    success = true,
                    message = "Submitted successfully!",
                    action = None
                };
            }
        }

        public static ReturnResult Failed
        {
            get
            {
                return new ReturnResult()
                {
                    success = false,
                    message = "Action failed...",
                    action = None
                };
            }
        }

        public static ReturnResult Success
        {
            get
            {
                return new ReturnResult()
                {
                    success = true,
                    message = "Action done successfully!",
                    action = None
                };
            }
        }

        public static ReturnResult SessionTimeOut
        {
            get
            {
                return new ReturnResult()
                {
                    success = false,
                    message = "Session timed out. Please sign in.",
                    url = "/Account/Login",
                    action = Redirect
                };
            }
        }

        public static JsonResult ToJsonResult(this ReturnResult result)
        {
            var jsonResut = new JsonResult();
            jsonResut.Data = result;
            return jsonResut;
        }

        public static JsonResult ToJsonResult(this List<ReturnResult> result)
        {
            var jsonResut = new JsonResult();
            jsonResut.Data = result;
            return jsonResut;
        }
    }
}
