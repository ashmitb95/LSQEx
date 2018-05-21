using LSQEx.BL;
using LSQEx.DL;
using LSQEx.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace LSQEx.Controllers.Accounts
{
    [ValidateInput(false)]
    public class AdminController : BaseController
    {
        public static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // GET: Admin
        public ActionResult ShowUsers()
        {
            return PartialView(new UserAccounts().GetAllUsers());
        }


        public ActionResult DeleteUser(int userID)
        {
            new UserAccounts().DeleteUser(userID);
            TempData["Success"] = "User has been deleted";
            return RedirectToAction("ShowUsers", "Admin");
        }

        public ActionResult AddNews()
        {
            AddNewsModel AddNewsModel = new AddNewsModel();
            //AddNewsModel.Categories = new NewsArticles().GetCategories();
            AddNewsModel.CanAddNews = new UserAccounts().IsUserAuthorizedToAddNews(HttpContext.User.Identity.Name);
            AddNewsModel.Categories = new NewsArticles().GetCategories().Select(value => new SelectListItem() { Text = value.Name, Value = value.Name });
            return View(AddNewsModel);
        }

        [HttpPost]
        public ActionResult AddNews(FormCollection formCollection)
        {
            NewsArticles NewsArticles = new NewsArticles();
            if(ModelState.IsValid)
            {
                NewsModel News = new NewsModel();
                //News.ID = Convert.ToInt32(formCollection["ID"]);
                News.Category = formCollection["Category"].ToString();
                News.Headline = formCollection["NewsModel.Headline"].ToString();
                News.Text = formCollection["NewsModel.Text"].ToString();
                News.Summary = formCollection["NewsModel.Summary"].ToString();
                News.Source = formCollection["NewsModel.Source"].ToString();
                News.Publish_Date = Convert.ToDateTime(formCollection["NewsModel.Publish_Date"]);
                News.ImageURL = formCollection["NewsModel.ImageURL"].ToString();
                News.CategoryID = Convert.ToString(NewsArticles.GetCategoryID(News.Category));
                News.AddedBy = Convert.ToString(HttpContext.User.Identity.Name);
                NewsArticles.AddNews(News);
                TempData["Success"] = "News article Added successfully";
                AddNewsModel AddNewsModel = new AddNewsModel();
                AddNewsModel.CanAddNews = new UserAccounts().IsUserAuthorizedToAddNews(HttpContext.User.Identity.Name);
                AddNewsModel.Categories = NewsArticles.GetCategories().Select(value => new SelectListItem() { Text = value.Name, Value = value.Name });
                return PartialView(AddNewsModel);
            }
            return View();
            
        }

        public ActionResult SearchUser(string searchValue)
        {
            List<UserModel> SearchResults = new List<UserModel>();
            SearchResults = new UserAccounts().RetrieveSearchResults(searchValue);
            return PartialView("ShowUsers", SearchResults);
        }

        public void SetWriteAccess(int userID, bool toggleAccess)
        {
            new UserAccounts().SetWriteAccess(userID, toggleAccess);

        }

        [HttpGet]
        public ActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCategory(FormCollection form)
        {
            NewsArticles NewsArticles = new NewsArticles();
            if(ModelState.IsValid)
            {
                string Category = Convert.ToString(form["Category"]);
                if (NewsArticles.CheckIfCategoryExists(Category))
                    ModelState.AddModelError("Category", "A duplicate category cannot be added");
                else
                {
                    NewsArticles.AddCategory(Category);
                    TempData["Success"] = "Category Added Successfully!";
                }


            }
            
            return View();
        }
    }
}