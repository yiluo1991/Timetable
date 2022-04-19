using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Timetable.DbContext;
using Timetable.Web.CommonViewModel;
using Timetable.Web.MiniApp.Infrastructure.Authorization;
using Timetable.Web.MiniApp.WeChatModel;

namespace Timetable.Web.MiniApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollegeController : ControllerBase
    {
        /// <summary>
        ///     获取可用院系combo数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        [Token]
        public ResponseBase ComboData()
        {
            using (var ctx = new TimeTableDbContext())
            {
                var colleges = new List<CollegeDepartmentTreeViewModel>();
                ctx.Colleges.Where(s => s.Enable).OrderBy(s => s.Id).ToList().ForEach(s => {
                    colleges.Add(new CollegeDepartmentTreeViewModel
                    {
                      
                        Id = s.Id,
                        Name = s.Name,
                      
                        SID = "C_" + s.Id,
                        children = s.Departments.Where(s => s.Enable).OrderBy(t => t.Id).Select(t => new CollegeDepartmentTreeViewModel
                        {
                            Id = t.Id,
                            Name = t.Name,
                            SID = "D_" + t.Id
                        }).ToList()
                    });
                });
                return new ResponseBase()
                {
                    success = true,
                    data = colleges,
                    msg = "获取成功"
                };


            }
        }
    }
}