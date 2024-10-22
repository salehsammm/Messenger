using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Messenger.Models.ViewModel
{
    public class GpDataViewModel
    {
        public int userId { get; set; }
        public string UserName { get; set; }
        public List<MessageJson> messages { get; set; }
        public List<NameAndId> nameOfUsers { get; set; }
        public string groupName { get; set; }
        public bool IsAdmin { get; set; }
    }
}