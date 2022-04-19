using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timetable.Web.MiniApp.WeChatModel
{
    public class WeChatLoginRequest
    {

        public string encryptedData { get; set; }
        public string iv { get; set; }

        public int id { get; set; }

    }
}
