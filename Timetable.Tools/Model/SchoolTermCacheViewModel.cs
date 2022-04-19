using System;
using System.Collections.Generic;
using System.Text;

namespace Timetable.Tools.Model
{
    public class SchoolTermCacheViewModel
    {

        public long Id { get; set; }

        /// <summary>
        ///     学期
        /// </summary>
        public int Term { get; set; }

        /// <summary>
        ///     用户设置的学期启始日期
        /// </summary>
        public DateTime CustomStart { get; set; }


        /// <summary>
        ///     用户设置的学期结束日期，课程安排不可超过该日期
        /// </summary>
        public DateTime CustomEnd { get; set; }

        /// <summary>
        ///     实际学期的第一个周一
        /// </summary>
        public DateTime FixedStart { get; set; }

    }
}
