using ASPNETWebInstall.BLL;
using ASPNETWebInstall.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ASPNETWebInstall.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        [Authorize]
        // GET: Account
        public ActionResult Index()
        {
            ViewData["User"]=User.Identity.Name;

            return View();
        }

        public ActionResult Login(string returnUrl=null)
        {
            // 已登录的提示，并跳转
            if (User.Identity.IsAuthenticated)
            {
                // 提示 如何实现？
                return View("Index");
            }

            ViewData["returnUrl"] = returnUrl;
            return View(new LoginViewModel());
        }
        [HttpPost]
        public ActionResult DoLogin(LoginViewModel u,string returnUrl = null)
        {
            UserHandler userHandler = new UserHandler();
            if (userHandler.IsValidUser(u))
            {
                FormsAuthentication.SetAuthCookie(u.Name, false);

                if (string.IsNullOrWhiteSpace(returnUrl))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return Redirect(returnUrl);
                }
            }
            else
            {
                ModelState.AddModelError("CredentialError", "无效的用户名或密码");
                return View("Login", u);
            }
        }

        public ActionResult Register(string returnUrl = null)
        {
            ViewData["returnUrl"] = returnUrl;
            return View(new RegisterViewModel());
        }
        [HttpPost]
        public ActionResult DoRegister(RegisterViewModel r, string returnUrl = null)
        {
            ViewData["returnUrl"] = returnUrl;

            // 仅发送邮件

            //if (发送失败)
            //{
            //    ModelState.AddModelError("CredentialError", "邮件发送失败！");
            //    return View("Register", r);
            //}

            //return RedirectToAction("Login");
            return Redirect(returnUrl); // 注册成功后登陆
        }
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            ModelState.AddModelError("CredentialError", "已退出登陆！");
            return RedirectToAction("Login");
        }
    }
}