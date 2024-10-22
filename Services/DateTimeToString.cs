using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Messenger.Services
{
    public class DateTimeToString
    {
        public static string DateToString(DateTime date)
        {
            return date.ToString("MM/dd/yyyy");
        }

        public static string TimeToString(DateTime date)
        {
            return date.ToString("HH:mm");
        }

    }
}