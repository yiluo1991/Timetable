using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Timetable.Web.MiniApp.WeChatModel
{
    public class PasswordViewModel
    {
        public string Old { get; set; }
        [StringLength(32,MinimumLength =6,ErrorMessage ="请输入6到32位新密码")]
        public string New { get; set; }
    }
}
