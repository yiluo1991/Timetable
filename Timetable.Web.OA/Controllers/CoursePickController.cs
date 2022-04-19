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

namespace Timetable.Web.OA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursePickController : ControllerBase
    {


        /// <summary>
        ///     所有学期列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public PagerResponse List(PagerRequest req)
        {
            using (TimeTableDbContext ctx = new TimeTableDbContext())
            {
                //  根据cid划取学期范围
                IQueryable<CoursePick> query = ctx.CoursePicks;
                if (req.cid != 0)
                {
                    query = query.Where(s => s.Course.SchoolTerm.Year == req.cid);
                }

                if (req.intFlag.HasValue)
                {
                    query = query.Where(s => s.Course.SchoolTerm.Term == req.intFlag.Value);
                }
                if (!string.IsNullOrWhiteSpace(req.keyword))
                {
                    var k = "%" + req.keyword + "%";
                    query = query.Where(s => EF.Functions.ILike(s.Course.College.Name, k) || EF.Functions.ILike(s.Course.Department.Name, k) || EF.Functions.ILike(s.Course.Subject.Name, k) || EF.Functions.ILike(s.Course.CourseName, k) || EF.Functions.ILike(s.Student.IdentityCode, k) || EF.Functions.ILike(s.Student.RealName, k) || EF.Functions.ILike(s.Course.Teacher.RealName, k) || EF.Functions.ILike(s.Course.Teacher.IdentityCode, k) || EF.Functions.ILike(s.Student.AdministrativeClass.FullName, k));
                }
                return new PagerResponse
                {
                    total = query.Count(),
                    data = query.OrderByDescending(s => s.Id).Skip((req.page - 1) * req.rows).Take(req.rows).ToList().Select(s => new
                    {
                        s.Id,
                        Course = new
                        {
                            s.CourseId,
                            s.Course.CourseName,
                            TeacherName = s.Course.Teacher.RealName,
                            s.Course.TeacherIdentityCode,
                            SubjectName = s.Course.Subject.Name,
                            CollegeName = s.Course.College?.Name,
                            Department = s.Course.Department?.Name,
                            s.Course.SchoolTerm.Year,
                            s.Course.SchoolTerm.Term,
                        },
                        Student = new
                        {
                            s.StudentIdentityCode,
                            s.Student.RealName,
                            s.Student.AdministrativeClass.FullName
                        }
                    }).ToList(),
                    msg = "获取成功",
                    success = true
                };
            }


        }

        [HttpPost("[action]")]
        public ResponseBase Add([FromForm] long courseId, [FromForm] string studentId)
        {
            using (TimeTableDbContext ctx = new TimeTableDbContext())
            {
                var course = ctx.Courses.FirstOrDefault(s => s.Id == courseId);
                if (course != null) {
                    var student = ctx.Students.FirstOrDefault(s => s.IdentityCode == studentId);
                    if (student != null)
                    {
                        if (ctx.CoursePicks.Count(s => s.CourseId == courseId && s.StudentIdentityCode == studentId) == 0)
                        {
                            var cp = new CoursePick { 
                             CourseId=course.Id,
                              StudentIdentityCode=student.IdentityCode
                            };
                            ctx.CoursePicks.Add(cp);
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
                                errCode = "ERR_CHECK_EXIEST",
                                msg = "该学生已选课程，请勿重复选课",
                                success = false
                            };
                        }
                    }
                }
                return new ErrorResponse { 
                     errCode="ERR_NOT_FOUND",
                     msg="没有找到响应课程信息或学生信息",
                     success=false
                };
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
            using (TimeTableDbContext ctx = new TimeTableDbContext())
            {
               
                var target = ctx.CoursePicks.FirstOrDefault(s =>s.Id==id);
                if (target == null)
                {
                    return new ErrorResponse
                    {
                        errCode = "ERR_NOT_FOUND",
                        msg = "没有找到要删除的选课信息。",
                        success = false
                    };
                }
             
                ctx.CoursePicks.Remove(target);
                ctx.SaveChanges();
                return new ResponseBase { msg = "删除成功", success = true };
            }
        }
    }
}