using Messenger.Models;
using Messenger.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Messenger.Controllers
{
    public class Chat2Controller : Controller
    {
        Messenger_DbEntities db = new Messenger_DbEntities();
        public ActionResult Index(int selectedId = 0)
        {
            if (Session["UserPhoneNumber"] == null)
            {
                return RedirectToAction("index", "Login");
            }

            var user = db.Users.SingleOrDefault(u => u.PhoneNumber == User.Identity.Name);
            if (user == null)
            {
                return HttpNotFound();
            }

            var friends = db.UserFriends.Where(u => u.UserId == user.UserId).ToList();
            if (!friends.Any())
            {
                return View("NoFriend");
            }

            if (selectedId == 0 && friends.Any())
            {
                selectedId = friends.First().FriendId;
            }

            var chatVM = new ChatViewModel()
            {
                Friends = friends,
                FullName = user.Fname + " " + user.Lname,
                SelectedFriend = db.UserFriends
                .FirstOrDefault(u => u.UserId == user.UserId && u.FriendId == selectedId),

                Messages = db.Messages
                .Where(m => m.SenderId == user.UserId || m.ReceiverId == user.UserId)
                .OrderBy(m => m.MessageDate).ToList()
            };

            return View(chatVM);
        }

        public JsonResult GetMessagesById(int selectedId)
        {
            var user = db.Users.SingleOrDefault(u => u.PhoneNumber == User.Identity.Name);
            var friend = db.Users.SingleOrDefault(u => u.UserId==selectedId);
            if (user == null || friend == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            var data = new MessagesViewModel()
            {
                friendName = db.UserFriends.Where(uf=>uf.FriendId==friend.UserId && uf.UserId==user.UserId)
                .Select(uf=>uf.FullName).SingleOrDefault(),

                messages = db.Messages
                    .Where(m => (m.SenderId == user.UserId && m.ReceiverId == friend.UserId) ||
                          (m.SenderId == friend.UserId && m.ReceiverId == user.UserId))
                    .OrderBy(m => m.MessageDate)
                        .Select(m => new MessageJson
                        {
                            MessageId = m.MessageId,
                            MessageText = m.MessageText,
                            MessageDate = m.MessageDate,
                            SentStatus = m.SentStatus,
                            SeenStatus = m.SeenStatus,
                            SenderId = m.SenderId,
                            ReceiverId = m.ReceiverId,
                            GroupId = m.GroupId
                        })
                    .ToList()
            };

            return Json(data , JsonRequestBehavior.AllowGet);
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
                SeenStatus = true,
                SentStatus = true,
            };
            db.Messages.Add(message2);
            db.SaveChanges();

            var data = new MessageJson()
            {
                MessageText = message,
                MessageDate = DateTime.Now,
                SenderId = user.UserId,
                ReceiverId = selectedId,
                SeenStatus = true,
                SentStatus = true,
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}