using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Messenger.Models.ViewModel
{
    public class GroupViewModel
    {
        public string GpName { get; set; }
        public List<int> SelectedFriends { get; set; }
    }
}