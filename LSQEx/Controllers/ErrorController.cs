using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LSQEx.Controllers
{
        // GET: Error
        public class ErrorController : BaseController
        {
        public static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ActionResult Index()
            {
            try
            {

            }
            catch (Exception e)
            {
                Log.Info("Custom Error", e);
                Log.Debug("Custom Error thrown", e);
            }
                return View("Error");
                
            }

            public ActionResult Unauthorized()
        {
            return View();
        }

        public ActionResult Invalid()
        {
            return View();
        }

    }
    
}