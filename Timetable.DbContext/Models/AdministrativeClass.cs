using System;
using System.Collections.Generic;
using System.Text;

namespace Timetable.DbContext.Models
{
    /// <summary>
    ///     新增班级，可能重组班级，重组后
    /// </summary>
   public class AdministrativeClass
    {

        public long Id { get; set; }

        /// <summary>
        ///     学院ID
        /// </summary>
        public long? CollegeId { get; set; }

        /// <summary>
        ///     系ID
        /// </summary>
        public long? DepartmentId { get; set; }

        /// <summary>
        ///     届
        /// </summary>
        public int Grade { get; set; }


        /// <summary>
        ///     班级名，与Excel表中一致，例如：19软件3班
        ///    
        ///     导入时使用学院、系、班级名三个信息匹配班级
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        ///     状态：正常、毕业、解散
        /// </summary>
        public AdministrativeClassState AdministrativeClassState { get; set; }


        public virtual College College { get; set; }

        public virtual Department Department { get; set; }

        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<AdministrativeClassBackup>   AdministrativeClassBackups { get;  set; }
        public virtual ICollection<Course> Courses { get;  set; }
    }

    public enum AdministrativeClassState
    {
        Normal=0,
       Graduated=1,
       Disbanded=2
    }
}
