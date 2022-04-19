using System;
using System.Collections.Generic;
using System.Text;

namespace Timetable.DbContext.Models
{
    public class Teacher
    {

        /// <summary>
        ///     主键，教工号，必填，唯一
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

        public DateTime CreateTime { get; set; }

        /// <summary>
        ///     启用，不影响已有课和已选课
        /// </summary>
        public bool Enable { get; set; }
        public virtual ICollection<Course> Courses { get;  set; }
    }

    /// <summary>
    ///     性别枚举
    /// </summary>
    public enum Gender
    {
        UnKnow = 0,
        Male = 1,
        Female = 2
    }
}
