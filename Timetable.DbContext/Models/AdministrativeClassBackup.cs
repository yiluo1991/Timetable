using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Timetable.DbContext.Models
{
   public  class AdministrativeClassBackup
    {
        public long Id { get; set; }

        public JsonElement BackupList { get; set; }

        public long AdminstractiveClassId { get; set; }

        public virtual AdministrativeClass AdministrativeClass { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}
