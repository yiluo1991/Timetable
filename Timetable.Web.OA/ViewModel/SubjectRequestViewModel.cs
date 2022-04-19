using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timetable.DbContext.Models;

namespace Timetable.Web.OA.ViewModel
{
    public class SubjectRequestViewModel
    {
        /// <summary>
        ///    课程码
        /// </summary>
        public string SubjectCode { get; set; }
        /// <summary>
        ///     原始SubjectCode，修改使用
        /// </summary>
        public string OriginSubjectCode { get; set; }

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
        public SubjectType SubjectType { get; set; }

        /// <summary>
        ///     课程性质：未知、必修、选修
        /// </summary>
        public SubjectProperty SubjectProperty { get; set; }
    }
}
