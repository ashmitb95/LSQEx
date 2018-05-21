using LSQEx.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LSQEx.DL
{
    public class MySqlUserData
    {
        public static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        MySqlConnection _Connection;
        MySqlCommand _Command;
        string _ConnectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
        string _Username;
        string _Password;
        MySqlDataReader _DataReader;
        DataTable _DataTable;
        DataSet _DataSet;
        public MySqlUserData(string username = "", string password = "")
        {
            _Username = username;
            _Password = password;
            _Connection = new MySqlConnection(_ConnectionString);
            _DataReader = null;
            _DataSet = null;
            _DataTable = null;
        }

        public DataTable CheckIfUserAuthorized(string name)
        {
            try
            {
                DataInitialize();
                _Command.CommandText = "Select CanAddNews from Users where UserName=@Username";
                _Command.Parameters.AddWithValue("@Username",name);
                DataAccess();
            }
            catch (Exception e)
            {

                throw e;
            }
            return _DataTable;

        }

        public void SetAccessPrivilege(int ID, bool toggle)
        {
            DataInitialize();
            _Command.CommandText = "Update Users SET CanAddNews=@Toggle WHERE UserID=@UserID";
            _Command.Parameters.AddWithValue("@UserID",ID);
            _Command.Parameters.AddWithValue("@Toggle",toggle);
            if (_Connection.State == ConnectionState.Closed)
                _Connection.Open();
            _Command.ExecuteNonQuery();
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

        public DataTable ReturnSearchResults(string searchValue)
        {
            try
            {
                DataInitialize();
                _Command.CommandText = "Select UserId, UserName,FirstName,LastName,Role, EmailID, CanAddNews from Users where UserName LIKE @Search";
                _Command.Parameters.AddWithValue("@Search", "%" + searchValue + "%");
                DataAccess();
            }
            catch (Exception e)
            {

                throw e;
            }
            return _DataTable;
        }

        public DataTable ValidateUser()
        {
            try
            {
                DataInitialize();
                _Command.CommandText = "Select UserId, UserName,FirstName,LastName,Role, EmailID, CanAddNews from Users where UserName= @UserName and Password= @Password";
                _Command.Parameters.AddWithValue("@Username", _Username);
                _Command.Parameters.AddWithValue("@Password", _Password);
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
        public DataTable GetUserID(string userName)
        {
            try
            {
                DataInitialize();
                _Command.CommandText = "Select UserID from Users where UserName= @UserName";
                _Command.Parameters.AddWithValue("@UserName", userName);
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

        public DataTable GetUserAccount(string userName)
        {
            try
            {
                DataInitialize();
                _Command.CommandText = "Select * from Users where UserName= @UserName";
                _Command.Parameters.AddWithValue("@UserName", userName);
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

        public DataTable CheckIfUserExists(string userName, string email)
        {
            try
            {
                DataInitialize();
                _Command.CommandText = "Select UserID from Users where UserName= @UserName OR EmailID= @EmailID";
                _Command.Parameters.AddWithValue("@UserName", userName);
                _Command.Parameters.AddWithValue("@EmailID", email);
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


        public void AddToUsers(RegistrationModel registrationModel, string hashedPassword)
        {
            try
            {
                DataInitialize();
                _Command.CommandText = "Insert into Users (UserName, Password, FirstName, LastName, EmailID, Role) values (@UserName, @Password, @FirstName, @LastName, @EmailID, 'User')";
                _Command.Parameters.AddWithValue("@UserName", registrationModel.UserName);
                _Command.Parameters.AddWithValue("@Password", hashedPassword);
                _Command.Parameters.AddWithValue("@FirstName", registrationModel.FirstName);
                _Command.Parameters.AddWithValue("@LastName", registrationModel.LastName);
                _Command.Parameters.AddWithValue("@EmailID", registrationModel.Email);
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

        public DataTable RetrieveAllUsers()
        {
            try
            {
                DataInitialize();
                _Command.CommandText = "Select * from Users where Role='User'";
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


        public void DeleteUserFromDB(int userID)
        {
            try
            {
                DataInitialize();
                _Command.CommandText = "Delete from Users where UserID=@UserID";
                _Command.Parameters.AddWithValue("@UserID", userID);
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

        public void EditUser(string userName, UserModel user)
        {
            try
            {
                DataInitialize();
                _Command.CommandText = "Update Users SET FirstName=@FirstName, LastName=@LastName, EmailID=@Email WHERE UserName=@UserName";
                _Command.Parameters.AddWithValue("@UserName", userName);
                _Command.Parameters.AddWithValue("@FirstName", user.FirstName);
                _Command.Parameters.AddWithValue("@LastName", user.LastName);
                _Command.Parameters.AddWithValue("@Email", user.Email);
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

        public DataTable RetrieveDuplicateEmail(string email)
        {
            try
            {
                DataInitialize();
                _Command.CommandText = "Select UserName from Users where EmailID=@Email";
                _Command.Parameters.AddWithValue("@Email", email);
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
        public DataTable RetrievePassword(string userName)
        {
            try
            {
                DataInitialize();
                _Command.CommandText = "Select Password from Users where UserName=@UserName";
                _Command.Parameters.AddWithValue("@UserName", userName);
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

        public void InsertNewPassword()
        {
            try
            {
                DataInitialize();
                _Command.CommandText = "Update Users SET Password=@Password WHERE UserName=@UserName";
                _Command.Parameters.AddWithValue("@UserName", _Username);
                _Command.Parameters.AddWithValue("@Password", _Password);
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
    }
}