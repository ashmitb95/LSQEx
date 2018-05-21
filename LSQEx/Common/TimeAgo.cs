using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSQEx.Common
{
    public class TimeAgo
    {
        public string GetTimeAgo(DateTime dateTime)
        {
            string TimePosted = string.Empty;
            try
            {
                var TimeSpan = DateTime.Now.Subtract(dateTime);

                if (TimeSpan <= TimeSpan.FromSeconds(60))
                {
                    TimePosted = string.Format("{0} seconds ago", TimeSpan.Seconds);
                }
                else if (TimeSpan <= TimeSpan.FromMinutes(60))
                {
                    TimePosted = TimeSpan.Minutes > 1 ?
                        String.Format("About {0} minutes ago", TimeSpan.Minutes) :
                        "About a minute ago";
                }
                else if (TimeSpan <= TimeSpan.FromHours(24))
                {
                    TimePosted = TimeSpan.Hours > 1 ?
                        String.Format("{0} hours ago", TimeSpan.Hours) :
                        "About an hour ago";
                }
                else if (TimeSpan <= TimeSpan.FromDays(30))
                {
                    TimePosted = TimeSpan.Days > 1 ?
                        String.Format("{0} days ago", TimeSpan.Days) :
                        "Yesterday";
                }
                else if (TimeSpan <= TimeSpan.FromDays(365))
                {
                    TimePosted = TimeSpan.Days > 30 ?
                        (TimeSpan.Days / 30 > 2 ?
                        String.Format("About {0} months ago", TimeSpan.Days / 30) :
                        "More than a month ago") :
                        "About a month ago";
                }
                else
                {
                    TimePosted = TimeSpan.Days > 365 ?
                        String.Format("About {0} years ago", TimeSpan.Days / 365) :
                        "About a year ago";
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return TimePosted;
        }
    }
}