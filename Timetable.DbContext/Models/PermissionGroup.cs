
using System;
using System.Collections.Generic;
using System.Text;

namespace Timetable.DbContext.Models
{
    public   class PermissionGroup
    {
        public Int64 Id { get; set; }

        /// <summary>
        ///     权限组/功能菜单的友好名称。
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        ///    标志名
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        ///     父级权限组Id，顶级为null。
        /// </summary>
        public Int64? ParentId { get; set; }

        /// <summary>
        ///     权限组/功能菜单的页面路径。
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        ///     小图标
        /// </summary>
        public string Headshot { get; set; }

        /// <summary>
        ///     权限组序号
        /// </summary>
        public int SN { get; set; }
        /// <summary>
        ///     导航属性，所属菜单。
        /// </summary>
        public virtual PermissionGroup Parent { get; set; }
        /// <summary>
        ///     导航属性，该权限组所拥有的所有权限项/按钮功能。
        /// </summary>
        public virtual ICollection<PermissionLine> PermissionLines { get; set; }

        /// <summary>
        ///     导航属性，该权限组下的所有子权限组/功能菜单（不包含孙字辈）。
        /// </summary>
        public virtual ICollection<PermissionGroup> Children { get; set; }
    }
}
