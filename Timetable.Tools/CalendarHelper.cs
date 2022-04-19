using System;
using System.Collections.Generic;
using System.Text;

namespace Timetable.Tools
{
   public class CalendarHelper
    {
        public static DateTime GetPreMonDay(DateTime date)
        {
            var d = date.Date;
         
            while (d.DayOfWeek != DayOfWeek.Monday)
            {
                d = d.AddDays(-1);
            }
            return d;
        }
    }
}
