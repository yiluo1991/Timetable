using System;
using System.Collections.Generic;
using System.Text;

namespace Timetable.Web.CommonViewModel
{
    public class ErrorResponse : ResponseBase
    { 
        public string errCode { get; set; }
    }
}
