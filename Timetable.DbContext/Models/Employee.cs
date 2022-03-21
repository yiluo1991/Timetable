
using System;
using System.Collections.Generic;
using System.Text;

namespace Timetable.DbContext.Models
{
    public  class Employee
    {
        /// <summary>
        ///     Id
        /// </summary>
        public Int64 Id { get; set; }

        /// <summary>
        ///     用户名
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        ///     密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     头像
        /// </summary>
        public string Headshot { get; set; }

        /// <summary>
        ///     邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        ///     是否启用
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        ///     手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        ///     联系地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        ///     账号所属学院Id
        /// </summary>
        public long? DepartmentId { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        ///     自动禁用时间
        /// </summary>
        public DateTime? PermissionEndTime { get; set; }

        /// <summary>
        ///     创建者Id
        /// </summary>
        public Int64? CreatorId { get; set; }

        /// <summary>
        ///     单点登录哈希
        /// </summary>
        public string SinglePointHsah { get; set; }
        
        /// <summary>
        ///     导航属性，创建人
        /// </summary>
        public virtual Employee Creator { get; set; }
        
        /// <summary>
        ///     导航属性：文章
        /// </summary>
        public virtual ICollection<Article> Articles { get; set; }
        
        /// <summary>
        ///     导航属性：员工角色
        /// </summary>
        public virtual ICollection<EmployeeRole> EmployeeRoles { get; set; }

        /// <summary>
        ///     导航属性： 登录日志
        /// </summary>
        public virtual ICollection<EmployeeLoginLog> EmployeeLoginLogs { get; set; }

        /// <summary>
        ///     导航属性：所属学院Id
        /// </summary>
        public virtual Department Department { get; set; }
        
  
    }
}
