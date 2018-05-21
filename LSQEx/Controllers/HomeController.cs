using LSQEx.BL;
using LSQEx.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace LSQEx.Controllers
{
    [HandleError]
    [ValidateInput(false)]
    public class HomeController : Controller
    {
        List<NewsModel> BreakingNewsList;
        //List<CategoriesModel> Categories = new NewsArticles().GetCategories();

        #region Public Methods      ##

        public static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [Authorize]
        public ActionResult News()
        {
            HomeNewsModel HomeNewsModel = null;
            
            try
            {
                ViewBag.IsNewsPage = true;
                NewsArticles NewsItems = new NewsArticles();
                ViewBag.Title = "Home";
                HomeNewsModel = new HomeNewsModel();
                HomeNewsModel.AllNews = NewsItems.GetAllNews();
                HomeNewsModel.NewsCategory = NewsItems.GetCategories();
                //HomeNewsModel.selectList = HomeNewsModel.NewsCategory.Select(m => new SelectCategory { ID = m.ID, Name = m.Name }).ToList();
                HomeNewsModel.SelectList = new List<SelectListItem>();
                //HomeNewsModel.SelectList = new MultiSelectList(HomeNewsModel.NewsCategory.Select(m => new SelectCategory { ID = m.ID, Name = m.Name }).ToList());
                //List <SelectListItem> FilterList = new List<SelectListItem>();
                foreach (var item in NewsItems.RetrieveAllCategories())
                {
                    HomeNewsModel.SelectList.Add(new SelectListItem
                    {
                        Selected=item.Filter,
                        Text = item.Name,
                        Value = Convert.ToString(item.ID)
                    });
                }
            }
            catch (Exception e)
            {
                Log.Info("Exception Thrown:"+ e);
                Log.Debug("Exception thrown" + e);
                Log.Error("Error", e);
            }
            return View(HomeNewsModel);
        }

        public ActionResult RSSFeed()
        {
            List<NewsModel> CarouselNews = new List<NewsModel>();
            try
            {



                string RSSURL = "https://economictimes.indiatimes.com/rssfeedstopstories.cms";
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                WebClient WebClient = new WebClient();
                string RSSData = WebClient.DownloadString(RSSURL);
                XDocument XMLData = XDocument.Parse(RSSData);
                foreach (var NewsItem in XMLData.Descendants("item"))
                {
                    NewsModel NewsModel = new NewsModel();
                    NewsModel.Category = "Just In";
                    NewsModel.Headline = (string)NewsItem.Element("title");
                    NewsModel.ImageURL = (string)NewsItem.Element("image");
                    NewsModel.Text = (string)NewsItem.Element("description");
                    NewsModel.Publish_Date = (DateTime)NewsItem.Element("pubDate");

                    CarouselNews.Add(NewsModel);
                }
            }
            catch(Exception e)
            {
                throw e;
            }

            return PartialView("RSSFeed", CarouselNews);
        }


        public ActionResult ShowFeed(string headline, string text, string category)
        {
            NewsModel NewsModel = new NewsModel();
            NewsModel.Text = text;
                NewsModel.Category = category;
                NewsModel.Headline = headline;
            NewsModel.TimeAgo = "Less than an hour ago";
            NewsModel.Source = "Economic Times";


            return View("RetrieveNews", NewsModel);
        }

        [ChildActionOnly]
        public ActionResult BreakingNews()
        {
            List<string> NewsList = new List<string>();
            List<string> BreakingNews= new List<string> { "http://indianexpress.com/section/world/feed/", "http://indianexpress.com/section/india/feed/", "http://indianexpress.com/section/sports/feed" };
            List<XDocument> XmlList = new List<XDocument>();
            //foreach (var item in BreakingNews)
            //{
            //    if (count < 5)
            //    {
            //        WebClient WebClient = new WebClient();
            //        XmlList.Add(XDocument.Parse(WebClient.DownloadString(item)));
            //        count++;
            //    }
            //    else
            //        break;
            //}
            try
            {
                foreach (var item in BreakingNews)
                {
                    string RSSURL = item;
                    WebClient wclient = new WebClient();
                    string RSSData = wclient.DownloadString(RSSURL);

                    XDocument xml = XDocument.Parse(RSSData);
                    int count = 0;
                    foreach (var NewsItem in xml.Descendants("item"))
                    {
                        if (count < 10)
                        {
                            NewsList.Add((string)NewsItem.Element("title"));
                            count++;
                        }
                        else
                            break;

                    }
                    BreakingNewsList = new List<NewsModel>(new NewsArticles().GetAllNews());
                }
            }
            catch (Exception)
            {

                throw;
            }
            return PartialView(NewsList);
        }
        [ChildActionOnly]
        public ActionResult RetrieveNews(int id, int category)
        {
            NewsModel NewsModel = null;
            try
            {
                NewsModel = (new NewsArticles().GetCurrentNewsItem(id, category));
            }
            catch (Exception e)
            {
                throw e;
            }
            return PartialView("RetrieveNews", NewsModel);
        }

        public ActionResult GetNews(int category, string categoryName,  int page)
        {
            PageModel NewsList = null;
            try
            {
                ViewBag.IsNewsPage = true;
                NewsList = new NewsArticles().GetCategoryNews(category, page);
                ViewBag.Title = categoryName;
            }
            catch (Exception)
            {
                throw;
            }
            return PartialView("GetNews", NewsList);
        }



        public ActionResult Blog()
        {
            HomeNewsModel HomeNewsModel = null;
            try
            {
                HomeNewsModel = new HomeNewsModel();
                HomeNewsModel.NewsCategory = new NewsArticles().GetCategories();
            }
            catch (Exception)
            {

                throw;
            }
            return View(HomeNewsModel);
        }


        public ActionResult Contact()
        {
            HomeNewsModel HomeNewsModel = null;
            try
            {
                HomeNewsModel = new HomeNewsModel();
                HomeNewsModel.NewsCategory = new NewsArticles().GetCategories();
            }
            catch (Exception)
            {

                throw;
            }
            return View(HomeNewsModel);
        }


        public ActionResult About()
        {
            HomeNewsModel HomeNewsModel = null;
            try
            {
                HomeNewsModel = new HomeNewsModel();
                HomeNewsModel.NewsCategory = new NewsArticles().GetCategories();
            }
            catch (Exception)
            {

                throw;
            }
            return View(HomeNewsModel);
        }
        [HttpPost]
        public ActionResult FilterCategories(FormCollection form)
        {
            List<String> itemList   = new List<string>();
            ViewBag.Message         = "Selected Items:\\n";
            List<String> HideFilter = new List<string>();
            List<String> ShowFilter = new List<string>();
            int count               = 0;
            for(int i=0;i<form.Count;i+=2)
            {
                string IsSelected   = "SelectList[" + count + "].Selected";
                string Value        = "SelectList[" + count + "].Value";
                if(form[IsSelected]=="false")
                {
                    HideFilter.Add(form[Value]);
                }
                else
                {
                    ShowFilter.Add(form[Value]);
                }
                count++;
                
            }
            new NewsArticles().SetFilters(HideFilter,ShowFilter);
            return RedirectToAction("News","Home");
        }

        public ActionResult ResetCategories()
        {
            new NewsArticles().ResetFilters();
            return RedirectToAction("News", "Home");
        }


        public ActionResult SearchNews(string search="", string category="", int page=0)
        {

            PageModel SearchResults = new NewsArticles().ReturnSearchResults(search, category, page);
                ViewBag.Title           = "Showing results for: " + search;
            return PartialView("GetNews", SearchResults);
        }

        public string VoteNews(int newsID, int categoryID, bool toggle)
        {
            return new Votes().VoteArticle(newsID, categoryID, toggle);
        }
        #endregion


    }
}