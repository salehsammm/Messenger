using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Messenger.Models
{
	public class AddFreindViewModel
	{
		public string PhoneNumber { get; set; }
		public string FullName { get; set; }
        public int FriendId { get; set; }
    }
}