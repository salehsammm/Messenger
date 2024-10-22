using Messenger.Models;
using Messenger.Models.ViewModel;
using Messenger.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Messenger.Controllers
{
    public class GroupController : Controller
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

        public JsonResult GetUsersByGpId(int groupId)
        {
            var user = db.Users.SingleOrDefault(u => u.PhoneNumber == User.Identity.Name);
            if (user == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            var usersId = db.GroupUsers.Where(gu => gu.GroupId == groupId && gu.UserId != user.UserId).Select(gu => gu.UserId).ToList();
            var usersInGroup = db.Users.Where(u => usersId.Contains(u.UserId))
                                        .Select(u => new
                                        {
                                            UserId = u.UserId,
                                            UserPhone = u.PhoneNumber,
                                            FriendName = db.UserFriends
                                                          .Where(uf => uf.UserId == user.UserId && uf.FriendId == u.UserId)
                                                          .Select(uf => uf.FullName)
                                                          .FirstOrDefault()
                                        }).ToList();

            var nameOfUsers = usersInGroup.Select(u => new NameAndId
            {
                UserId = u.UserId,
                FullName = u.FriendName ?? u.UserPhone
            }).ToList();

            var messageList = db.Messages.Where(m => m.GroupId == groupId).OrderBy(m => m.MessageDate).ToList();

            foreach (var item in messageList)
            {
                if (item.SenderId != user.UserId)
                {
                    item.SeenStatus = true;
                }
            }
            db.SaveChanges();

            var data = new GpDataViewModel()
            {
                userId = user.UserId,
                groupName = db.Groups.Where(g => g.GroupId == groupId).Select(g => g.GroupName).SingleOrDefault(),
                IsAdmin = db.GroupUsers.Where(g => g.GroupId == groupId && g.UserId == user.UserId).Select(g => g.IsAdmin).FirstOrDefault(),
                UserName = user.Fname + " " + user.Lname,

                nameOfUsers = nameOfUsers,
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
                })
                    .ToList()
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SendMessage(int groupId, string message)
        {
            var user = db.Users.SingleOrDefault(u => u.PhoneNumber == User.Identity.Name);

            var message2 = new Message()
            {
                MessageText = message,
                MessageDate = DateTime.Now,
                SenderId = user.UserId,
                GroupId = groupId,
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
                GroupId = groupId,
                SeenStatus = false,
                SentStatus = true,
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MakeGroup()
        {
            if (Session["UserPhoneNumber"] == null)
            {
                return RedirectToAction("index", "Login");
            }

            return View();
        }

        public JsonResult GpMakePost(GroupViewModel data)
        {
            byte S;
            var user = db.Users.SingleOrDefault(u => u.PhoneNumber == User.Identity.Name);

            if (db.Groups.Any(u => u.GroupName == data.GpName))
            {
                S = 1;
            }
            else
            {
                var Gp = new Group()
                {
                    GroupDate = DateTime.Now,
                    GroupName = data.GpName,
                };
                db.Groups.Add(Gp);
                db.SaveChanges();

                var admin = new GroupUser()
                {
                    IsAdmin = true,
                    UserId = user.UserId,
                    GroupId = Gp.GroupId,
                };
                db.GroupUsers.Add(admin);

                foreach (var friendId in data.SelectedFriends)
                {
                    var GpUser = new GroupUser()
                    {
                        IsAdmin = false,
                        UserId = friendId,
                        GroupId = Gp.GroupId,
                    };
                    db.GroupUsers.Add(GpUser);
                }

                db.SaveChanges();
                S = 2;
            }

            return Json(S, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetGpData()
        {
            string a = Session["UserPhoneNumber"].ToString();
            var user = db.Users.SingleOrDefault(u => u.PhoneNumber == a);
            var groupsOfUserId = db.GroupUsers.Where(g => g.UserId == user.UserId).Select(g => g.GroupId).ToList();
            var groupsOfUser = db.Groups.Where(g => groupsOfUserId.Contains(g.GroupId))
                             .Select(g => new
                             {
                                 g.GroupId,
                                 g.GroupName
                             })
                             .ToList();
            var friends = db.UserFriends.Where(u => u.UserId == user.UserId)
                                        .Select(f => new
                                        {
                                            f.FullName,
                                            f.FriendId
                                        }).ToList();

            return Json(new{groups = groupsOfUser ,friends = friends}, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateGroupName(int groupId, string groupName)
        {
            byte S = 0;
            var gp = db.Groups.SingleOrDefault(g=>g.GroupId == groupId);
            if (gp == null)
            {
                S = 1;
            }
            else
            {
                gp.GroupName = groupName;
                db.Entry(gp).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                S = 2;
            }
            return Json(S, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddUser(int groupId, List<int> SelectedFriends)
        {
            byte S;
            var user = db.Users.SingleOrDefault(u => u.PhoneNumber == User.Identity.Name);
            var Gp = db.Groups.SingleOrDefault(g => g.GroupId == groupId);

            foreach (var friendId in SelectedFriends)
            {
                var GpUser = new GroupUser()
                {
                    IsAdmin = false,
                    UserId = friendId,
                    GroupId = Gp.GroupId,
                };
                db.GroupUsers.Add(GpUser);
            }
            db.SaveChanges();
            S = 2;

            return Json(S, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteMember(int groupId, int memberId)
        {
            var user = db.Users.SingleOrDefault(u => u.PhoneNumber == User.Identity.Name);

            var groupmember = db.GroupUsers.SingleOrDefault(g => g.GroupId == groupId && g.UserId==memberId);
            db.GroupUsers.Remove(groupmember);
            db.SaveChanges();

            return Json(1, JsonRequestBehavior.AllowGet);
        }

    }
}