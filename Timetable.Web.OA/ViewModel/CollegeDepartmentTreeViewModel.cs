using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timetable.Web.OA.ViewModel
{
    public class CollegeDepartmentTreeViewModel
    {

        public CollegeDepartmentTreeViewModel() {
            children = new List<CollegeDepartmentTreeViewModel>();
        }

        /// <summary>
        ///     原始ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        ///     字符串ID，C_或D_
        /// </summary>
        public string SID { get; set; }

        /// <summary>
        ///     系部名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        ///     联系人
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        ///     联系方式
        /// </summary>
        public string ContactPhone { get; set; }

        /// <summary>
        ///     是否启用
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        ///     系所属学院
        /// </summary>
        public long? CollegeId { get; set; }

        /// <summary>
        ///     备注
        /// </summary> 
        public string Remark { get; set; }


        public List<CollegeDepartmentTreeViewModel> children { get; set; }
    }
}
