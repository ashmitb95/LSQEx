using LSQEx.Common;
using LSQEx.DL;
using LSQEx.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace LSQEx.BL
{
    public class NewsArticles
    {
        public static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        int _ID;
        int _Category;
        DataTable _DataTable;
        NewsModel _NewsModel;
        public NewsArticles(int id = 0, int category = 0)
        {
            //remove DataTable initialization IMPORTANT
            _ID = id;
            _Category = category;
            _DataTable = null;
            _NewsModel = null;
        }
        #region public Methods ##

        public void AddCategory(string name)
        {
            new MySqlNewsData().AddCategoryToDB(name);
        }

        public bool CheckIfCategoryExists(string name)
        {
            return new MySqlNewsData().CheckCategoryInDB(name);
        }
        public PageModel ReturnSearchResults(string search, string category, int page)
        {
            List<NewsModel> SearchResult = new List<NewsModel>();

            PageModel PageModel = new PageModel();
            int MaxItems = 6;
            try
            {
                _DataTable = new MySqlNewsData().RetrieveSearchResults(search, category);
                foreach (DataRow row in _DataTable.Rows)
                {
                    _NewsModel              = new NewsModel();
                    _NewsModel.ID           = Convert.ToInt32(row["ID"]);
                    _NewsModel.Headline     = Convert.ToString(row["Headline"]);
                    _NewsModel.Source       = Convert.ToString(row["Source"]);
                    _NewsModel.Text         = Convert.ToString(row["Text"]);
                    _NewsModel.Publish_Date = Convert.ToDateTime(row["PublishDate"]);
                    _NewsModel.Summary      = Convert.ToString(row["Summary"]);
                    _NewsModel.CategoryID   = Convert.ToString(row["CategoryID"]);
                    _NewsModel.ImageURL     = Convert.ToString(row["ImageURL"]);
                    _NewsModel.TimeAgo      = Convert.ToString(new TimeAgo().GetTimeAgo(_NewsModel.Publish_Date));
                    _NewsModel.VoteCount    = Convert.ToInt32(row["VoteCount"]);
                    _NewsModel.AddedBy = Convert.ToString(row["AddedBy"]);
                    var LikeFlag            = new Votes().ReturnVoteData(_NewsModel.ID, new UserAccounts().RetrieveUserID(HttpContext.Current.User.Identity.Name));
                    if (LikeFlag.ID == 0)
                        _NewsModel.HasCurrentUserLiked = null;
                    else
                        _NewsModel.HasCurrentUserLiked = LikeFlag.IsUpvote;
                    _NewsModel.UsersVoted = new Votes().ReturnVoteCount(_NewsModel.ID);

                    SearchResult.Add(_NewsModel);
                }
                PageModel.News          = SearchResult.Skip((page - 1) * MaxItems).Take(MaxItems).ToList();
                double PageCount        = (double)(SearchResult.Count() / Convert.ToDecimal(MaxItems));
                PageModel.PageCount     = (int)Math.Ceiling(PageCount);
                PageModel.CurrentPage   = page;
            }
            
            catch (Exception exception)
            {
                Log.Error(exception);
            }

            return PageModel;

        }

        public void ResetFilters()
        {
            try
            {
                new MySqlNewsData().ResetCategoryFilters();
            }
            catch(Exception e)
            {
                Log.Error(e);
            }
            
        }


        public void SetFilters(List<String> HideFilter, List<string>ShowFilter)
        {
            //Make changes here
            string [] HideFilterArray       = HideFilter.ToArray();
            string[] ShowFilterArray        = ShowFilter.ToArray();
            new MySqlNewsData().ApplyFilters(HideFilterArray, ShowFilterArray);
        }


        
        public NewsModel GetHighlightNewsItem()
        {
            _NewsModel = new NewsModel();
            
            try
            {
                _DataTable = new MySqlNewsData(_ID, _Category).GetHighlightNews();
                foreach (DataRow row in _DataTable.Rows)
                {
                    _NewsModel.ID           = Convert.ToInt32(row["ID"]);
                    _NewsModel.Headline     = Convert.ToString(row["Headline"]);
                    _NewsModel.Source       = Convert.ToString(row["Source"]);
                    _NewsModel.Text         = Convert.ToString(row["Text"]);
                    _NewsModel.Publish_Date = Convert.ToDateTime(row["PublishDate"]);
                    _NewsModel.TimeAgo      = Convert.ToString(new TimeAgo().GetTimeAgo(_NewsModel.Publish_Date));
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }

            return _NewsModel;
        }

        public NewsModel GetCurrentNewsItem(int newsID, int categoryID)
        {
            _NewsModel = new NewsModel();
            
            try
            {
                _DataTable = new MySqlNewsData(newsID, categoryID).GetCurrentNews();
                foreach (DataRow row in _DataTable.Rows)
                {

                    _NewsModel.ID           = Convert.ToInt32(row["ID"]);
                    _NewsModel.Category     = Convert.ToString(row["Category"]);
                    _NewsModel.Headline     = Convert.ToString(row["Headline"]);
                    _NewsModel.Source       = Convert.ToString(row["Source"]);
                    _NewsModel.Text         = Convert.ToString(row["Text"]);
                    _NewsModel.CategoryID   = Convert.ToString(row["CategoryID"]);
                    _NewsModel.Publish_Date = Convert.ToDateTime(row["PublishDate"]);
                    _NewsModel.TimeAgo      = new TimeAgo().GetTimeAgo(_NewsModel.Publish_Date);
                    _NewsModel.VoteCount    = Convert.ToInt32(row["VoteCount"]);
                    _NewsModel.AddedBy = Convert.ToString(row["AddedBy"]);
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }

            return _NewsModel;
        }

        

        

        public List<NewsModel> GetHighlightsList()
        {
            List<NewsModel> Highlights = new List<NewsModel>();
            
            try
            {
                _DataTable = new MySqlNewsData().GetHighlights();
                foreach (DataRow row in _DataTable.Rows)
                {

                    _NewsModel              = new NewsModel();
                    _NewsModel.ID           = Convert.ToInt32(row["ID"]);
                    _NewsModel.Category     = Convert.ToString(row["Category"]);
                    _NewsModel.Headline     = Convert.ToString(row["Headline"]);
                    _NewsModel.Source       = Convert.ToString(row["Source"]);
                    _NewsModel.Text         = Convert.ToString(row["Text"]);
                    _NewsModel.Publish_Date = Convert.ToDateTime(row["PublishDate"]);
                    _NewsModel.TimeAgo      = new TimeAgo().GetTimeAgo(_NewsModel.Publish_Date);
                    Highlights.Add(_NewsModel);
                }
            }


            catch (Exception exception)
            {
                Log.Error(exception);
            }



            return (Highlights);
        }

        public List<CategoriesModel> RetrieveAllCategories()
        {
            List<CategoriesModel> NewsCategories = new List<CategoriesModel>();
            try
            {
                _DataTable = new MySqlNewsData().GetAllCategories();
                foreach (DataRow row in _DataTable.Rows)
                {
                    CategoriesModel Category = new CategoriesModel
                    {
                        Name = Convert.ToString(row["Name"]),
                        ID = Convert.ToInt32(row["ID"]),
                        Filter = Convert.ToBoolean(row["Filter"])
                    };
                    NewsCategories.Add(Category);
                }
            }


            catch (Exception exception)
            {
                Log.Error(exception);
            }
            return NewsCategories;

        }
        public List<CategoriesModel> GetCategories()
        {
            List<CategoriesModel> NewsCategories = new List<CategoriesModel>();
            try
            {
                _DataTable = new MySqlNewsData().GetCategories();
                foreach (DataRow row in _DataTable.Rows)
                {
                    CategoriesModel Category    = new CategoriesModel();
                    Category.Name               = Convert.ToString(row["Name"]);
                    Category.ID                 = Convert.ToInt32(row["ID"]);
                    NewsCategories.Add(Category);
                }
            }


            catch (Exception exception)
            {
                Log.Error(exception);
            }
            return NewsCategories;

        }

        public int GetCategoryID(string category)
        {
            int CategoryID = 0;
            try
            {
                _DataTable = new MySqlNewsData().GetCategoryID(category);
                foreach (DataRow row in _DataTable.Rows)
                {
                    CategoryID = Convert.ToInt32(row["ID"]);
                }
            }


            catch (Exception exception)
            {
                Log.Error(exception);
            }
            return CategoryID;

        }

        public List<NewsModel> GetAllNews()
        {
            List<NewsModel> NewsArticles = new List<NewsModel>();
            
            try
            {
                _DataTable = new MySqlNewsData().GetAllNews();
                foreach (DataRow row in _DataTable.Rows)
                {
                    _NewsModel              = new NewsModel();
                    _NewsModel.ID           = Convert.ToInt32(row["ID"]);
                    _NewsModel.Category     = Convert.ToString(row["Category"]);
                    _NewsModel.Headline     = Convert.ToString(row["Headline"]);
                    _NewsModel.Source       = Convert.ToString(row["Source"]);
                    _NewsModel.Text         = Convert.ToString(row["Text"]);
                    _NewsModel.Publish_Date = Convert.ToDateTime(row["PublishDate"]);
                    _NewsModel.CategoryID   = Convert.ToString(row["CategoryID"]);
                    _NewsModel.TimeAgo      = new TimeAgo().GetTimeAgo(_NewsModel.Publish_Date);
                    _NewsModel.Summary      = Convert.ToString(row["Summary"]);
                    _NewsModel.ImageURL     = Convert.ToString(row["ImageURL"]);
                    _NewsModel.VoteCount    = Convert.ToInt32(row["VoteCount"]);
                    _NewsModel.AddedBy = Convert.ToString(row["AddedBy"]);

                    var LikeFlag= new Votes().ReturnVoteData(_NewsModel.ID, new UserAccounts().RetrieveUserID(HttpContext.Current.User.Identity.Name));
                    if (LikeFlag.ID == 0)
                        _NewsModel.HasCurrentUserLiked  = null;
                    else
                        _NewsModel.HasCurrentUserLiked  = LikeFlag.IsUpvote;
                    _NewsModel.UsersVoted               = new Votes().ReturnVoteCount(_NewsModel.ID);

                    NewsArticles.Add(_NewsModel); 
                }
            }


            catch (Exception exception)
            {
                Log.Error(exception);
            }
            return NewsArticles;
        }

        

        public PageModel GetCategoryNews(int category, int page)
        {
            List<NewsModel> NewsArticles = new List<NewsModel>();
            PageModel PageModel = new PageModel();
            int MaxItems = 6;
            try
            {
                _DataTable = new MySqlNewsData().GetNews(category);
                foreach (DataRow row in _DataTable.Rows)
                {
                    _NewsModel              = new NewsModel();
                    _NewsModel.ID           = Convert.ToInt32(row["ID"]);
                    _NewsModel.Category     = Convert.ToString(row["Category"]);
                    _NewsModel.Headline     = Convert.ToString(row["Headline"]);
                    _NewsModel.Source       = Convert.ToString(row["Source"]);
                    _NewsModel.Text         = Convert.ToString(row["Text"]);
                    _NewsModel.Publish_Date = Convert.ToDateTime(row["PublishDate"]);
                    _NewsModel.CategoryID   = Convert.ToString(row["CategoryID"]);
                    _NewsModel.TimeAgo      = new TimeAgo().GetTimeAgo(_NewsModel.Publish_Date);
                    _NewsModel.Summary      = Convert.ToString(row["Summary"]);
                    _NewsModel.ImageURL     = Convert.ToString(row["ImageURL"]);
                    _NewsModel.VoteCount    = Convert.ToInt32(row["VoteCount"]);
                    _NewsModel.AddedBy = Convert.ToString(row["AddedBy"]);

                    var LikeFlag = new Votes().ReturnVoteData(_NewsModel.ID, new UserAccounts().RetrieveUserID(HttpContext.Current.User.Identity.Name));
                    if (LikeFlag.ID == 0)
                        _NewsModel.HasCurrentUserLiked = null;
                    else
                    {
                        if(LikeFlag.IsUpvote)
                            _NewsModel.HasCurrentUserLiked = true;
                        else
                            _NewsModel.HasCurrentUserLiked = false;
                    }
                    _NewsModel.UsersVoted = new Votes().ReturnVoteCount(_NewsModel.ID);
                    NewsArticles.Add(_NewsModel);
                }

                PageModel.News          = NewsArticles.Skip((page - 1) * MaxItems).Take(MaxItems).ToList();
                double PageCount        = (double)(NewsArticles.Count() / Convert.ToDecimal(MaxItems));
                PageModel.PageCount     = (int)Math.Ceiling(PageCount);
                PageModel.CurrentPage   = page;
            }


            catch (Exception exception)
            {
                Log.Error(exception);
            }
            return PageModel;
        }


        public void AddNews(NewsModel news)
        {
            new MySqlNewsData().AddNewsArticle(news, Convert.ToInt32(news.CategoryID));
        }

        #endregion
    }
}