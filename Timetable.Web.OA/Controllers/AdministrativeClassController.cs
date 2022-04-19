using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Timetable.DbContext;
using Timetable.DbContext.Models;
using Timetable.Web.CommonViewModel;

namespace Timetable.Web.OA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministrativeClassController : ControllerBase
    {
        /// <summary>
        ///  分页获取
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public PagerResponse List(PagerRequest req)
        {
            using (TimeTableDbContext ctx = new TimeTableDbContext())
            {
                IQueryable<AdministrativeClass> query = ctx.AdministrativeClasses;
                if (!string.IsNullOrEmpty(req.keyword))
                {
                    var k = "%" + req.keyword + "%";
                    query = query.Where(s => EF.Functions.ILike(s.College.Name, k) || EF.Functions.ILike(s.Department.Name, k) || EF.Functions.ILike(s.FullName, k) || EF.Functions.ILike(s.Grade.ToString(), k) || EF.Functions.ILike(s.Id.ToString(), req.keyword));
                }
                return new PagerResponse
                {

                    total = query.Count(),
                    data = query.OrderBy(s => s.CollegeId).ThenByDescending(s => s.DepartmentId).Skip((req.page - 1) * req.rows).Take(req.rows).Select(s => new
                    {
                        s.CollegeId,
                        s.DepartmentId,
                        CollegeName = s.CollegeId.HasValue ? s.College.Name : null,
                        DepartmentName = s.DepartmentId.HasValue ? s.Department.Name : null,
                        s.AdministrativeClassState,
                        s.FullName,
                        s.Grade,
                        s.Id

                    }).ToList(),
                    msg = "获取成功",
                    success = true
                };
            }
        }


        [HttpPost("[action]")]
        public PagerResponse CurrentList(PagerRequest req)
        {
            using (TimeTableDbContext ctx = new TimeTableDbContext())
            {
                IQueryable<AdministrativeClass> query = ctx.AdministrativeClasses.Where(s=>s.AdministrativeClassState== AdministrativeClassState.Normal);
                if (!string.IsNullOrEmpty(req.keyword))
                {
                    var k = "%" + req.keyword + "%";
                    query = query.Where(s => EF.Functions.ILike(s.College.Name, k) || EF.Functions.ILike(s.Department.Name, k) || EF.Functions.ILike(s.FullName, k) || EF.Functions.ILike(s.Grade.ToString(), k) || EF.Functions.ILike(s.Id.ToString(), req.keyword));
                }
                return new PagerResponse
                {

                    total = query.Count(),
                    data = query.OrderBy(s => s.CollegeId).ThenByDescending(s => s.DepartmentId).Skip((req.page - 1) * req.rows).Take(req.rows).Select(s => new
                    {
                        s.CollegeId,
                        s.DepartmentId,
                        CollegeName = s.CollegeId.HasValue ? s.College.Name : null,
                        DepartmentName = s.DepartmentId.HasValue ? s.Department.Name : null,
                        s.AdministrativeClassState,
                        s.FullName,
                        s.Grade,
                        s.Id

                    }).ToList(),
                    msg = "获取成功",
                    success = true
                };
            }
        }


        /// <summary>
        ///     添加
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public ResponseBase Add(AdministrativeClass c)
        {


            using (TimeTableDbContext ctx = new TimeTableDbContext())
            {
                c.AdministrativeClassBackups = null;
                c.College = null;
                c.Courses = null;
                c.Department = null;
                c.Students = null;
                ctx.AdministrativeClasses.Add(c);
                ctx.SaveChanges();
                return new ResponseBase { success = true, msg = "添加成功" };

            }
        }

        /// <summary>
        ///     修改
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>

        [HttpPost("[action]")]
        public ResponseBase Edit(AdministrativeClass c)
        {

            using (TimeTableDbContext ctx = new TimeTableDbContext())
            {
                var target = ctx.AdministrativeClasses.FirstOrDefault(s => s.Id == c.Id);
                if (target != null)
                {

                    try
                    {
                        target.AdministrativeClassState = c.AdministrativeClassState;
                        target.CollegeId = c.CollegeId;
                        target.DepartmentId = c.DepartmentId;
                        target.FullName = c.FullName;
                        target.Grade = c.Grade;
                        target.Students.ToList().ForEach(s => {
                            s.DepartmentId = c.DepartmentId;
                            s.CollegeId = c.CollegeId;

                        });
                        ctx.SaveChanges();
                        return new ResponseBase { success = true, msg = "修改成功" };
                    }
                    catch (Exception)
                    {

                        return new ErrorResponse
                        {
                            msg = "外键引用错误",
                            success = false,
                            errCode = "ERR_FOREIGN_KEY"
                        };
                    }

                }
                else
                {
                    return new ErrorResponse
                    {
                        msg = "没有找到要修改的信息",
                        success = false,
                        errCode = "ERR_NOT_FOUND"
                    };
                }
            }
        }


        /// <summary>
        ///     删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public ResponseBase Delete([FromQuery]long id)
        {
            var ctx = new TimeTableDbContext();
            var target = ctx.AdministrativeClasses.FirstOrDefault(s => s.Id == id);
            if (null == target)
            {
                return (new ErrorResponse
                {
                    success = false,
                    errCode = "ERR_NO_FOUND",
                    msg = "没有找到要处理的数据"
                });
            }
            else
            {
                ctx.AdministrativeClasses.Remove(target);
                try
                {
                    ctx.SaveChanges();
                    return new ResponseBase { success = true, msg = "删除成功" };
                }
                catch (Exception)
                {
                    return new ErrorResponse
                    {
                        msg = "班级有相关数据，无法删除",
                        success = false,
                        errCode = "ERR_IN_USE"
                    };

                }

            }
        }



    }
}