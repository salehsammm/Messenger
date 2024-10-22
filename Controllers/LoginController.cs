using Messenger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Messenger.Controllers
{
    public class LoginController : Controller
    {
        Messenger_DbEntities db = new Messenger_DbEntities();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult LoginPost(Login2ViewModel l1)
        {
            byte S;
            var user = db.Users.SingleOrDefault(u => u.PhoneNumber == l1.PhoneNumber && u.Password == l1.Password);
            if (user == null)
            {
                S = 1;
            }
            else
            {
                FormsAuthentication.SetAuthCookie(l1.PhoneNumber, l1.RememberMe);//1
                Session["UserPhoneNumber"] = l1.PhoneNumber;//2
                Session["Name"] = user.Fname + " " + user.Lname;
               S = 2;
            }
            return Json(S, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();//1
            Session.Clear();//2
            Session.Abandon();//2
            return RedirectToAction("Index", "Home");
        }
    }
}