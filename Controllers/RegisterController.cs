using Messenger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Messenger.Controllers
{
    public class RegisterController : Controller
    {
        Messenger_DbEntities db = new Messenger_DbEntities();
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult RegisterPost(Register2ViewModel u1)
        {
            byte S;

            if (db.Users.Any(u => u.PhoneNumber == u1.PhoneNumber))
            {
                S = 1;
            }
            else
            {
                var user = new User
                {
                    Fname = u1.Fname,
                    Lname = u1.Lname,
                    PhoneNumber = u1.PhoneNumber,
                    Gender = u1.Gender,
                    UserName = u1.Username,
                    Password = u1.Password,
                    RegisterDate = DateTime.Now
                };

                db.Users.Add(user);
                db.SaveChanges();

                FormsAuthentication.SetAuthCookie(u1.PhoneNumber, false);//1
                Session["UserPhoneNumber"] = u1.PhoneNumber;//2
                Session["Name"] = user.Fname + " " + user.Lname;

                S = 2;
            }

            return Json(S,JsonRequestBehavior.AllowGet);
        }
    }
}