using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Timetable.Web.OA.Infrastructure.PermissionManager.Models
{
    public class PermissionLineCache
    {
  
            public long Id { get; set; }

            /// <summary>
            ///     所属权限组/菜单功能，关联到PermissionGroup表Id字段
            /// </summary>
            public long GroupId { get; set; }
            /// <summary>
            ///     权限项/按钮功能的服务方法/路径。
            /// </summary> 
            public string Url { get; set; }

            /// <summary>
            ///     权限项/按钮功能在HTML中的标识。
            /// </summary>
            public string Tag { get; set; }

            /// <summary>
            ///     拥有该权限项的角色Id
            /// </summary>
            public List<long> Roles { get; set; }
        
    }
}