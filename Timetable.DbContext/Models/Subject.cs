using System;
using System.Collections.Generic;
using System.Text;

namespace Timetable.DbContext.Models
{
    /// <summary>
    ///     课程，一个科目可以有多个老师开课
    /// </summary>
    public class Subject
    {

        /// <summary>
        ///    主键， 课程代码，必填，唯一
        /// </summary>
        public string SubjectCode { get; set; }

        /// <summary>
        ///     课程名
        /// </summary>
        public string Name { get; set; }

        public long? CollegeId { get; set; }

        public long? DepartmentId { get; set; }



        /// <summary>
        ///     启用
        /// </summary>
        public bool Enable { get; set; }


        /// <summary>
        ///     课程类型：未知，通识课，专业课，实践课
        /// </summary>
        public SubjectType SubjectType { get;set;}

        /// <summary>
        ///     课程性质：未知、必修、选修
        /// </summary>
        public SubjectProperty SubjectProperty { get; set; }


        public virtual College College { get; set; }

        public virtual Department Department { get; set; }


        public virtual ICollection<Course> Courses { get; set; }


    }

    public enum SubjectType
    {
        Unknow=0,
        TongShi=1,
        ZhuanYe=2,
        ShiJian=3
    }

    public enum SubjectProperty
    {
        Unknow=0,
        BiXiu=1,
        XuanXiu=2
    }
}
