using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Timetable.Web.CommonViewModel
{
    public class PagerRequest
    {
        public PagerRequest()
        {
            page = 1;
            rows = 15;
            keyword = "";
        }

        /// <summary>
        ///     页码
        /// </summary>
        public int page { get; set; }
        /// <summary>
        ///     分页条数
        /// </summary>
        [Range(1, 30)]
        public int rows { get; set; }
        /// <summary>
        ///     关键词
        /// </summary>
        public string keyword { get; set; }
        /// <summary>
        ///     分类id，非必须
        /// </summary>
        public long cid { get; set; }
        /// <summary>
        ///     扩展字段，非必须
        /// </summary>
        public string extend { get; set; }

        /// <summary>
        ///     布尔值扩展字段
        /// </summary>
        public bool? boolFlag { get; set; }

        public int? intFlag { get; set; }

        /// <summary>
        ///     开始日期，从所选日起
        /// </summary>
        public DateTime? datestart { get; set; }

        /// <summary>
        ///     结束日期，至所选日结束（包括所选日）
        /// </summary>
        public DateTime? dateend { get; set; }
    }

}
