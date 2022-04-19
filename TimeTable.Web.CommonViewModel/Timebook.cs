using System;
using System.Collections.Generic;
using System.Text;

namespace Timetable.Web.CommonViewModel
{
    public class Timebook
    {
        //public Dictionary<int, int[]> QuickSetting { get; set; }

        public String Desc { get; set; }

        //public int StartWeek { get; set; }

        //public int EndWeek { get; set; }

        public Dictionary<string, int[]> Book{get;set;}


    }
}
