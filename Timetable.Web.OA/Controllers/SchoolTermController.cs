using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Timetable.DbContext;
using Timetable.DbContext.Models;
using Timetable.Tools;
using Timetable.Web.CommonViewModel;
using Timetable.Web.OA.Infrastructure.Authorization;

namespace Timetable.Web.OA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Permission]
    public class SchoolTermController : ControllerBase
    {
        /// <summary>
        ///     分页获取学期列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public PagerResponse List(PagerRequest req)
        {
            using (TimeTableDbContext ctx = new TimeTableDbContext())
            {
                IQueryable<SchoolTerm> query = ctx.SchoolTerms;
                if (!string.IsNullOrEmpty(req.keyword))
                {
                    query = query.Where(s => EF.Functions.ILike(s.Year.ToString(), "%" + req.keyword + "%") || s.Term.ToString() == req.keyword);
                }
                return new PagerResponse
                {

                    total = query.Count(),
                    data = query.OrderByDescending(s => s.Year).ThenByDescending(s => s.Term).Select(s => new
                    {
                        s.Id,
                        s.CustomEnd,
                        s.CustomStart,
                        s.FixedStart,
                        s.IsDefault,
                        s.Term,
                        s.Year
                    }).ToList(),
                    msg = "获取成功",
                    success = true
                };
            }
        }

        /// <summary>
        ///     设为当前学期
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public ResponseBase SetDefault(long id)
        {

            using (TimeTableDbContext ctx = new TimeTableDbContext())
            {
                var target = ctx.SchoolTerms.FirstOrDefault(s => s.Id == id);
                if (target != null)
                {
                    var ds = ctx.SchoolTerms.Where(s => s.IsDefault).ToList();
                    ds.ForEach(s =>
                    {
                        s.IsDefault = false;
                    });
                    SchoolTermCacheManager.ClearCache();
                    target.IsDefault = true;
                    ctx.SaveChanges();
                    return new ResponseBase
                    {
                        success = true,
                        msg = "操作成功"
                    };
                }
                else
                {
                    return new ErrorResponse
                    {
                        msg = "没有找到相应学年信息",
                        success = false,
                        errCode = "ERR_NOT_FOUND"
                    };
                }
            }
        }

        /// <summary>
        ///     删除学年
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public ResponseBase Delete(long id)
        {
            using (TimeTableDbContext ctx = new TimeTableDbContext())
            {
                var target = ctx.SchoolTerms.FirstOrDefault(s => s.Id == id);
                if (target != null)
                {
                    try
                    {
                        if (!target.IsDefault)
                        {
                            ctx.SchoolTerms.Remove(target);
                            ctx.SaveChanges();
                            return new ResponseBase
                            {
                                success = true,
                                msg = "操作成功"
                            };
                        }
                        else
                        {
                            return new ErrorResponse
                            {
                                msg = "不可以删除当前学期，请将当前学期设为其他学期后进行操作",
                                success = false,
                                errCode = "ERR_IN_USE"
                            };
                        }

                    }
                    catch (Exception)
                    {
                        return new ErrorResponse
                        {
                            msg = "学年中有数据，请清空后删除",
                            success = false,
                            errCode = "ERR_FOREIGN_KEY_LIMIT"
                        };
                    }
                }
                else
                {
                    return new ErrorResponse
                    {
                        msg = "没有找到相应学年信息",
                        success = false,
                        errCode = "ERR_NOT_FOUND"
                    };
                }
            }
        }


        /// <summary>
        ///  清空数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public ResponseBase Clear(long id)
        {
            using (TimeTableDbContext ctx = new TimeTableDbContext())
            {
                var target = ctx.SchoolTerms.FirstOrDefault(s => s.Id == id);
                if (target != null)
                {
                    ctx.Courses.RemoveRange(target.Courses);
                    ctx.SaveChanges();
                    return new ResponseBase
                    {
                        success = true,
                        msg = "操作成功"
                    };
                }
                else
                {
                    return new ErrorResponse
                    {
                        msg = "没有找到相应学年信息",
                        success = false,
                        errCode = "ERR_NOT_FOUND"
                    };
                }
            }
        }

        /// <summary>
        ///     添加
        /// </summary>
        /// <param name="schoolTerm"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public ResponseBase Add(SchoolTerm schoolTerm)
        {
            using (TimeTableDbContext ctx = new TimeTableDbContext())
            {
                var existTerm = ctx.SchoolTerms.FirstOrDefault(s => s.Year == schoolTerm.Year && s.Term == schoolTerm.Term);
                if (existTerm == null)
                {
                    schoolTerm.FixedStart = CalendarHelper.GetPreMonDay(schoolTerm.CustomStart);

                    schoolTerm.IsDefault = ctx.SchoolTerms.Count(s => s.IsDefault) > 0 ? false : true;
                    schoolTerm.Courses = null;
                    ctx.SchoolTerms.Add(schoolTerm);
                    ctx.SaveChanges();
                    return new ResponseBase
                    {
                        success = true,
                        msg = "操作成功"
                    };
                }
                else
                {
                    return new ErrorResponse
                    {
                        success = false,
                        errCode = "ERR_UNIQUE",
                        msg = "已存在要添加的学期，添加失败。"
                    };
                }


            }
        }

        [HttpPost("[action]")]
        public ResponseBase GetCurrentTerm()
        {
            var v = SchoolTermCacheManager.GetCurrentTerm();
            if (v != null)
            {
                return new ResponseBase
                {
                    success = true,
                    data = v
                };
            }
            else
            {
                return new ErrorResponse
                {
                    errCode = "ERR_NOT_FOUND",
                    msg = "管理员请在学期管理中设置当前学期",
                    success = false
                };
            }
        }
    }
}