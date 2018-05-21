using LSQEx.Models;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace LSQEx.DL
{
    public class MySqlVoteData
    {
        public static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        MySqlConnection _Connection;
        MySqlCommand _Command;
        string _ConnectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
        MySqlDataReader _DataReader;
        DataTable _DataTable;
        DataSet _DataSet;

        public MySqlVoteData()
        {
            _Connection = new MySqlConnection(_ConnectionString);
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

        public DataTable GetUpvotes(int userID)
        {
            try
            {
                DataInitialize();
                _Command.CommandText = "Select COUNT(ID) from Votes where UserID= @ID AND isUpvote=true";
                _Command.Parameters.AddWithValue("@ID", userID);
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

        public DataTable GetDownvotes(int userID)
        {
            try
            {
                DataInitialize();
                _Command.CommandText = "Select COUNT(ID) from Votes where UserID= @ID AND isUpvote=false";
                _Command.Parameters.AddWithValue("@ID", userID);
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
        public DataTable GetVoteCount(int id)
        {
            try
            {
                DataInitialize();
                _Command.CommandText = "Select COUNT(ID) from Votes where NewsID= @ID";
                _Command.Parameters.AddWithValue("@ID", id);
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

        public void DeleteVoteFromDB(int id)
        {
            try
            {
                DataInitialize();
                _Command.CommandText = "Delete from Votes WHERE ID=@ID";
                _Command.Parameters.AddWithValue("@ID", id);
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

        public void UpdateVoteCount(int id, int count)
        {
            try
            {
                DataInitialize();
                _Command.CommandText = "Update News SET VoteCount= @VoteCount WHERE ID=@ID";
                _Command.Parameters.AddWithValue("@ID", id);
                _Command.Parameters.AddWithValue("@VoteCount", count);
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

        public void AddArticleVote(VoteModel like)
        {
            try
            {
                DataInitialize();
                _Command.CommandText = "Insert into Votes (NewsID, UserID, isUpvote) VALUES (@NewsID, @UserID, @isUpvote)";
                _Command.Parameters.AddWithValue("@UserID", like.UserID);
                _Command.Parameters.AddWithValue("@NewsID", like.NewsID);
                _Command.Parameters.AddWithValue("@isUpvote", like.IsUpvote);
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

        public DataTable GetVoteData(int newsID, int userID)
        {
            try
            {
                DataInitialize();
                _Command.CommandText = "Select * from Votes where UserID= @UserID AND NewsID= @NewsID";
                _Command.Parameters.AddWithValue("@UserID", userID);
                _Command.Parameters.AddWithValue("@NewsID", newsID);
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
    }
}