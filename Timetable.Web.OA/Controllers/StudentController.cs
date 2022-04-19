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
    public class StudentController : ControllerBase
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
                IQueryable<Student> query = ctx.Students;
                if (!string.IsNullOrEmpty(req.keyword))
                {
                    var k = "%" + req.keyword + "%";
                    query = query.Where(s => EF.Functions.ILike(s.College.Name, k) || EF.Functions.ILike(s.Department.Name, k) || EF.Functions.ILike(s.AdministrativeClass.FullName, k) || EF.Functions.ILike(s.IdentityCode, k) || EF.Functions.ILike(s.Mobile, k) || EF.Functions.ILike(s.RealName, k) || EF.Functions.ILike(s.SchoolOpenId, k)||EF.Functions.ILike(s.AdmissionYear.ToString(),k));
                }
                return new PagerResponse
                {

                    total = query.Count(),
                    data = query.OrderByDescending(s => s.CreateTime).Skip((req.page - 1) * req.rows).Take(req.rows).Select(s => new
                    {
                        s.AvatarUrl,
                        s.CreateTime,
                        s.AdministrativeClassId,
                        s.CollegeId,
                        s.DepartmentId,
                        AdministrativeClassName = s.AdministrativeClass.FullName ,
                        CollegeName = s.CollegeId.HasValue ? s.College.Name : null,
                        DepartmentName = s.DepartmentId.HasValue ? s.Department.Name : null,
                        s.Gender,
                        s.IdentityCode,
                        s.Mobile,
                        s.RealName,
                        s.SchoolOpenId,
                        s.AdmissionYear,
                        s.StudentState
                    }).ToList(),
                    msg = "获取成功",
                    success = true
                };
            }
        }



        /// <summary>
        ///     添加
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public ResponseBase Add(Student student)
        {

            student.IdentityCode = student.IdentityCode.Trim();
            using (TimeTableDbContext ctx = new TimeTableDbContext())
            {
                if (ctx.Students.Count(s => EF.Functions.ILike(s.IdentityCode, student.IdentityCode)) > 0)
                {
                    return new ErrorResponse
                    {
                        success = false,
                        errCode = "ERR_ID_UNIQUE",
                        msg = "学号已存在"
                    };
                }
                else
                {
                    student.CreateTime = DateTime.Now;
                    student.AdministrativeClass = null;
                    student.College = null;
                    student.CoursePicks = null;
                    student.Department = null;
                    var password = RandomHelper.CreateMixVerifyCode(6);
                    student.Password = CryptoHelper.Crypto.HashPassword(student.IdentityCode + password);
                    ctx.Students.Add(student);
                    ctx.SaveChanges();
                    return new ResponseBase { success = true, msg = "创建成功，学号【" + student.IdentityCode + "】，密码【" + password + "】" };
                }
            }
        }

        /// <summary>
        ///     修改
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>

        [HttpPost("[action]")]
        public ResponseBase Edit(Student student)
        {
            student.IdentityCode = student.IdentityCode.Trim();
            using (TimeTableDbContext ctx = new TimeTableDbContext())
            {
                var target = ctx.Students.FirstOrDefault(s => s.IdentityCode == student.IdentityCode);
                if (target != null)
                { 
                    target.StudentState = student.StudentState;
                    target.Gender = student.Gender;
                    target.Mobile = student.Mobile;
                    target.RealName = student.RealName;
                    target.AdministrativeClassId = student.AdministrativeClassId;
                    target.AdmissionYear = student.AdmissionYear;
                    target.CollegeId = student.CollegeId;
                    target.DepartmentId = student.DepartmentId;
                   
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
            var target = ctx.Students.FirstOrDefault(s => s.IdentityCode == id);
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
                ctx.Students.Remove(target);
                try
                {
                    ctx.SaveChanges();
                    return new ResponseBase { success = true, msg = "删除成功" };
                }
                catch (Exception)
                {
                    return new ErrorResponse
                    {
                        msg = "学生有相关数据，无法删除",
                        success = false,
                        errCode = "ERR_IN_USE"
                    };

                }

            }
        }

        /// <summary>
        ///     重置密码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public ResponseBase ResetPassword([FromQuery]string id)
        {
            var ctx = new TimeTableDbContext();
            var target = ctx.Students.FirstOrDefault(s => s.IdentityCode == id);
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


                var password = RandomHelper.CreateMixVerifyCode(6);
                target.Password = CryptoHelper.Crypto.HashPassword(target.IdentityCode + password);
                ctx.SaveChanges();
                return new ResponseBase { success = true, msg = "密码重置成功，学号【" + target.IdentityCode + "】，密码【" + password + "】" };


            }
        }

        /// <summary>
        ///     检查是否可用
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public bool CheckExist([FromQuery]string skip, [FromQuery]string code)
        {
            var ctx = new TimeTableDbContext();
            var target = ctx.Students.FirstOrDefault(s => EF.Functions.ILike(s.IdentityCode, code) && s.IdentityCode != skip);
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