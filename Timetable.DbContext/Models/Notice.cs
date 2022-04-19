
using System;
using System.Collections.Generic;
using System.Text;

namespace Timetable.DbContext.Models
{
    public class Notice { 
        public long Id { get; set; }

        public int SN { get; set; }

        public string Title { get; set; }

        public bool Enable { get; set; }

        public string Content { get; set; }

        public  DateTime CreateTime { get; set; }
    }
}
