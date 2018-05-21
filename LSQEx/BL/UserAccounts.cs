using LSQEx.DL;
using LSQEx.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace LSQEx.BL
{
    public class UserAccounts
    {
        public static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string _Username;
        string _Password;
        DataTable _DataTable;
        UserModel _UserModel;
        List<UserModel> UserModelList;
        public UserAccounts(string username = "", string password = "")
        {
            _Username = username;
            _Password = password;
            _DataTable = null;
            _UserModel = null;
        }

        public bool IsUserAuthorizedToAddNews(string name)
        {
            bool flag = false;
            _DataTable = new MySqlUserData().CheckIfUserAuthorized(name);
            try
            {
                foreach (DataRow row in _DataTable.Rows)
                {
                    flag = Convert.ToBoolean(row["CanAddNews"]);
                }
            }
            catch (Exception exception)
            {
                Log.Error("Thrown from CheckLoginCredentials()" + exception);
            }

            return flag;
        }

        public void SetWriteAccess(int userID, bool toggleAccess)
        {
            new MySqlUserData().SetAccessPrivilege(userID, toggleAccess);
        }

        public List<UserModel> RetrieveSearchResults(string searchValue)
        {
            UserModelList = new List<UserModel>();

            _DataTable = new MySqlUserData().ReturnSearchResults(searchValue);
            try
            {
                foreach (DataRow row in _DataTable.Rows)
                {
                    _UserModel = new UserModel();
                    _UserModel.UserID = Convert.ToInt32(row["UserID"]);
                    _UserModel.FirstName = Convert.ToString(row["FirstName"]);
                    _UserModel.LastName = Convert.ToString(row["LastName"]);
                    _UserModel.Email = Convert.ToString(row["EmailID"]);
                    _UserModel.UserName = Convert.ToString(row["UserName"]);
                    _UserModel.CanAddNews = Convert.ToBoolean(row["CanAddNews"]);
                    UserModelList.Add(_UserModel);
                }
            }
            catch (Exception e)
            {
                throw e;
            }


            return UserModelList;

        }

        public UserModel CheckLoginCredentials()
        {
            
            _DataTable = new MySqlUserData(_Username, _Password).ValidateUser();
            try
            {
                foreach (DataRow row in _DataTable.Rows)
                {
                    _UserModel = new UserModel();
                    _UserModel.UserID = Convert.ToInt32(row["UserID"]);
                    _UserModel.UserName = Convert.ToString(row["UserName"]);
                    _UserModel.FirstName = Convert.ToString(row["FirstName"]);
                    _UserModel.LastName = Convert.ToString(row["LastName"]);
                    _UserModel.Role = Convert.ToString(row["Role"]);
                    _UserModel.Email = Convert.ToString(row["EmailID"]);
                }
            }
            catch (Exception exception)
            {
                Log.Error("Thrown from CheckLoginCredentials()"+exception);
            }

            return _UserModel;
        }
        public int RetrieveUserID(string userName)
        {
            _UserModel = new UserModel();
            int ID = 0;
            _DataTable = new MySqlUserData().GetUserID(userName);
            try
            {
                foreach (DataRow row in _DataTable.Rows)
                    ID = Convert.ToInt32(row["UserID"]);
            }
            catch (Exception exception)
            {
                throw exception;
            }

            return ID;
        }

        public UserModel RetrieveUserProfile(string userName)
        {
            _UserModel = new UserModel();
            _DataTable = new MySqlUserData().GetUserAccount(userName);
            Votes Votes = new Votes();
            try
            {
                foreach (DataRow row in _DataTable.Rows)
                {
                    _UserModel.UserID       = Convert.ToInt32(row["UserID"]);
                    _UserModel.FirstName    = Convert.ToString(row["FirstName"]);
                    _UserModel.LastName     = Convert.ToString(row["LastName"]);
                    _UserModel.Email        = Convert.ToString(row["EmailID"]);
                    _UserModel.UserName     = Convert.ToString(row["UserName"]);
                    _UserModel.CanAddNews = Convert.ToBoolean(row["CanAddNews"]);
                    _UserModel.Upvotes      = Convert.ToInt32(Votes.ReturnUpvotes(_UserModel.UserID));
                    _UserModel.Downvotes    = Convert.ToInt32(Votes.ReturnDownvotes(_UserModel.UserID));
                }
            }
            catch (Exception exception)
            {
                Log.Error("Thrown from CheckLoginCredentials()" + exception);
            }

            return _UserModel;
        }


        public bool DoesUserExist(string userName, string email)
        {
            _DataTable                          = new MySqlUserData().CheckIfUserExists(userName, email);
            RegistrationModel RegistrationModel = new RegistrationModel();
            foreach (DataRow row in _DataTable.Rows)
            {
                RegistrationModel.UserID = Convert.ToInt32(row["UserID"]);
            }

            if (RegistrationModel.UserID == 0)
                return false;
            else
                return true;

        }

        public void CreateUser(RegistrationModel registrationModel)
        {
            new MySqlUserData().AddToUsers(registrationModel, CreateMD5(registrationModel.Password));
        }


        public string CreateMD5(string password)
        {
            StringBuilder HashString= null;
            try
            {
                using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
                {
                    byte[] InputBytes   = System.Text.Encoding.ASCII.GetBytes(password);
                    byte[] HashBytes    = md5.ComputeHash(InputBytes);
                    HashString          = new StringBuilder();
                    for (int i = 0; i < HashBytes.Length; i++)
                    {
                        HashString.Append(HashBytes[i].ToString("X2"));
                    }
                    
                }
            }


            catch (Exception exception)
            {
                Log.Error("Thrown from CreateMd5()" + exception);
            }
            // Use input string to calculate MD5 hash
            return HashString.ToString();
        }

        public List<UserModel> GetAllUsers()
        {
            UserModelList = new List<UserModel>();

            _DataTable = new MySqlUserData().RetrieveAllUsers();
            try
            {
                foreach (DataRow row in _DataTable.Rows)
                {
                    _UserModel              = new UserModel();
                    _UserModel.UserID       = Convert.ToInt32(row["UserID"]);
                    _UserModel.FirstName    = Convert.ToString(row["FirstName"]);
                    _UserModel.LastName     = Convert.ToString(row["LastName"]);
                    _UserModel.Email        = Convert.ToString(row["EmailID"]);
                    _UserModel.UserName     = Convert.ToString(row["UserName"]);
                    _UserModel.CanAddNews = Convert.ToBoolean(row["CanAddNews"]);
                    UserModelList.Add(_UserModel);
                }
            }
            catch (Exception e)
            {
                throw e;
            }


            return UserModelList;
        }

        public void DeleteUser(int userID)
        {
            new MySqlUserData().DeleteUserFromDB(userID);
        }


        public void EditUserInformation(String userName, UserModel user)
        {
            new MySqlUserData().EditUser(userName, user);
        }

        public bool CheckDuplicateEmail(string email)
        {
            _DataTable = new MySqlUserData().RetrieveDuplicateEmail(email);
            if (_DataTable == null)
                return true;
            else
                return false;
        }

        public bool CheckValidPassword(string password, string userName)
        {
            string Password = "";
            _DataTable = new MySqlUserData().RetrievePassword(userName);
            try
            {
                foreach(DataRow row in _DataTable.Rows)
                {
                    Password = Convert.ToString(row["Password"]);
                }
                if (Password.Equals(password))
                    return true;
            }
            catch(Exception exception)
            {
                Log.Error("Thrown from CheckValidPassword()" + exception);
            }
            return false;
        }

        public void ChangePassword(string password, string userName)
        {
            new MySqlUserData(userName, password).InsertNewPassword();
        }
    }
}