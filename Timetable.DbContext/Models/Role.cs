
using System;
using System.Collections.Generic;
using System.Text;

namespace Timetable.DbContext.Models
{
    public class Role
    {
        /// <summary>
        ///     角色Id
        /// </summary>
        public Int64 Id { get; set; }

        /// <summary>
        ///     角色名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     备注
        /// </summary>
        public string Remark { get; set; }

        
        /// <summary>
        ///     导航属性：角色权限项关系列表
        /// </summary>
        public virtual ICollection<RolePermission> RolePermissions { get;  set; }

        
        /// <summary>
        ///     导航属性：用户角色
        /// </summary>
        public virtual ICollection<EmployeeRole> EmployeeRoles { get; set; }
    }
}
