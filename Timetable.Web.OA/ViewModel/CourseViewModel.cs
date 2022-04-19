using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timetable.Web.CommonViewModel;

namespace Timetable.Web.OA.ViewModel
{
    public class CourseViewModel
    {
        public long Id { get; set; }

        /// <summary>
        ///     课程编码，外键
        /// </summary>
        public string SubjectCode { get; set; }

     
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
        ///     上课时间Json
        /// </summary>
        public Timebook TimebookJson { get; set; }

        public string Address { get; set; }

    }
}
