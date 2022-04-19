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
using Timetable.Web.OA.ViewModel;

namespace Timetable.Web.OA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
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
                IQueryable<Subject> query = ctx.Subjects;
                if (!string.IsNullOrEmpty(req.keyword))
                {
                    var k = "%" + req.keyword + "%";
                    query = query.Where(s => EF.Functions.ILike(s.College.Name, k) || EF.Functions.ILike(s.Department.Name, k) || EF.Functions.ILike(s.Name, k) || EF.Functions.ILike(s.SubjectCode, k));
                }
                return new PagerResponse
                {

                    total = query.Count(),
                    data = query.OrderBy(s => s.CollegeId).ThenByDescending(s => s.DepartmentId).Skip((req.page-1)*req.rows).Take(req.rows).Select(s => new
                    {
                        s.CollegeId,
                        s.DepartmentId,
                        CollegeName = s.CollegeId.HasValue ? s.College.Name : null,
                        DepartmentName = s.DepartmentId.HasValue ? s.Department.Name : null,
                        s.Enable,
                        s.Name,
                        s.SubjectCode,
                        s.SubjectProperty,
                        s.SubjectType
                    }).ToList(),
                    msg = "获取成功",
                    success = true
                };
            }
        }
        [HttpPost("[action]")]
        public PagerResponse EnableList(PagerRequest req)
        {
            using (TimeTableDbContext ctx = new TimeTableDbContext())
            {
                IQueryable<Subject> query = ctx.Subjects.Where(s=>s.Enable);
                if (!string.IsNullOrEmpty(req.keyword))
                {
                    var k = "%" + req.keyword + "%";
                    query = query.Where(s => EF.Functions.ILike(s.College.Name, k) || EF.Functions.ILike(s.Department.Name, k) || EF.Functions.ILike(s.Name, k) || EF.Functions.ILike(s.SubjectCode, k));
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
                        s.Enable,
                        s.Name,
                        s.SubjectCode,
                        s.SubjectProperty,
                        s.SubjectType
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
        public ResponseBase Add(SubjectRequestViewModel subject)
        {

            subject.SubjectCode= subject.SubjectCode.Trim();
            using (TimeTableDbContext ctx = new TimeTableDbContext())
            {
                if (ctx.Subjects.Count(s => EF.Functions.ILike(s.SubjectCode, subject.SubjectCode)) > 0)
                {
                    return new ErrorResponse
                    {
                        success = false,
                        errCode = "ERR_ID_UNIQUE",
                        msg = "课程编码已存在"
                    };
                }
                else
                {
                    ctx.Subjects.Add(new Subject
                    {
                        CollegeId = subject.CollegeId,
                        DepartmentId = subject.DepartmentId,
                        Enable = subject.Enable,
                        SubjectCode = subject.SubjectCode,
                        Name = subject.Name,
                        SubjectType = subject.SubjectType,
                        SubjectProperty = subject.SubjectProperty
                    });
                    ctx.SaveChanges();
                    return new ResponseBase { success = true, msg = "添加成功" };
                }
            }
        }

        /// <summary>
        ///     修改
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>

        [HttpPost("[action]")]
        public ResponseBase Edit(SubjectRequestViewModel subject)
        {
            subject.SubjectCode= subject.SubjectCode.Trim();
            using (TimeTableDbContext ctx = new TimeTableDbContext())
            {
                var target = ctx.Subjects.FirstOrDefault(s => s.SubjectCode == subject.OriginSubjectCode);
                if (target != null)
                {
                    if (ctx.Subjects.FirstOrDefault(s => EF.Functions.ILike(s.SubjectCode, target.SubjectCode) && s.SubjectCode != subject.OriginSubjectCode) == null)
                    {
                        target.DepartmentId = subject.DepartmentId;
                        target.CollegeId = subject.CollegeId;
                        target.Enable = subject.Enable;
                      
                        target.SubjectProperty = subject.SubjectProperty;
                        target.SubjectType = subject.SubjectType;
                        target.Name = subject.Name;
                       
                        try
                        {
                            
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
                            success = false,
                            errCode = "ERR_ID_UNIQUE",
                            msg = "课程编码已存在"
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
        public ResponseBase Delete([FromQuery]string id)
        {
            var ctx = new TimeTableDbContext();
            var target = ctx.Subjects.FirstOrDefault(s => s.SubjectCode== id);
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
                ctx.Subjects.Remove(target);
                try
                {
                    ctx.SaveChanges();
                    return new ResponseBase { success = true, msg = "删除成功" };
                }
                catch (Exception)
                {
                    return new ErrorResponse
                    {
                        msg = "科目有相关数据，无法删除",
                        success = false,
                        errCode = "ERR_IN_USE"
                    };
                  
                } 

            }
        }

        /// <summary>
        ///     检查课程码是否可用
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public bool CheckExist([FromQuery]string skip,[FromQuery]string code)
        {
            var ctx = new TimeTableDbContext();
            var target = ctx.Subjects.FirstOrDefault(s =>EF.Functions.ILike(s.SubjectCode,code)&&s.SubjectCode!=skip);
            if (null == target)
            {
                return true;
            }
            else
            {
                return false;

            }
        }
    }
}