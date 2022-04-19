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
    public class NoticeController : ControllerBase
    {
        [HttpPost("[action]")]
        public PagerResponse List(PagerRequest req)
        {
            using (var ctx = new TimeTableDbContext())
            {
                IQueryable<Notice> query = ctx.Notices;
                if (!string.IsNullOrEmpty(req.keyword))
                    query = query.Where(s => EF.Functions.ILike(s.Title, "%" + req.keyword + "%") || EF.Functions.ILike(s.Content, "%" + req.keyword + "%"));
                var total = query.Count();
                return (new PagerResponse
                {
                    total = total,
                    data = query.OrderBy(s => s.SN).ThenByDescending(s=>s.CreateTime).Skip((req.page - 1) * req.rows).Take(req.rows).Select(s => new
                    {
                        s.Id,
                        s.SN,
                        s.Title,
                        s.Content,
                        s.CreateTime,
                       
                        s.Enable
                    }).ToList()
                });
            }
        }

        [HttpPost("[action]")]
        public ResponseBase Add(Notice notice)
        {
            using (var ctx = new TimeTableDbContext())
            {
                notice.CreateTime = DateTime.Now;
                ctx.Add(notice);
                ctx.SaveChanges();
                return (new ResponseBase
                {
                    success = true,
                    msg = "添加成功"
                });
            }
        }


        [HttpPost("[action]")]
        public ResponseBase Edit(Notice notice)
        {
            using (var ctx = new TimeTableDbContext())
            {
                var target = ctx.Notices.FirstOrDefault(s => s.Id == notice.Id);
                if (target != null)
                {
                    target.Title = notice.Title;
                    target.Content = notice.Content;
                    target.SN = notice.SN; 
                    target.Enable = notice.Enable;
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
                var target = ctx.Notices.FirstOrDefault(s => s.Id ==id);
                if (target != null)
                {
                    ctx.Notices.Remove(target);
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