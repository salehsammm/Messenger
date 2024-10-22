using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Messenger.Models
{
	public class ChatViewModel
	{
        public string FullName { get; set; } = " ";
        public List<UserFriend> Friends { get; set; }
        public UserFriend SelectedFriend { get; set; }
        public List<Message> Messages { get; set; }
    }
}