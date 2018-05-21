using LSQEx.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

namespace LSQEx.DL
{
    public class MySqlNewsData
    {
        public static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        MySqlConnection _Connection;
        MySqlCommand _Command;
        string _ConnectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
        int _ID;
        int _Category;
        MySqlDataReader _DataReader;
        DataTable _DataTable;
        DataSet _DataSet;
        public MySqlNewsData(int id = 0, int category = 0)
        {
            _ID = id;
            _Category = category;
            _Connection = new MySqlConnection(_ConnectionString);
            _DataReader = null;
            _DataSet = null;
            _DataTable = null;
        }

        public void AddCategoryToDB(string name)
        {
            DataInitialize();
            _Command.CommandText = "Insert into Categories (Name) values (@Name)";
            _Command.Parameters.AddWithValue("@Name", name);
            _Connection.Open();
            _Command.ExecuteNonQuery();

        }

        public bool CheckCategoryInDB(string name)
        {
            DataInitialize();
            _Command.CommandText = "Select Name from Categories where Name= @Name";
            _Command.Parameters.AddWithValue("@Name", name);
            if (_Connection.State == ConnectionState.Closed)
                _Connection.Open();
            _DataReader = _Command.ExecuteReader();
            if(_DataReader.Read())
            {
                return true;
            }
            return false;
        }

        public void DataInitialize()
        {
            _Command = new MySqlCommand();
            _Command.Connection = _Connection;
            _Command.CommandType = CommandType.Text;
            _Command.Parameters.Clear();
        }

        public void DataAccess()
        { 
            if (_Connection.State == ConnectionState.Closed)
                _Connection.Open();
            _DataReader = _Command.ExecuteReader();
            _DataSet = new DataSet();
            _DataTable = new DataTable();
            _DataSet.Tables.Add(_DataTable);
            _DataSet.EnforceConstraints = false;
            _DataTable.Load(_DataReader);

        }

        #region SearchandFilter
        public DataTable RetrieveSearchResults(string search, string category)
        {
            try
            {
                DataInitialize();
                if(category=="Home")
                {
                    _Command.CommandText    = "Select * from News where (Headline LIKE @Search OR Text LIKE @Search) AND Category IN (Select Name from Categories Where Filter = 1) ORDER BY PublishDate DESC";
                    _Command.Parameters.AddWithValue("@Search", "%" + search + "%");
                }
                else
                {
                    _Command.CommandText    = "Select * from News where (Headline LIKE @Search OR Text LIKE @Search) AND Category IN (Select Name from Categories Where Filter = 1 AND Name= @category) ORDER BY PublishDate DESC";
                    _Command.Parameters.AddWithValue("@Search", "%" + search + "%");
                    _Command.Parameters.AddWithValue("@category", category);
                }
                DataAccess();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (_Connection.State == ConnectionState.Open)
                {
                    _DataReader.Close();

                    _Connection.Close();
                }
            }
            return _DataTable;
        }
        public void ResetCategoryFilters()
        {
            try
            {
                _Command                = new MySqlCommand();
                _Command.Connection     = _Connection;
                _Command.CommandType    = CommandType.Text;
                _Command.Parameters.Clear();
                _Command.CommandText    = "Update Categories SET Filter = true";
                _Connection.Open();
                _Command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (_Connection.State == ConnectionState.Open)
                {
                    _Connection.Close();
                }
            }
        }

        public void ApplyFilters(string[] HideFilter, string[] ShowFilter)
        {
            try
            {
                DataInitialize();
                if (HideFilter.Length > 0)
                {
                    string CommandText  = "Update Categories SET Filter = false WHERE ID IN ({0})";
                    string[] Parameters = HideFilter.Select((s, i) => "@HideParameter" + i.ToString()).ToArray();
                    string InClause     = string.Join(", ", Parameters);
                    _Command.CommandText = string.Format(CommandText, InClause);
                    for (int i = 0; i < Parameters.Length; i++)
                    {
                        _Command.Parameters.AddWithValue(Parameters[i], Convert.ToInt32(HideFilter[i]));
                    }
                    if (_Connection.State == ConnectionState.Closed)
                        _Connection.Open();
                    _Command.ExecuteNonQuery();
                }
                if (ShowFilter.Length > 0)
                {
                    string CommandText      = "Update Categories SET Filter = true WHERE ID IN ({0})";
                    string[] Parameters     = ShowFilter.Select((s, i) => "@ShowParameter" + i.ToString()).ToArray();
                    string InClause         = string.Join(", ", Parameters);
                    _Command.CommandText    = string.Format(CommandText, InClause);
                    for (int i = 0; i < Parameters.Length; i++)
                    {
                        _Command.Parameters.AddWithValue(Parameters[i], Convert.ToInt32(ShowFilter[i]));
                    }
                    if (_Connection.State == ConnectionState.Closed)
                        _Connection.Open();
                    _Command.ExecuteNonQuery();
                }

            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (_Connection.State == ConnectionState.Open)
                {
                    _Connection.Close();
                }
            }
        }
        #endregion

