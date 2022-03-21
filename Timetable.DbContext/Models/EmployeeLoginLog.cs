using System;
using System.Collections.Generic;
using System.Text;

namespace Timetable.DbContext.Models
{
  public   class EmployeeLoginLog
    {
        public long Id { get; set; }

        public long EmployeeId { get; set; }

        public string IP { get; set; }

        public string Remark { get; set; }

        public bool Success { get; set; }

        public DateTime LoginTime { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
