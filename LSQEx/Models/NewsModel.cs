using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LSQEx.Models
{
    public class NewsModel
    {
        public int ID { get; set; }
        [Required(ErrorMessage ="Category must be selected")]
        public string Category { get; set; }
        [Required(ErrorMessage = "News Headline cannot be empty")]
        public String Headline { get; set; }
        [Required(ErrorMessage = "News Source cannot be empty")]
        public string Source { get; set; }
        [Required(ErrorMessage = "Publish Date cannot be empty")]
        [DataType(DataType.DateTime, ErrorMessage ="Date in not in a valid format")]
        [DisplayName("Publish Date")]
        public DateTime Publish_Date { get; set; }
        [Required(ErrorMessage = "News Body cannot be empty")]
        [DisplayName("News Body")]
        public string Text { get; set; }
        [Required(ErrorMessage = "Summary cannot be empty")]
        public string Summary { get; set; }

        public string TimeAgo { get; set; }

        public string ImageURL { get; set; }

        public string CategoryID { get; set; }

        public bool? HasCurrentUserLiked { get; set; }

        public int VoteCount { get; set; }

        public int UsersVoted { get; set; }

        public string AddedBy { get; set; }
    }

    public class FeedModel
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public string PubDate { get; set; }
        public string Image { get; set; }
    }

    public class HomeNewsModel
    {
        public List<NewsModel> AllNews { get; set; }
        public List<CategoriesModel> NewsCategory { get; set; }
        public List<FilterModel> CategoryList { get; set; }
        public NewsModel User { get; set; }
        public List<SelectListItem> SelectList { get; set; }
        public string[] SelectedCategory { get; set; }
    }

    public class CategoryModel
    {
        [Required(ErrorMessage ="This Field cannot be empty")]
        public string Category { get; set; }
    }

    public class CategoriesModel
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public bool Filter { get; set; }
    }
    public class FilterModel:CategoriesModel
    {
        public bool Filter { get; set; }
    }
    public class AddNewsModel
    {
        public IEnumerable<SelectListItem> Categories { get; set; }
        //public List<CategoriesModel> Categories{get; set;}
        [Required(ErrorMessage = "Category must be selected")]
        public string Category { get; set; }
        public NewsModel NewsModel { get; set; }
        public bool CanAddNews { get; set; }
    }
        

    public class VoteModel
    {
        public  int  ID { get; set; }
        public int UserID { get; set; }
        public int NewsID { get; set; }
        public bool IsUpvote { get; set; }
    }

    public class PageModel
    {
        public List<NewsModel> News { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
    }
}