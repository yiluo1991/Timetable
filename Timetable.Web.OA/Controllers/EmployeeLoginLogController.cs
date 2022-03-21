using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Timetable.DbContext;
using Timetable.DbContext.Models;
using Timetable.Web.CommonViewModel;
using Timetable.Web.OA.Infrastructure.Authorization;

namespace Timetable.Web.OA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Permission]
    public class EmployeeLoginLogController : ControllerBase
    {
        /// <summary>
        ///     分页获取登录记录列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public PagerResponse List(PagerRequest req)
        {
            using (var ctx = new TimeTableDbContext()) { 
       
           
            IQueryable<EmployeeLoginLog> query = ctx.EmployeeLoginLogs;
            if (req.datestart.HasValue)
            {
                var start = req.datestart.Value.Date;
                query= query.Where(s => s.LoginTime >= start);
            }
            if (req.dateend.HasValue)
            {
                var end = req.dateend.Value.Date.AddDays(1);
                query= query.Where(s => s.LoginTime < end);
            }
            if (!string.IsNullOrEmpty(req.keyword))
                query = query.Where(s => EF.Functions.ILike(s.Employee.RealName, "%" + req.keyword + "%") || EF.Functions.ILike(s.Employee.LoginName, "%" + req.keyword + "%") || EF.Functions.ILike(s.Employee.Mobile, "%" + req.keyword + "%") || EF.Functions.ILike(s.IP, "%" + req.keyword + "%") || EF.Functions.ILike(s.Remark, "%" + req.keyword + "%"));
            var total = query.Count();
            return (new PagerResponse
            {
                total = total,
                data = query.OrderByDescending(s => s.LoginTime).Skip((req.page - 1) * req.rows).Take(req.rows).Select(s => new
                {
                  s.IP,
                   s.Id,
                   s.Remark,
                   s.Employee.LoginName,
                   s.Employee.RealName,
                   s.Employee.Mobile,
                   s.LoginTime
                }).ToList()
            });
            }
        }
    }
}