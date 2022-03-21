using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Timetable.Web.OA.Infrastructure.PermissionManager.Models
{
    public class PermissionGroupCache
    {
        public long Id { get; set; }
        /// <summary>
        ///     父级权限组Id，顶级为NULL。
        /// </summary>
        public long? ParentId { get; set; }
        /// <summary>
        ///     权限组/功能菜单的页面路径。
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        ///     功能菜单的HTML标识。
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        ///     权限组序号
        /// </summary>
        public int SN { get; set; }

        public string DisplayName { get; set; }

        public string Headshot { get; set; }

        public List<long> RoleIds { get; set; }
    }

}