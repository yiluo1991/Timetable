using System;
using System.Collections.Generic;
using System.Text;

namespace Timetable.DbContext.Models
{
    public class CoursePick
    {
        public long Id { get; set; }

        public long CourseId{ get; set; }

        public string StudentIdentityCode { get; set; }

        public virtual Course Course { get; set; }

        public virtual Student Student { get; set; }
    }
}
