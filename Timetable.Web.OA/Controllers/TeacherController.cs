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
    public class TeacherController : ControllerBase
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
                IQueryable<Teacher> query = ctx.Teachers;
                if (!string.IsNullOrEmpty(req.keyword))
                {
                    var k = "%" + req.keyword + "%";
                    query = query.Where(s => EF.Functions.ILike(s.IdentityCode, k) || EF.Functions.ILike(s.Mobile, k) || EF.Functions.ILike(s.RealName, k) || EF.Functions.ILike(s.SchoolOpenId, k));
                }
                return new PagerResponse
                {

                    total = query.Count(),
                    data = query.OrderByDescending(s => s.CreateTime).Skip((req.page - 1) * req.rows).Take(req.rows).Select(s => new
                    {
                        s.AvatarUrl,
                        s.CreateTime,
                        s.Enable,
                        s.Gender,
                        s.IdentityCode,
                        s.Mobile,
                        s.RealName,
                        s.SchoolOpenId

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
                IQueryable<Teacher> query = ctx.Teachers.Where(s=>s.Enable);
                if (!string.IsNullOrEmpty(req.keyword))
                {
                    var k = "%" + req.keyword + "%";
                    query = query.Where(s => EF.Functions.ILike(s.IdentityCode, k) || EF.Functions.ILike(s.Mobile, k) || EF.Functions.ILike(s.RealName, k) || EF.Functions.ILike(s.SchoolOpenId, k));
                }
                return new PagerResponse
                {

                    total = query.Count(),
                    data = query.OrderByDescending(s => s.CreateTime).Skip((req.page - 1) * req.rows).Take(req.rows).Select(s => new
                    {
                        s.AvatarUrl,
                        s.CreateTime,
                        s.Enable,
                        s.Gender,
                        s.IdentityCode,
                        s.Mobile,
                        s.RealName,
                        s.SchoolOpenId

                    }).ToList(),
                    msg = "获取成功",
                    success = true
                };
            }
        }

        /// <summary>
        ///     添加
        /// </summary>
        /// <param name="teacher"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public ResponseBase Add(Teacher teacher)
        {

            teacher.IdentityCode = teacher.IdentityCode.Trim();
            using (TimeTableDbContext ctx = new TimeTableDbContext())
            {
                if (ctx.Teachers.Count(s => EF.Functions.ILike(s.IdentityCode, teacher.IdentityCode)) > 0)
                {
                    return new ErrorResponse
                    {
                        success = false,
                        errCode = "ERR_ID_UNIQUE",
                        msg = "教工号已存在"
                    };
                }
                else
                {
                    teacher.CreateTime = DateTime.Now;
                    teacher.Courses = null;
                    var password = RandomHelper.CreateMixVerifyCode(6);
                    teacher.Password = CryptoHelper.Crypto.HashPassword(teacher.IdentityCode + password);
                    ctx.Teachers.Add(teacher);
                    ctx.SaveChanges();
                    return new ResponseBase { success = true, msg = "创建成功，教工号【" + teacher.IdentityCode + "】，密码【" + password + "】" };
                }
            }
        }

        /// <summary>
        ///     修改
        /// </summary>
        /// <param name="teacher"></param>
        /// <returns></returns>

        [HttpPost("[action]")]
        public ResponseBase Edit(Teacher teacher)
        {
            teacher.IdentityCode = teacher.IdentityCode.Trim();
            using (TimeTableDbContext ctx = new TimeTableDbContext())
            {
                var target = ctx.Teachers.FirstOrDefault(s => s.IdentityCode == teacher.IdentityCode);
                if (target != null)
                {

                
                    target.Enable = teacher.Enable;
                    target.Gender = teacher.Gender;
                    target.Mobile = teacher.Mobile;
                    target.RealName = teacher.RealName;
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
            var target = ctx.Teachers.FirstOrDefault(s => s.IdentityCode == id);
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
                ctx.Teachers.Remove(target);
                try
                {
                    ctx.SaveChanges();
                    return new ResponseBase { success = true, msg = "删除成功" };
                }
                catch (Exception)
                {
                    return new ErrorResponse
                    {
                        msg = "教师有相关数据，无法删除",
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
            var target = ctx.Teachers.FirstOrDefault(s => s.IdentityCode == id);
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
                return new ResponseBase { success = true, msg = "密码重置成功，教工号【" + target.IdentityCode + "】，密码【" + password + "】" };


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
            var target = ctx.Teachers.FirstOrDefault(s => EF.Functions.ILike(s.IdentityCode, code) && s.IdentityCode != skip);
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