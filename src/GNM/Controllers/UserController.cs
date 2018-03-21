using GNM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GNM.Service;
using System.Text.RegularExpressions;
using CaptchaMvc.HtmlHelpers;

namespace GNM.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/
        #region ServiceDelaration
        IUserService _userService = new UserService();
        #endregion
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(User model)
        {
            try
            {
                if (this.IsCaptchaValid("Captcha is not valid"))
                {
                    if (ModelState.IsValid)
                    {
                        if (!_userService.IsUserNameExist(model.UserName, model.UserId))
                        {
                            if (!_userService.IsEmailAddressExist(model.EmailAddress, model.UserId))
                            {
                                var i = _userService.AddUser(model);
                                if (i > 0)
                                {
                                    var user = _userService.LoginAuthentication(model.UserName, model.Password);
                                    Session["UserId"] = user.UserId;
                                    Session["UserName"] = user.UserName;
                                    return RedirectToAction("Index", "Home");
                                }
                                else
                                {
                                    return View();
                                }
                            }
                            else
                            {
                                ModelState.Clear();
                                ModelState.AddModelError("", "EmailAddress already Exist Please Change EmailAddress.");
                                return View(model);
                            }
                        }
                        else
                        {
                            ModelState.Clear();
                            ModelState.AddModelError("", "UserName already Exist Please Change UserName.");
                            return View(model);
                        }
                    }
                    else
                    {
                        return View(model);
                    }
                }
                else
                {
                    ModelState.Clear();
                    ModelState.AddModelError("", "Invalid Captcha");
                    return View(model);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region User Profile
        public ActionResult UserProfile()
        {
            if (Session["UserId"] != null)
            {

                User model = new User();
                int userId = Convert.ToInt32(Session["UserId"].ToString());
                try
                {
                    model = _userService.GetUserById(userId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        public ActionResult EditUser(User model)
        {
            try
            {
                int userId = Convert.ToInt32(Session["UserId"].ToString());
                model.UserId = userId;
                if (!_userService.IsEmailAddressExist(model.EmailAddress, model.UserId))
                {
                    var i = _userService.UpdateUser(model);
                    if (i > 0)
                    {
                        return RedirectToAction("UserProfile");
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("", "Problem in Update User Information.");
                        return View("UserProfile", model);
                    }
                }
                else
                {
                    ModelState.Clear();
                    ModelState.AddModelError("", "EmailAddress already Exist Please Change EmailAddress.");
                    return View("UserProfile", model);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}