using System;
using System.Collections.Generic;
using System.Text;

namespace Timetable.DbContext.Models
{
   public  class Student
    {
        /// <summary>
        ///     主键，学号号，必填，唯一
        /// </summary>
        public string IdentityCode { get; set; }

        /// <summary>
        ///     校内统一登录：第三方登录Id，唯一
        /// </summary>
        public string SchoolOpenId { get; set; }


        /// <summary>
        ///     密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     单点登录Hash
        /// </summary>
        public string SinglePointHash { get; set; }

        /// <summary>
        ///     真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        ///     头像
        /// </summary>
        public string AvatarUrl { get; set; }

        /// <summary>
        ///     手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        ///     性别
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        ///     学院ID
        /// </summary>
        public long? CollegeId { get; set; }

        /// <summary>
        ///     系ID
        /// </summary>
        public long? DepartmentId { get; set; }

        /// <summary>
        ///     班级ID
        /// </summary>
        public  long AdministrativeClassId { get; set; }

        /// <summary>
        ///     入学年份
        /// </summary>
        public long AdmissionYear { get; set; }

      
        /// <summary>
        ///     学生状态
        /// </summary>
        public StudentState StudentState { get; set; }



        public virtual College College { get; set; }

        public virtual Department Department { get; set; }

        public virtual AdministrativeClass  AdministrativeClass { get; set; }
        public virtual ICollection<CoursePick> CoursePicks { get; set; }
    }

    public enum StudentState
    {
        Normal=0,
        Graduated=1,
        Suspended = 2,
        DropOut =3
    }
}
