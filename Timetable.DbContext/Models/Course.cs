using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Timetable.DbContext.Models
{
    /// <summary>
    ///     排课
    /// </summary>
   public class Course
    {
        public long Id { get; set; }

        /// <summary>
        ///     课程编码，外键
        /// </summary>
        public string SubjectCode { get; set; }

        public long? CollegeId { get; set; }

        public long? DepartmentId { get; set; }

       

        /// <summary>
        ///     排课名
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        ///     教师教工号
        /// </summary>
        public string TeacherIdentityCode { get; set; }

        /// <summary>
        ///     授课班级Id
        /// </summary>
        public long? AdministrativeClassId { get; set; }

        /// <summary>
        ///     学期Id
        /// </summary>
        public long SchoolTermId { get; set; }

        /// <summary>
        ///     上课时间Json
        /// </summary>
        public JsonElement TimebookJson { get; set; }
 

        /// <summary>
        ///     班级人员备份Id，用于学期结束时备份班级的课堂人员
        /// </summary>
        public  long? AdminstractiveClassBackupId { get; set; }

 
        public string Address { get; set; }


        public virtual AdministrativeClassBackup AdminstractiveClassBackup { get; set; }

        public  virtual Teacher Teacher { get; set; }

        public virtual Subject Subject { get; set; }

        public virtual College College { get; set; }

        public virtual Department Department { get; set; }


        public virtual AdministrativeClass AdministrativeClass { get; set; }

        public virtual SchoolTerm SchoolTerm { get; set; }
        public virtual ICollection<CoursePick>  CoursePicks { get;  set; }
    }
}
