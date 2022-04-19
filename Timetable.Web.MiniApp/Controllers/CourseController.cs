using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Timetable.DbContext;
using Timetable.DbContext.Models;
using Timetable.Tools;
using Timetable.Tools.Model;
using Timetable.Web.CommonViewModel;
using Timetable.Web.MiniApp.Infrastructure.Authorization;
using Timetable.Web.MiniApp.WeChatModel;

namespace Timetable.Web.MiniApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        [HttpPost("[action]")]
        [Token]
        public ResponseBase Mine()
        {
            var username = User.Identity.Name.Substring(2);
            var type = User.FindFirst(ClaimTypes.Role).Value;
            SchoolTermCacheViewModel term = SchoolTermCacheManager.GetCurrentTerm();
            using (var ctx = new TimeTableDbContext())
            {
                if (type == "T")
                {
                    var teacher = ctx.Teachers.FirstOrDefault(s => s.IdentityCode == username);
                    if (teacher != null)
                    {
                        var courses = ctx.Courses.Where(s => s.SchoolTermId == term.Id && s.TeacherIdentityCode == username).ToList().Select(s => new
                        {
                            s.Address,
                            s.Id,
                            ClassName = s.AdministrativeClass?.FullName,
                            CollegeName = s.College?.Name,
                            DepartmentName = s.Department?.Name,
                            s.SubjectCode,
                            SubjectName = s.Subject.Name,
                            s.CourseName,
                            s.Teacher.RealName,
                            TimeBook = s.TimebookJson.GetRawText()
                        }).ToList();
                        return new ResponseBase
                        {
                            success = true,
                            data = courses,
                            msg = "获取成功"
                        };
                    }
                }
                else
                {
                    var student = ctx.Students.FirstOrDefault(s => s.IdentityCode == username);
                    if (student != null)
                    {
                        var classId = student.AdministrativeClassId;
                        var courses = ctx.Courses.Where(s => s.SchoolTermId == term.Id && (s.AdministrativeClassId == classId || s.CoursePicks.Count(t => t.StudentIdentityCode == username) > 0)).ToList().Select(s => new
                        {
                            s.Address,
                            s.Id,
                            ClassName = s.AdministrativeClass?.FullName,
                            CollegeName = s.College?.Name,
                            DepartmentName = s.Department?.Name,
                            s.SubjectCode,
                            SubjectName = s.Subject.Name,
                            s.CourseName,
                            s.Teacher.RealName,
                            TimeBook = s.TimebookJson.GetRawText()
                        }).ToList();
                        return new ResponseBase
                        {
                            success = true,
                            data = courses,
                            msg = "获取成功"
                        };
                    }
                }

                return new ErrorResponse
                {
                    success = false,
                    errCode = "ERR_NOT_FOUND",
                    msg = "没有找到用户信息"
                };
            }
        }

        [HttpGet("[action]")]
        [Token]
        public ResponseBase MineDate(DateTime date)
        {
            var username = User.Identity.Name.Substring(2);
            var type = User.FindFirst(ClaimTypes.Role).Value;
            SchoolTermCacheViewModel term = SchoolTermCacheManager.GetCurrentTerm();
            using (var ctx = new TimeTableDbContext())
            {
                string dateStr = date.ToString("yyyy-M-d");
                if (type == "T")
                {
                    var teacher = ctx.Teachers.FirstOrDefault(s => s.IdentityCode == username);
                    if (teacher != null)
                    {

                        var courses = ctx.Courses.Where(s => s.SchoolTermId == term.Id && s.TeacherIdentityCode == username && EF.Functions.JsonExists(s.TimebookJson.GetProperty("Book"), dateStr)).ToList().Select(s => new
                        {
                            s.Address,
                            s.Id,
                            ClassName = s.AdministrativeClass?.FullName,
                            CollegeName = s.College?.Name,
                            DepartmentName = s.Department?.Name,
                            s.SubjectCode,
                            SubjectName = s.Subject.Name,
                            s.CourseName,
                            s.Teacher.RealName,
                            TimeBook = s.TimebookJson.GetProperty("Book").GetProperty(dateStr).GetRawText(),
                            Desc = s.TimebookJson.GetProperty("Desc").GetString()
                        }).ToList();
                        return new ResponseBase
                        {
                            success = true,
                            data = courses,
                            msg = "获取成功"
                        };
                    }
                }
                else
                {
                    var student = ctx.Students.FirstOrDefault(s => s.IdentityCode == username);
                    if (student != null)
                    {
                        var classId = student.AdministrativeClassId;
                        var courses = ctx.Courses.Where(s => s.SchoolTermId == term.Id && EF.Functions.JsonExists(s.TimebookJson.GetProperty("Book"), dateStr)).Where(s => s.AdministrativeClassId == classId || s.CoursePicks.Count(t => t.StudentIdentityCode == username) > 0).ToList().Select(s => new
                        {
                            s.Address,
                            s.Id,
                            ClassName = s.AdministrativeClass?.FullName,
                            CollegeName = s.College?.Name,
                            DepartmentName = s.Department?.Name,
                            s.SubjectCode,
                            SubjectName = s.Subject.Name,
                            s.CourseName,
                            s.Teacher.RealName,
                            TimeBook = s.TimebookJson.GetProperty("Book").GetProperty(dateStr).GetRawText(),
                            Desc = s.TimebookJson.GetProperty("Desc").GetString()
                        }).ToList();
                        return new ResponseBase
                        {
                            success = true,
                            data = courses,
                            msg = "获取成功"
                        };
                    }
                }

                return new ErrorResponse
                {
                    success = false,
                    errCode = "ERR_NOT_FOUND",
                    msg = "没有找到用户信息"
                };
            }
        }


        [HttpGet("[action]")]
        [Token]
        public  ResponseBase MineWeek()
        {
            var username = User.Identity.Name.Substring(2);
            var date = CalendarHelper.GetPreMonDay(DateTime.Now);
            var type = User.FindFirst(ClaimTypes.Role).Value;
            SchoolTermCacheViewModel term = SchoolTermCacheManager.GetCurrentTerm();
            using (var ctx = new TimeTableDbContext())
            {
                List<string> dayList = new List<string>();
                for(var i = 0; i < 7; i++)
                {
                    dayList.Add(date.ToString("yyyy-M-d"));
                    date = date.AddDays(1);
                }
                var arr = dayList.ToArray();
                if (type == "T")
                {
                    var teacher = ctx.Teachers.FirstOrDefault(s => s.IdentityCode == username);
                    if (teacher != null)
                    {
                     
                        var courses = ctx.Courses.Where(s => s.SchoolTermId == term.Id && s.TeacherIdentityCode == username && EF.Functions.JsonExistAny(s.TimebookJson.GetProperty("Book"), arr)).ToList().Select(s => new
                        {
                            s.Address,
                            s.Id,
                            ClassName = s.AdministrativeClass?.FullName,
                            CollegeName = s.College?.Name,
                            DepartmentName = s.Department?.Name,
                            s.SubjectCode,
                            SubjectName = s.Subject.Name,
                            s.CourseName,
                            s.Teacher.RealName,
                            TimeBook = s.TimebookJson.GetProperty("Book").GetRawText(),
                            Desc = s.TimebookJson.GetProperty("Desc").GetString()
                        }).ToList();
                        return new ResponseBase
                        {
                            success = true,
                            data = courses,
                            msg = "获取成功"
                        };
                    }
                }
                else
                {
                    var student = ctx.Students.FirstOrDefault(s => s.IdentityCode == username);
                    if (student != null)
                    {
                        var classId = student.AdministrativeClassId;
                        var courses = ctx.Courses.Where(s => s.SchoolTermId == term.Id && EF.Functions.JsonExistAny(s.TimebookJson.GetProperty("Book"), arr)).Where(s => s.AdministrativeClassId == classId || s.CoursePicks.Count(t => t.StudentIdentityCode == username) > 0).ToList().Select(s => new
                        {
                            s.Address,
                            s.Id,
                            ClassName = s.AdministrativeClass?.FullName,
                            CollegeName = s.College?.Name,
                            DepartmentName = s.Department?.Name,
                            s.SubjectCode,
                            SubjectName = s.Subject.Name,
                            s.CourseName,
                            s.Teacher.RealName,
                            TimeBook = s.TimebookJson.GetProperty("Book").GetRawText(),
                            Desc = s.TimebookJson.GetProperty("Desc").GetString(),
                          
                        }).ToList();
                        return new ResponseBase
                        {
                            success = true,
                            data =courses,
                            msg = "获取成功"
                        };
                    }
                }

                return new ErrorResponse
                {
                    success = false,
                    errCode = "ERR_NOT_FOUND",
                    msg = "没有找到用户信息"
                };
            }
        }

        [HttpPost("[action]")]
        [Token]
        public ResponseBase Search(SearchViewModel req )
        {
            SchoolTermCacheViewModel term = SchoolTermCacheManager.GetCurrentTerm();
            using (var ctx = new TimeTableDbContext())
            {
                IQueryable<Course> query = ctx.Courses.Where(s => s.SchoolTermId == term.Id);

                if (!string.IsNullOrWhiteSpace(req.Address))
                {
                    query = query.Where(s => EF.Functions.ILike(s.Address, "%" + req.Address + "%"));
                }
                if (!string.IsNullOrWhiteSpace(req.Course))
                {
                    query = query.Where(s => EF.Functions.ILike(s.CourseName, "%" + req.Course + "%") || EF.Functions.ILike(s.Subject.Name, "%" + req.Course + "%"));
                }
                if (!string.IsNullOrWhiteSpace(req.Teacher))
                {
                    query = query.Where(s => EF.Functions.ILike(s.Teacher.RealName, "%" + req.Teacher + "%"));
                }
                if (!string.IsNullOrWhiteSpace(req.Keyword))
                {
                    query = query.Where(s => EF.Functions.ILike(s.CourseName, "%" + req.Keyword + "%") || EF.Functions.ILike(s.Subject.Name, "%" + req.Keyword + "%")|| EF.Functions.ILike(s.Teacher.RealName, "%" + req.Keyword + "%"));
                }
          
                if (req.Date.HasValue)
                {
                    string dateStr = req.Date.Value.ToString("yyyy-M-d");
                    query = query.Where(s => EF.Functions.JsonExists(s.TimebookJson.GetProperty("Book"), dateStr));
                }
                if (req.DepartmentId.HasValue)
                {
                    query = query.Where(s => s.DepartmentId == req.DepartmentId.Value);
                }
                if (req.CollegeId.HasValue)
                {
                    query = query.Where(s => s.CollegeId == req.CollegeId.Value);
                }

                return new ResponseBase
                {
                    success = true,
                    msg = "获取成功",
                    data = query.OrderByDescending(s => s.Id).ThenBy(s => s.CollegeId).ThenBy(s => s.DepartmentId).ThenBy(s => s.Teacher.CreateTime).Skip((req.Page - 1) * req.Rows).Take(req.Rows).ToList().Select(s => new
                    {
                        s.Address,
                        s.Id,
                        ClassName = s.AdministrativeClass?.FullName,
                        CollegeName = s.College?.Name,
                        DepartmentName = s.Department?.Name,
                        s.SubjectCode,
                        SubjectName = s.Subject.Name,
                        s.CourseName,
                        s.Teacher.RealName,
                        Desc = s.TimebookJson.GetProperty("Desc").GetString()
                    }).ToList()
                };
            }
        }
    }
}