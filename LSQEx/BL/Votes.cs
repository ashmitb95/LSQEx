using LSQEx.DL;
using LSQEx.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace LSQEx.BL
{
    public class Votes
    {
        public static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataTable _DataTable;

        public string VoteArticle(int newsID, int categoryID, bool toggle)
        {
            NewsModel NewsArticle= null;
            try
            {
                NewsArticle         = new NewsArticles().GetCurrentNewsItem(newsID, categoryID);
                VoteModel Vote      = ReturnVoteData(newsID, new UserAccounts().RetrieveUserID(HttpContext.Current.User.Identity.Name));
                int AddCount        = 0;
                if (Vote.ID == 0) //if a vote does not exist, add new vote
                {
                    Vote            = new VoteModel();
                    Vote.UserID     = new UserAccounts().RetrieveUserID(HttpContext.Current.User.Identity.Name);
                    Vote.IsUpvote   = toggle;
                    Vote.NewsID     = newsID;
                    AddVote(Vote);
                    if (toggle)
                        AddCount    = 1;
                    else
                        AddCount    = -1;
                    NewsArticle.VoteCount = NewsArticle.VoteCount + AddCount;
                }
                else // else check if Previous vote is upvote or downvote
                {
                    if (toggle)
                    {
                        if (Vote.IsUpvote)//if Upvote
                            AddCount = -1;//remove Upvote from total count
                        else
                        {
                            AddCount            = 2;
                            VoteModel NewVote   = new VoteModel();
                            NewVote.UserID      = new UserAccounts().RetrieveUserID(HttpContext.Current.User.Identity.Name);
                            NewVote.IsUpvote    = toggle;
                            NewVote.NewsID      = newsID;
                            AddVote(NewVote);
                        }
                    }
                    else
                    {
                        if (Vote.IsUpvote)
                        {
                            AddCount            = -2;
                            VoteModel NewVote   = new VoteModel();
                            NewVote.UserID      = new UserAccounts().RetrieveUserID(HttpContext.Current.User.Identity.Name);
                            NewVote.IsUpvote    = toggle;
                            NewVote.NewsID      = newsID;
                            AddVote(NewVote);

                        }
                        else
                        {
                            AddCount = 1;

                        }

                    }
                    DeleteVote(Vote.ID);
                    NewsArticle.VoteCount = NewsArticle.VoteCount+AddCount;
                    
                    
                }
                UpdateVoteCount(NewsArticle.ID, NewsArticle.VoteCount);
            }

            catch (Exception e)
            {

                Log.Error(e);
            }
            return NewsArticle.VoteCount.ToString();
        }

        public void DeleteVote(int id)
        {
            new MySqlVoteData().DeleteVoteFromDB(id);
        }

        public void UpdateVoteCount(int id, int count)
        {
            new MySqlVoteData().UpdateVoteCount(id, count);
        }

        public int ReturnVoteCount(int id)
        {
            int VoteCount = 0;
            try
            {
                _DataTable      = new MySqlVoteData().GetVoteCount(id);
                foreach (DataRow row in _DataTable.Rows)
                {
                    VoteCount = Convert.ToInt32(row["COUNT(ID)"]);
                }
            }


            catch (Exception exception)
            {
                Log.Error("Thrown from ReturnVoteoCount()"+exception);
            }

            return VoteCount;
        }

        public void AddVote(VoteModel like)
        {
            new MySqlVoteData().AddArticleVote(like);
        }


        public VoteModel ReturnVoteData(int newsID, int userID)
        {
            VoteModel Vote              = new VoteModel();
            _DataTable                  = new MySqlVoteData().GetVoteData(newsID, userID);
            foreach (DataRow row in _DataTable.Rows)
            {
                Vote.ID             = Convert.ToInt32(row["ID"]);
                Vote.NewsID         = Convert.ToInt32(row["NewsID"]);
                Vote.UserID         = Convert.ToInt32(row["UserID"]);
                Vote.IsUpvote       = Convert.ToBoolean(row["isUpvote"]);
            }
            return Vote;
        }

        public int ReturnUpvotes(int userID)
        {
            int VoteCount = 0;
            try
            {
                _DataTable = new MySqlVoteData().GetUpvotes(userID);
                foreach (DataRow row in _DataTable.Rows)
                {
                    VoteCount = Convert.ToInt32(row["COUNT(ID)"]);
                }
            }


            catch (Exception exception)
            {
                Log.Error("Thrown from GetUpvotes()" + exception);
            }

            return VoteCount;
        }

        public int ReturnDownvotes(int userID)
        {
            int VoteCount = 0;
            try
            {
                _DataTable      = new MySqlVoteData().GetDownvotes(userID);
                foreach (DataRow row in _DataTable.Rows)
                {
                    VoteCount   = Convert.ToInt32(row["COUNT(ID)"]);
                }
            }


            catch (Exception exception)
            {
                Log.Error("Thrown from GetUpvotes()" + exception);
            }

            return VoteCount;
        }
    }
}