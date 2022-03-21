
using System;
using System.Collections.Generic;

namespace Timetable.DbContext.Models
{
    public class PermissionLine
    {
        public Int64 Id { get; set; }

        /// <summary>
        ///     权限标志名
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        ///     所属权限组/菜单功能，关联到PermissionGroup表Id字段
        /// </summary>
        public Int64 GroupId { get; set; }

        /// <summary>
        ///     权限项/按钮功能的友好名称。
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        ///     权限项/按钮功能的服务方法/路径。
        /// </summary> 
        public string Url { get; set; }

        /// <summary>
        ///     排序号
        /// </summary>
        public int SN { get; set; }

        /// <summary>
        ///     导航属性，所属权限组/菜单功能。
        /// </summary>
        public virtual PermissionGroup Group { get; set; }

        /// <summary>
        ///     导航属性：角色的权限项
        /// </summary>
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}