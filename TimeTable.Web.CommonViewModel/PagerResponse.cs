using System;
using System.Collections.Generic;
using System.Text;

namespace Timetable.Web.CommonViewModel
{
    public class PagerResponse : ResponseBase
    {


        /// <summary>
        ///     数据条数
        /// </summary>
        public long total { get; set; }

        /// <summary>
        ///     扩展数据
        /// </summary>
        public Object extendData { get; set; }


    }
}
