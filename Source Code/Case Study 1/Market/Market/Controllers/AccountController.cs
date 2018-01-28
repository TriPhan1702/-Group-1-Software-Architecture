using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using Market.Models;
using Market.ModelView;

namespace Market.Controllers
{
    public class AccountController : Controller
    {
        private MarketDBEntities Context;

        public AccountController()
        {
            Context = new MarketDBEntities();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CheckRegister(RegisterAccountViewModel accountViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Register", accountViewModel);
            }

            var User = Context.Users.SingleOrDefault(u => u.username.Equals(accountViewModel.Username));

            if (User == null)
            {
                Context.Users.Add(new User()
                {
                    username = accountViewModel.Username.Trim(),
                    password = accountViewModel.Password.Trim(),
                    roleId = RegisterAccountViewModel.NormalUserId,
                    isActive = true
                });

                try
                {
                    Context.SaveChanges();

                    //Display Alert on Marketplace Page
                    TempData["message"] = "<script>alert('Registration Successful')</script>";
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("customMess", "Account was not added, please try again");
                    
                    return View("Register", accountViewModel);
                }
            }

            ModelState.AddModelError("customMess", "Account already existed");
            return View("Register", accountViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckLogin(LoginAccountViewModel accountViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Login", accountViewModel);
            }

            try
            {
                var account = Context.Users.Single(u =>
                    u.username.Equals(accountViewModel.Username) && u.password.Equals(accountViewModel.Password));

                if (account.isActive)
                {
                    Session["CurrentUser"] = new AccountViewModel()
                    {
                        Username = account.username,
                        Password = account.password,
                        RoleId = account.roleId
                    };

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("customMess", "Account is not active");
                return View("Login", accountViewModel);
            }
            catch (Exception)
            {
                ModelState.AddModelError("customMess", "User or Password are incorrect");
                return View("Login", accountViewModel);
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}