        #region News
        public void AddNewsArticle(NewsModel news, int ID)
        {
            try
            {
                DataInitialize();
                _Command.CommandText = "Insert into News (Category, Headline, Source, PublishDate, Text, Summary, ID, ImageURL, CategoryID, AddedBy) values (@Category,@Headline,@Source,@PublishDate,@Text,@Summary,@ID,@ImageURL,@CategoryID,@AddedBy)";
                _Command.Parameters.AddWithValue("@Category", news.Category);
                _Command.Parameters.AddWithValue("@Headline", news.Headline);
                _Command.Parameters.AddWithValue("@Source", news.Source);
                _Command.Parameters.AddWithValue("@PublishDate", news.Publish_Date);
                _Command.Parameters.AddWithValue("@Text", news.Text);
                _Command.Parameters.AddWithValue("@Summary", news.Summary);
                _Command.Parameters.AddWithValue("@ID", news.ID);
                _Command.Parameters.AddWithValue("@ImageURL", news.ImageURL);
                _Command.Parameters.AddWithValue("@CategoryID", news.CategoryID);
                _Command.Parameters.AddWithValue("@AddedBy", news.AddedBy);
                if (_Connection.State == ConnectionState.Closed)
                    _Connection.Open();
                _Command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (_Connection.State == ConnectionState.Open)
                {

                    _Connection.Close();
                }
            }
        }

        public DataTable GetCurrentNews()
        {

            try
            {
                DataInitialize();
                _Command.CommandText = "Select * from News where categoryID= @Category and ID= @ID";
                _Command.Parameters.AddWithValue("@Category", _Category);
                _Command.Parameters.AddWithValue("@ID", _ID);
                DataAccess();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (_Connection.State == ConnectionState.Open)
                {
                    _DataReader.Close();

                    _Connection.Close();
                }
            }
            return _DataTable;

        }

        public DataTable GetAllNews()
        {
            try
            {
                DataInitialize();
                _Command.CommandText    = "Select * from News ORDER BY PublishDate DESC";
                DataAccess();
            }

            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (_Connection.State == ConnectionState.Open)
                {
                    _DataReader.Close();

                    _Connection.Close();
                }
            }
            return _DataTable;


        }

        public DataTable GetNews(int id)
        {
            try
            {
                DataInitialize();
                _Command.Parameters.AddWithValue("@ID", id);
                _Command.CommandText    = "Select * from News where CategoryID= (Select ID from Categories where ID=@ID) ORDER BY PublishDate DESC";
                DataAccess();
            }

            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (_Connection.State == ConnectionState.Open)
                {
                    _DataReader.Close();

                    _Connection.Close();
                }
            }
            return _DataTable;
        }


            public DataTable GetHighlightNews()
            {
                try
                {
                DataInitialize();
                    _Command.CommandText    = "Select * from News where ID= @ID";
                    _Command.Parameters.AddWithValue("@Category", _Category);
                    _Command.Parameters.AddWithValue("@ID", _ID);
                DataAccess();
                }
                catch (Exception e)
                {
                    throw e;
                }

                finally
                {
                    if (_Connection.State == ConnectionState.Open)
                    {
                        _DataReader.Close();

                        _Connection.Close();
                    }

                }


                return _DataTable;

            }





            public DataTable GetHighlights()
            {
                List<NewsModel> HighlightsList = new List<NewsModel>();
                try
                {
                DataInitialize();
                    _Command.CommandText    = "Select * from News where VoteCount>=2 ORDER BY PublishDate DESC";
                DataAccess();
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    if (_Connection.State == ConnectionState.Open)
                    {
                        _DataReader.Close();

                        _Connection.Close();
                    }
                }

                return _DataTable;
            }

        
        #endregion

        #region Categories
        public DataTable GetAllCategories()
        {
            try
            {
                DataInitialize();
                _Command.CommandText    = "Select * from Categories ORDER BY ID";
                DataAccess();

            }



            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (_Connection.State == ConnectionState.Open)
                {
                    _DataReader.Close();

                    _Connection.Close();
                }
            }
            return _DataTable;


        }
        public DataTable GetCategories()
        {
            try
            {
                DataInitialize();
                _Command.CommandText = "Select * from Categories Where Filter=1 ORDER BY ID";
                DataAccess();
            }



            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (_Connection.State == ConnectionState.Open)
                {
                    _DataReader.Close();

                    _Connection.Close();
                }
            }
            return _DataTable;


        }



        public DataTable GetCategoryID(string category)
        {
            List<NewsModel> HighlightsList = new List<NewsModel>();
            try
            {
                DataInitialize();
                _Command.CommandText = "Select ID from Categories where Name=@Category";
                _Command.Parameters.AddWithValue("@Category", category);
                DataAccess();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (_Connection.State == ConnectionState.Open)
                {
                    _DataReader.Close();

                    _Connection.Close();
                }
            }

            return _DataTable;
        }
        #endregion








    }
}