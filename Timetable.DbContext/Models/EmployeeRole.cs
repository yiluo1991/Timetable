
using System;

namespace Timetable.DbContext.Models
{
    public class EmployeeRole
    {
        public Int64 Id { get; set; }

        public Int64 EmployeeId { get; set; }

        public Int64 RoleId { get; set; }
        
        public virtual Employee Employee { get; set; }
        
        public virtual Role Role { get; set; }
    }
}