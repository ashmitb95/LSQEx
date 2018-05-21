using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LSQEx.Controllers
{
    public class BaseController : Controller
    {
        public static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected override void OnException(ExceptionContext filterContext)
        {
            var a = 5;
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                //Return Json
                filterContext.Result = new JsonResult()
                {
                    Data = filterContext.Exception.Message,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
            }
            else
            {
                //Normal Exception
                base.OnException(filterContext);

            }
            if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
            {
                return;
            }

            // if the request is AJAX return JSON else view.

        }
    }
}