using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Timetable.Web.MiniApp.WeChatModel
{
    public class SearchViewModel
    {
        public SearchViewModel() {
            Page = 1;
            Rows = 15;
        }
        public string Keyword { get; set; }

        public string Address { get; set; }

        public string Teacher { get; set; }

        public string Course { get; set; }

        public DateTime? Date { get; set; }


        public int Page { get; set; }

        public int? DepartmentId { get; set; }

        public int? CollegeId { get; set; }

        [Range(1,30,ErrorMessage ="分页大小1-30")]
        public int Rows { get; set; }

    }
}
