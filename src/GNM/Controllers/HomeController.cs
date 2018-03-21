using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Npgsql;
using GNM.Models;
using GNM.Service;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net;
//using System.Web.Mail;

namespace GNM.Controllers
{
    public class HomeController : Controller
    {
        #region ServiceDelaration
        IUserService _userService = new UserService();
        #endregion
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["UserId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }

        }

        #region Login
        public ActionResult Login()
        {
            if (Session["UserId"] != null)
            {
                return View("Index");
            }
            else
            {
                User model = new User();
                return View(model);
            }
            //return View();
        }

        [HttpPost]
        public ActionResult Login(User model)
        {
            try
            {
                var user = _userService.LoginAuthentication(model.UserName, model.Password);
                if (user!=null)
                {
                    Session["UserId"] = user.UserId;
                    Session["UserName"] = user.UserName;
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.Clear();
                    ModelState.AddModelError("", "Invalid UserName and Paassword");
                    return View("Login",model);

                }
            }
            catch (Exception rx)
            {
                throw rx;
            }
        }
        #endregion

        #region ChangePassword
        public ActionResult ChangePassword()
        {
            if (Session["UserId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        [HttpPost]
        public ActionResult ChangePassword(string oldPassword,string newPassword)
        {
            try
            {
                int userId = Convert.ToInt32(Session["UserId"].ToString());
                var pass = _userService.CheckOldPassword(userId);
                if (pass!=null)
                {
                    if (pass != oldPassword)
                    {
                        return Json("Your Old Password is wrong", JsonRequestBehavior.AllowGet);
                    }
                }
                int i = _userService.ChangePassword(userId, newPassword);
                if (i>0)
                {
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Error in Changing Password", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception rx)
            {
                throw rx;
            }
        }
        #endregion

        #region Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
        #endregion

        #region ForgotPassword
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ForgotPassword(string emailAddress)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (!string.IsNullOrWhiteSpace(emailAddress)
                    && regex.IsMatch(emailAddress))
            {
                var user = _userService.GetUserInformationByEmailAddress(emailAddress);
                if(user!=null)
                {
                    SendMail(user.EmailAddress,user.Password);
                    return Json("Ypur Password Send to your email Address", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Email Address is not Exist",JsonRequestBehavior.AllowGet);
                }
            }
            else if (string.IsNullOrWhiteSpace(emailAddress))
            {
                return Json("Email Address is required", JsonRequestBehavior.AllowGet);
            }
            else if(!regex.IsMatch(emailAddress))
            {
                return Json("Invalid Email Address", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Invalid Email Address", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region SendMail
        public void SendMail(string emailAddress,string password)
        {
            MailMessage msg = new MailMessage();

            msg.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["EmailAddress"].ToString());
            msg.To.Add(new MailAddress(System.Configuration.ConfigurationManager.AppSettings["EmailAddress"].ToString()));
            msg.Subject = "Password Information";
            msg.Body = "<html><body><table 'border=1'><tr><td>Your Passowrd</td></tr><tr><td>" + password + "</td></tr></table></body></html>";
            msg.IsBodyHtml = true;
            msg.Priority = MailPriority.High;

           // SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
            SmtpClient smtpServer = new SmtpClient("smtp.mail.yahoo.com");
            smtpServer.Credentials = new System.Net.NetworkCredential("niravdshah@yahoo.in","9428489047");
            smtpServer.Port = 587; // Gmail works on this port
            smtpServer.EnableSsl = true;
            smtpServer.UseDefaultCredentials = false;
            smtpServer.Send(msg);
        }
        #endregion
    }
}