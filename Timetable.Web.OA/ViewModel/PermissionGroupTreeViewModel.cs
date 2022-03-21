using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timetable.Web.OA.ViewModel
{
    public class PermissionGroupTreeViewModel
    {
        public PermissionGroupTreeViewModel()
        {
            children = new List<PermissionGroupTreeViewModel>();
        }

        public Int64 Id { get; set; }

        /// <summary>
        ///     权限组/功能菜单的友好名称。
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        ///     父级权限组Id，顶级为null。
        /// </summary>
        public Int64? ParentId { get; set; }

        /// <summary>
        ///     权限组/功能菜单的页面路径。
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        ///     
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        ///     头像。
        /// </summary>
        public string Headshot { get; set; }

        /// <summary>
        ///     权限组序号
        /// </summary>
        public int SN { get; set; }

        public List<PermissionGroupTreeViewModel> children { get; set; }
    }

}
