using LSQEx.BL;
using LSQEx.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace LSQEx.Controllers
{
    [HandleError]
    public class HighlightsController : BaseController
    {

        public static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [ChildActionOnly]
        public ActionResult Highlights()
        {
            List<NewsModel> HighlightsList;

            try
            {
                HighlightsList = new NewsArticles().GetHighlightsList();
            }
            catch (Exception)
            {

                throw;
            }
            return PartialView(HighlightsList);
        }

        public ActionResult RetrieveHighlights(int id, String category)
        {
            NewsModel Highlight = null;
            try
            {
                Highlight = (new NewsArticles(id).GetHighlightNewsItem());
            }
            catch (Exception)
            {
                throw;
            }
            //throw new Exception("Oops");
            return PartialView("RetrieveNews", Highlight);
        }
    }
}