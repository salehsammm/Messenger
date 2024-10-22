using Messenger.Models;
using Messenger.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Messenger.Controllers
{
    public class FriendController : Controller
    {
        Messenger_DbEntities db = new Messenger_DbEntities();
        public ActionResult Index()
        {
            if (Session["UserPhoneNumber"] == null)
            {
                return RedirectToAction("index", "Login");
            }

            return View();
        }

        public JsonResult AddFriend(AddFreindViewModel f1)
        {
            byte S = 0;
            var user = db.Users.SingleOrDefault(u => u.PhoneNumber == User.Identity.Name);
            var friend = db.Users.SingleOrDefault(u => u.PhoneNumber == f1.PhoneNumber);
            if (friend == null)
            {
                S = 1;
            }
            else if (db.UserFriends.Any(uf => uf.UserId == user.UserId && uf.FriendId == friend.UserId))
            {
                S = 2;
            }
            else if (f1.PhoneNumber == user.PhoneNumber)
            {
                S = 3;
            }
            else 
            {
                var userFriend = new UserFriend
                {
                    BlockStatus = false,
                    FullName = f1.FullName,
                    FriendId = friend.UserId,
                    UserId = user.UserId
                };

                db.UserFriends.Add(userFriend);
                db.SaveChanges();
                S = 4;
            }
            return Json(S, JsonRequestBehavior.AllowGet);
        }

        public JsonResult postEditFriend(EditFriendViewModel f1)
        {
            byte S = 0;
            var user = db.Users.SingleOrDefault(u => u.PhoneNumber == User.Identity.Name);
            var friend = db.Users.SingleOrDefault(u => u.UserId == f1.selectedId);
            if (friend == null)
            {
                S = 1;
            }
            else
            {
                var userFriend = db.UserFriends.SingleOrDefault(uf => uf.UserId == user.UserId && uf.FriendId == f1.selectedId);
                userFriend.FullName = f1.friendName;

                db.Entry(userFriend).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                S = 2;
            }
            return Json(S, JsonRequestBehavior.AllowGet);
        }
    }
}