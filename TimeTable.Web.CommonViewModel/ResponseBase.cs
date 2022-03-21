using System;

namespace Timetable.Web.CommonViewModel
{
    public class ResponseBase
    {
        public ResponseBase()
        {
            clientNotice = false;
        }
        public bool clientNotice { get; set; }
        /// <summary>
        ///  是否成功
        /// </summary>

        public bool success { get; set; }

        /// <summary>
        ///     返回数据
        /// </summary>
        public Object data { get; set; }

        /// <summary>
        ///     提示信息
        /// </summary>
        public string msg { get; set; }
    }
}
