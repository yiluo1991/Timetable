using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timetable.Web.MiniApp.WeChatModel
{
    public class CollegeDepartmentTreeViewModel
    {
        public CollegeDepartmentTreeViewModel()
        {
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
         

        public List<CollegeDepartmentTreeViewModel> children { get; set; }
    }
}
