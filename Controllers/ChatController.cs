using Messenger.Models;
using Messenger.Models.ViewModel;
using Messenger.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Messenger.Controllers
{
    public class ChatController : Controller
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

        public JsonResult GetMessagesById(int selectedId)
        {
            var user = db.Users.SingleOrDefault(u => u.PhoneNumber == User.Identity.Name);
            var friend = db.Users.SingleOrDefault(u => u.UserId == selectedId);
            if (user == null || friend == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            var messageList = db.Messages
                .Where(m => (m.SenderId == user.UserId && m.ReceiverId == friend.UserId) ||
                            (m.SenderId == friend.UserId && m.ReceiverId == user.UserId))
                .OrderBy(m => m.MessageDate)
                .ToList();

            foreach (var item in messageList)
            {
                if (item.ReceiverId == user.UserId)
                {
                    item.SeenStatus = true;
                }
            }
            db.SaveChanges();

            var data = new MessagesViewModel()
            {
                friendName = db.UserFriends.Where(uf => uf.FriendId == friend.UserId && uf.UserId == user.UserId)
                .Select(uf => uf.FullName).SingleOrDefault(),

                friendMobile = friend.PhoneNumber,

                messages = messageList.Select(m => new MessageJson
                {
                    MessageId = m.MessageId,
                    MessageText = m.MessageText,
                    MessageTime = DateTimeToString.TimeToString(m.MessageDate),
                    MessageDate = DateTimeToString.DateToString(m.MessageDate),
                    SentStatus = m.SentStatus,
                    SeenStatus = m.SeenStatus,
                    SenderId = m.SenderId,
                    ReceiverId = m.ReceiverId,
                    GroupId = m.GroupId
                }).ToList()
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SendMessage(int selectedId, string message)
        {
            var user = db.Users.SingleOrDefault(u => u.PhoneNumber == User.Identity.Name);

            var message2 = new Message()
            {
                MessageText = message,
                MessageDate = DateTime.Now,
                SenderId = user.UserId,
                ReceiverId = selectedId,
                SeenStatus = false,
                SentStatus = true,
            };
            db.Messages.Add(message2);
            db.SaveChanges();

            var data = new MessageJson()
            {
                MessageText = message,
                MessageTime = DateTimeToString.TimeToString(DateTime.Now),
                MessageDate = DateTimeToString.DateToString(DateTime.Now),
                SenderId = user.UserId,
                ReceiverId = selectedId,
                SeenStatus = false,
                SentStatus = true,
            };

            if (!(db.UserFriends.Any(u => u.UserId == selectedId && u.FriendId == user.UserId)))
            {
                var unknownFriend = new UserFriend()
                {
                    FullName = user.PhoneNumber,
                    BlockStatus = false,
                    UserId = selectedId,
                    FriendId = user.UserId
                };

                db.UserFriends.Add(unknownFriend);
                db.SaveChanges();
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetFriends()
        {
            var user = db.Users.SingleOrDefault(u => u.PhoneNumber == User.Identity.Name);
            var friends = db.UserFriends.Where(u => u.UserId == user.UserId)
                                        .Select(f => new
                                        {
                                            f.FullName,
                                            f.FriendId
                                        }).ToList();
            if (!friends.Any())
            {
                return Json(new { message = "No friends found." }, JsonRequestBehavior.AllowGet);
            }

            return Json(friends, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteFriend(int selectedId)
        {
            int S;

            var user = db.Users.SingleOrDefault(u => u.PhoneNumber == User.Identity.Name);
            var friend = db.Users.SingleOrDefault(u => u.UserId == selectedId);
            if (user == null || friend == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            var userFriend = db.UserFriends.SingleOrDefault(uf => uf.UserId == user.UserId && uf.FriendId == friend.UserId);
            db.UserFriends.Remove(userFriend);
            db.SaveChanges();
            S = 2;

            //var chats = db.Messages
            //    .Where(m => (m.SenderId == user.UserId && m.ReceiverId == selectedId) ||
            //                (m.SenderId == selectedId && m.ReceiverId == user.UserId))
            //    .ToList();

            //foreach (var chat in chats)
            //{
            //    db.Messages.Remove(chat);
            //}
            //چون برای طرف مقابل هم حذف میشه
            
            return Json(2, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NoFriend()
        {
            if (Session["UserPhoneNumber"] == null)
            {
                return RedirectToAction("index", "Login");
            }

            return View();
        }

    }
}