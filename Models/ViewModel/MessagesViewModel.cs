using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Messenger.Models.ViewModel
{
    public class MessagesViewModel
    {
        public List<MessageJson> messages { get; set; }
        public string friendName { get; set; }
        public string friendMobile { get; set; }
    }
}