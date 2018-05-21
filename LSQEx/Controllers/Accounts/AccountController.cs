using LSQEx.BL;
using LSQEx.DL;
using LSQEx.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LSQEx.Controllers.Accounts
{
    [HandleError]
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        public static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        UserModel _CurrentUser;

        [HttpGet]
        public ActionResult Login()
        {
            ViewBag.Title = "Login";
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel loginModel, string returnUrl = "")
        {
            try
            {
                //throw new DivideByZeroException();
                if(ModelState.IsValid)
                {
                    
                    _CurrentUser = new UserAccounts(loginModel.UserName, new UserAccounts().CreateMD5(loginModel.Password)).CheckLoginCredentials();
                    
                    if (_CurrentUser != null)
                    {
                        FormsAuthenticationTicket Ticket    = new FormsAuthenticationTicket(1, _CurrentUser.UserName, DateTime.Now, DateTime.Now.AddMinutes(2880), loginModel.RememberUser, _CurrentUser.Role, FormsAuthentication.FormsCookiePath);
                        string EncryptedTicket              = FormsAuthentication.Encrypt(Ticket);
                        HttpCookie Cookie                   = new HttpCookie(FormsAuthentication.FormsCookieName, EncryptedTicket);

                        if (Ticket.IsPersistent)
                        {
                            Cookie.Expires = Ticket.Expiration;
                        }
                        Response.Cookies.Add(Cookie);
                        Response.Redirect(FormsAuthentication.GetRedirectUrl(loginModel.UserName, loginModel.RememberUser));
                        Session["LoggedFullName"]       = _CurrentUser.FirstName.ToString() + " " + _CurrentUser.LastName.ToString();
                        Session["LoggedUserID"]         = Convert.ToInt32(_CurrentUser.UserID);
                        Session["LoggedUserRole"]       = _CurrentUser.Role.ToString();

                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }

                    else
                    {
                        ModelState.AddModelError(String.Empty, "Please enter correct Username and Password");
                    }
                }
                 
            }
            catch (Exception e)
            {
                ModelState.AddModelError(String.Empty, "Something went wrong while logging you in. \n Please try again later.");
                log.Info("Error thrown in Login Page", e);
            }
            return View(loginModel);
        }

        public ActionResult Logout()
        {
            try
            {
                FormsAuthentication.SignOut();

                Response.Cookies["ASPXAUTH"].Expires = DateTime.Now.AddDays(-1);
                return RedirectToAction("News", "Home");
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.Title = "Register";
            return View("Register");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Register(RegistrationModel registrationModel)
        {
            if (ModelState.IsValid)
            {
                if (new UserAccounts().DoesUserExist(registrationModel.UserName, registrationModel.Email))
                    ModelState.AddModelError(String.Empty, "User with same email or Username Already Exists");
                else
                {
                    new UserAccounts().CreateUser(registrationModel);
                    TempData["Success"] = "User has been created!";
                    return RedirectToAction("Login", "Account");
                }
            }      
            return View();
        }

        [Authorize(Roles = "User")]
        public ActionResult Profile(string userName)
        {
            UserModel CurrentUser = new UserAccounts().RetrieveUserProfile(userName);
            if (CurrentUser.UserID != 0)
                return View("UserProfile", CurrentUser);
            else
                return RedirectToAction("Invalid", "Error");
        }

        [Authorize(Roles ="Administrator")]
        public ActionResult Admin(string userName)
        {
            UserModel CurrentUser = new UserAccounts().RetrieveUserProfile(userName);
            if (CurrentUser.UserID != 0)
                return View("Admin", CurrentUser);
            else
                return RedirectToAction("Invalid", "Error");
        }

        public ActionResult AfterLogin()
        {
            return View();
        }

        public ActionResult EditDetails(string userName)
        {
            if (userName != HttpContext.User.Identity.Name)
            {
                return RedirectToAction("Unauthorized","Error");
            }
            else
            {
                UserModel User = new UserAccounts().RetrieveUserProfile(userName);
                return View(User);
            }
        }

        [HttpPost]
        public ActionResult EditDetails(FormCollection formCollection)
        {
            UserAccounts UserAccounts = new UserAccounts();
            if(UserAccounts.CheckDuplicateEmail(formCollection["Email"].ToString()))
            {
                ModelState.AddModelError(String.Empty, "Email is already in use");
                return View();
            }
            else
            {
                var Name = HttpContext.User.Identity.Name;
                UserModel EditUser = new UserModel();
                EditUser.FirstName = formCollection["FirstName"].ToString();
                EditUser.LastName = formCollection["LastName"].ToString();
                EditUser.Email = formCollection["Email"].ToString();
                UserAccounts.EditUserInformation(Name, EditUser);
                TempData["Success"] = "Details have been saved!";
                return RedirectToAction("EditDetails","Account", new { userName = Name });
            }            
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
                return View();
        }


        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if(ModelState.IsValid)
            {
                if (new UserAccounts().CheckValidPassword(new UserAccounts().CreateMD5(model.OldPassword), HttpContext.User.Identity.Name))
                {
                    new UserAccounts().ChangePassword(new UserAccounts().CreateMD5(model.NewPassword), HttpContext.User.Identity.Name);
                    TempData["Success"] = "Password changed successfully!";
                }
                else
                    ModelState.AddModelError("OldPassword", "Incorrect Password entered.");
                }
            return View();
        }
    }
}