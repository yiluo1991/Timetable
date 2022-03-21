using System;
using System.Collections.Generic;
using System.Text;

namespace Timetable.DbContext.Models
{
    /// <summary>
    ///     院系
    /// </summary>
   public class Department
    {
        public long Id { get;set; }

        /// <summary>
        ///     系部名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        ///     联系人
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        ///     联系方式
        /// </summary>
        public string ContactPhone { get; set; }

        /// <summary>
        ///     是否启用
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        ///     系所属学院
        /// </summary>
        public long DepartmentId { get; set; }
         
        /// <summary>
        ///     备注
        /// </summary> 
        public string Remark { get; set; }


        /// <summary>
        ///     导航属性：学院
        /// </summary>
        public virtual College College { get; set; }

        public virtual ICollection<AdministrativeClass> AdministrativeClasses { get; set; }

        public virtual ICollection<Student> Students { get; set; }

        public virtual ICollection<Subject> Subjects { get; set; }

        public virtual ICollection<Course> Courses { get; set; }


      
    }
}
