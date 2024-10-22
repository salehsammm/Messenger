using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Messenger.Models
{
    public class MessageJson
    {
        public int MessageId { get; set; }
        public string MessageText { get; set; }
        public string MessageDate { get; set; }
        public string MessageTime { get; set; }
        public bool SentStatus { get; set; }
        public bool SeenStatus { get; set; }
        public int SenderId { get; set; }
        public Nullable<int> ReceiverId { get; set; }
        public Nullable<int> GroupId { get; set; }
    }
}