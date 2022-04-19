using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Timetable.DbContext;
using Timetable.DbContext.Models;
using Timetable.Tools;
using Timetable.Web.CommonViewModel;
using Timetable.Web.OA.ViewModel;

namespace Timetable.Web.OA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        /// <summary>
        ///     本学期列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public PagerResponse CurrentList(PagerRequest req)
        {
            using (TimeTableDbContext ctx = new TimeTableDbContext())
            {
                var currentTerm = SchoolTermCacheManager.GetCurrentTerm();
                if (currentTerm == null)
                    return new PagerResponse
                    {
                        msg = "当前没有找到默认学期，无法查询内容。",
                        success = false,
                        total = 0
                    };
                //  限定学期范围
                IQueryable<Course> query = ctx.Courses.Where(s => s.SchoolTermId == currentTerm.Id); 
                if (!string.IsNullOrWhiteSpace(req.keyword))
                {
                    var k = "%" + req.keyword + "%";
                    query = query.Where(s => EF.Functions.ILike(s.AdministrativeClass.FullName, k) || EF.Functions.ILike(s.College.Name, k) || EF.Functions.ILike(s.CourseName, k) || EF.Functions.ILike(s.Department.Name, k) || EF.Functions.ILike(s.SchoolTerm.Year.ToString(), k) || EF.Functions.ILike(s.Teacher.RealName, k) || EF.Functions.ILike(s.TeacherIdentityCode, k) || EF.Functions.ILike(s.Subject.Name, k) || EF.Functions.ILike(s.SubjectCode, k));
                }
                return new PagerResponse
                {
                    total = query.Count(),
                    data = query.OrderByDescending(s => s.Id).Skip((req.page - 1) * req.rows).Take(req.rows).ToList().Select(s => new
                    {
                        s.Id,
                        s.AdministrativeClassId,
                        AdministrativeClassName = s.AdministrativeClass?.FullName,
                        s.CollegeId, 
                        s.Address,
                        CollegeName = s.College!=null? s.College.Name:null,
                        s.DepartmentId,
                        DepartmentName =s.Department?.Name,
                        s.CourseName,
                        s.SchoolTermId,
                        s.SchoolTerm.Year,
                        s.SchoolTerm.Term,
                        s.SubjectCode,
                        s.Subject.Name,
                        TeacherName = s.Teacher.RealName,
                        s.TeacherIdentityCode,
                        TimebookJson = JsonConvert.DeserializeObject<Timebook>(s.TimebookJson.GetRawText())

                    }).ToList(),
                    msg = "获取成功",
                    success = true
                };
            }


        }

        /// <summary>
        ///     所有学期列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public PagerResponse AllList(PagerRequest req)
        {
            using (TimeTableDbContext ctx = new TimeTableDbContext())
            {

                //  根据cid划取学期范围
                IQueryable<Course> query = ctx.Courses;
                if (req.cid != 0)
                {
                    query = query.Where(s => s.SchoolTerm.Year==req.cid);
                }
                
                if (req.intFlag.HasValue)
                {
                    query = query.Where(s => s.SchoolTerm.Term == req.intFlag.Value);
                } 

                if (!string.IsNullOrWhiteSpace(req.keyword))
                {
                    var k = "%" + req.keyword + "%";
                    query = query.Where(s => EF.Functions.ILike(s.AdministrativeClass.FullName, k) || EF.Functions.ILike(s.College.Name, k) || EF.Functions.ILike(s.CourseName, k) || EF.Functions.ILike(s.Department.Name, k) || EF.Functions.ILike(s.SchoolTerm.Year.ToString(), k) || EF.Functions.ILike(s.Teacher.RealName, k) || EF.Functions.ILike(s.TeacherIdentityCode, k) || EF.Functions.ILike(s.Subject.Name, k) || EF.Functions.ILike(s.SubjectCode, k));
                }
                return new PagerResponse
                {
                    total = query.Count(),
                    data = query.OrderByDescending(s => s.Id).Skip((req.page - 1) * req.rows).Take(req.rows).ToList().Select(s => new
                    {
                        s.Id,
                        s.AdministrativeClassId,
                        AdministrativeClassName = s.AdministrativeClass?.FullName,
                        s.CollegeId,
                        CollegeName = s.College != null ? s.College.Name : null,
                        s.DepartmentId,
                        DepartmentName = s.Department?.Name,
                        s.CourseName,
                        s.SchoolTermId,
                        s.Address,
                        s.SchoolTerm.Year,
                        s.SchoolTerm.Term,
                        s.SubjectCode,
                        s.Subject.Name,
                        TeacherName = s.Teacher.RealName,
                        s.TeacherIdentityCode,
                        TimebookJson = JsonConvert.DeserializeObject<Timebook>(s.TimebookJson.GetRawText())

                    }).ToList(),
                    msg = "获取成功",
                    success = true
                };
            }


        }

        /// <summary>
        ///     添加
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public ResponseBase Add(CourseViewModel c)
        {
            using (TimeTableDbContext ctx = new TimeTableDbContext())
            {
                var course = new Course() { CourseName=c.CourseName};
                var currentTerm = SchoolTermCacheManager.GetCurrentTerm();
                //检查学期
                if (currentTerm == null)
                    //未找到学期返回错误提示
                    return new ErrorResponse
                    {
                        errCode = "ERR_NOT_FOUND",
                        msg = "当前没有找到默认学期，无法录入学期课程信息。",
                        success = false
                    };
                else
                    //设置学期
                    course.SchoolTermId = currentTerm.Id;

                //检查科目
                var subject = ctx.Subjects.FirstOrDefault(s => s.SubjectCode == c.SubjectCode && s.Enable);
                if (subject == null)
                    return new ErrorResponse
                    {
                        errCode = "ERR_NOT_FOUND",
                        msg = "未找到对应的可用科目。",
                        success = false
                    };
                else {
                    course.SubjectCode = c.SubjectCode;
                    course.CollegeId = subject.CollegeId;
                    course.DepartmentId = subject.DepartmentId;
                }
                //检查教师
                var teacher = ctx.Teachers.FirstOrDefault(s => s.Enable == true && s.IdentityCode == c.TeacherIdentityCode);
                if (teacher == null)
                    return new ErrorResponse
                    {
                        errCode = "ERR_NOT_FOUND",
                        msg = "未找到对应的可用教师信息。",
                        success = false
                    };
                else
                    course.TeacherIdentityCode = c.TeacherIdentityCode;

                if (c.AdministrativeClassId.HasValue)
                {
                    //  检查班级
                    var ac = ctx.AdministrativeClasses.FirstOrDefault(s => s.AdministrativeClassState == AdministrativeClassState.Normal && s.Id == c.AdministrativeClassId.Value);
                    if (ac == null)
                    {
                        return new ErrorResponse
                        {
                            errCode = "ERR_NOT_FOUND",
                            msg = "未找到对应的可用授课班级。",
                            success = false
                        };
                    }
                    else
                        course.AdministrativeClassId = c.AdministrativeClassId.Value;
                }
                course.TimebookJson = JsonDocument.Parse(JsonConvert.SerializeObject(c.TimebookJson)).RootElement;

                course.Address = c.Address;
                ctx.Courses.Add(course);
                ctx.SaveChanges();
                return new ResponseBase
                {
                    success = true,
                    msg = "添加成功"
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
            using(TimeTableDbContext ctx=new TimeTableDbContext())
            {
                var course = new Course();
                var currentTerm = SchoolTermCacheManager.GetCurrentTerm();
               
                //检查学期
                if (currentTerm == null)
                    //未找到学期返回错误提示
                    return new ErrorResponse
                    {
                        errCode = "ERR_NOT_FOUND",
                        msg = "当前没有找到默认学期，无法操作。",
                        success = false
                    };

                var target = ctx.Courses.FirstOrDefault(s => s.Id == id && s.SchoolTermId == currentTerm.Id);
                if (target == null)
                {
                    return new ErrorResponse
                    {
                        errCode = "ERR_NOT_FOUND",
                        msg = "在当前学期中没有找到对应课程信息。",
                        success = false
                    };
                }
                ctx.CoursePicks.RemoveRange(target.CoursePicks);
               if(target.AdminstractiveClassBackup != null)
                {
                    ctx.AdministrativeClassBackups.Remove(target.AdminstractiveClassBackup);
                }
                ctx.Courses.Remove(target);
                ctx.SaveChanges();
                return new ResponseBase { msg = "删除成功", success = true };
            }
        }

    }
}