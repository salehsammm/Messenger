using Messenger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Messenger.Controllers
{
    public class UserController : Controller
    {
        Messenger_DbEntities db = new Messenger_DbEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register2()
        {
            return View();
        }

        public JsonResult Register2Post(Register2ViewModel u1)
        {
            var response = new RegisterResponse
            {
                Success = false,
                Message = string.Empty
            };

            if (db.Users.Any(u => u.UserName == u1.Username))
            {
                response.Message = "نام کاربری مورد نظر شما تکراری است.";
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

                response.Success = true;
                response.Message = "اطلاعات مورد نظر شما با موفقیت ثبت شد.";
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Login2()
        {
            return View();
        }

        public JsonResult Login2Post(Login2ViewModel l1)
        {
            var response = new RegisterResponse
            {
                Success = false,
                Message = string.Empty
            };
            byte Ststus = 0;

            var user = db.Users.SingleOrDefault(u => u.PhoneNumber == l1.PhoneNumber && u.Password == l1.Password);
            if (user == null)
            {
                Ststus = 0;
               // response.Message = "اطلاعات وارد شده صحیح نیست";
            }
            else
            {
                FormsAuthentication.SetAuthCookie(l1.PhoneNumber, l1.RememberMe);//1
                Session["UserPhoneNumber"] = l1.PhoneNumber;//2
                Session["Name"] = user.Fname + " " + user.Lname;
                Ststus = 1;
                //response.Success = true;
                //response.Message = "شما با موفقیت وارد شدید.";
            }
            return Json(Ststus, JsonRequestBehavior.AllowGet);
        }
    }
}