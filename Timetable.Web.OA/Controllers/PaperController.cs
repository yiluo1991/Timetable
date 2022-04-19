using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Timetable.DbContext;
using Timetable.DbContext.Models;
using Timetable.Web.CommonViewModel;
using Timetable.Web.OA.Infrastructure.Authorization;
using Timetable.Web.OA.Infrastructure.PermissionManager;
using Timetable.Web.OA.ViewModel;

namespace Timetable.Web.OA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Permission]
    public class PaperController : ControllerBase
    {
        [HttpPost("[action]")]
        public PagerResponse List(PagerRequest req)
        {
            using (var ctx = new TimeTableDbContext())
            {
                IQueryable<Paper> query = ctx.Papers;
                if (!string.IsNullOrEmpty(req.keyword))
                    query = query.Where(s => EF.Functions.ILike(s.Title, "%" + req.keyword + "%") || EF.Functions.ILike(s.Url, "%" + req.keyword + "%"));
                var total = query.Count();
                return (new PagerResponse
                {
                    total = total,
                    data = query.OrderBy(s => s.SN).Skip((req.page - 1) * req.rows).Take(req.rows).Select(s => new
                    {
                        s.Id,
                        s.SN,
                        s.Title,
                        s.Url,
                        s.Enable
                    }).ToList()
                });
            }
        }

        [HttpPost("[action]")]
        public ResponseBase Add(Paper paper)
        {
            using (var ctx = new TimeTableDbContext())
            {
                ctx.Add(paper);
                ctx.SaveChanges();
                return (new ResponseBase
                {
                    success = true,
                    msg = "添加成功"
                });
            }
        }


        [HttpPost("[action]")]
        public ResponseBase Edit(Paper paper)
        {
            using (var ctx = new TimeTableDbContext())
            {
                var target = ctx.Papers.FirstOrDefault(s => s.Id == paper.Id);
                if (target != null)
                {
                    target.Title = paper.Title;
                    target.Url = paper.Url;
                    target.SN = paper.SN;
                    target.Enable = paper.Enable;
                    ctx.SaveChanges();
                  
                    return (new ResponseBase
                    {
                        success = true,
                        msg = "修改成功"
                    });
                }
                else
                {

                    return (new ErrorResponse
                    {
                        success = false,
                        errCode = "ERR_NO_FOUND",
                        msg = "没有找到要处理的数据"

                    });
                }

            }
        }


        [HttpPost("[action]")]
        public ResponseBase Delete(long id)
        {
            using (var ctx = new TimeTableDbContext())
            {
                var target = ctx.Papers.FirstOrDefault(s => s.Id ==id);
                if (target != null)
                {
                    ctx.Papers.Remove(target);
                    ctx.SaveChanges();
                 
                    return (new ResponseBase
                    {
                        success = true,
                        msg = "删除成功"
                    });
                }
                else
                {

                    return (new ErrorResponse
                    {
                        success = false,
                        errCode = "ERR_NO_FOUND",
                        msg = "没有找到要处理的数据"

                    });
                }

            }
        }

    }
}