﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Messenger.Models
{
    public class Login2ViewModel
    {
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}