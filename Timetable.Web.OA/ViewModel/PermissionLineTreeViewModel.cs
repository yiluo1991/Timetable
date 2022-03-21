using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timetable.Web.OA.ViewModel
{
    public class PermissionLineTreeViewModel
    {
        public PermissionLineTreeViewModel()
        {
            children = new List<PermissionLineTreeViewModel>();
        }

        public Int64 Id { get; set; }

        /// <summary>
        ///     所属权限组/菜单功能，关联到PermissionGroup表Id字段
        /// </summary>
        public Int64? GroupId { get; set; }

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
        ///     权限项/按钮功能在HTML中的标识。
        /// </summary>
        public string Key { get; set; }

        public List<PermissionLineTreeViewModel> children { get; set; }
    }
}
