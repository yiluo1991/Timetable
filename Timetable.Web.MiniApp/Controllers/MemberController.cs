using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CryptoHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Timetable.DbContext;
using Timetable.Web.CommonViewModel;
using Timetable.Web.MiniApp.Infrastructure.Authorization;
using Timetable.Web.MiniApp.WeChatModel;

namespace Timetable.Web.MiniApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        [Token]
        [HttpGet]
        public ResponseBase Get()
        {
            var username = User.Identity.Name.Substring(2);
            var type = User.FindFirst(ClaimTypes.Role).Value;
            using (var ctx = new TimeTableDbContext())
            {
                if (type == "T")
                {
                    var teacher = ctx.Teachers.FirstOrDefault(s => s.IdentityCode == username);
                    if (teacher != null)
                    {
                        return new ResponseBase
                        {
                            success = true,
                            data = new
                            {
                                Type = "T",
                                teacher.Gender,
                                teacher.IdentityCode,
                                teacher.Mobile,
                                teacher.RealName,
                                teacher.AvatarUrl
                            },
                            msg = "获取成功"
                        };
                    }
                }
                else
                {
                    var student = ctx.Students.FirstOrDefault(s => s.IdentityCode == username);
                    if (student != null)
                    {
                        return new ResponseBase
                        {
                            success = true,
                            data = new
                            {
                                Type = "S",
                                student.Gender,
                                student.IdentityCode,
                                student.Mobile,
                                student.RealName,
                                student.AvatarUrl
                            },
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


        [Token(ValidateDatabase = true)]
        [HttpPost("[action]")]
        public ResponseBase Password(PasswordViewModel vm)
        {
            var username = User.Identity.Name.Substring(2);
            var type = User.FindFirst(ClaimTypes.Role).Value;
            using (var ctx = new TimeTableDbContext())
            {
                if (type == "T")
                {
                    var teacher = ctx.Teachers.FirstOrDefault(s => s.IdentityCode == username);
                    if (teacher != null)
                    {
                        if (Crypto.VerifyHashedPassword(teacher.Password, teacher.IdentityCode + vm.Old))
                        {
                            teacher.Password = Crypto.HashPassword(teacher.IdentityCode + vm.New);
                            ctx.SaveChanges();
                            return new ResponseBase
                            {
                                success = true,

                                msg = "修改成功"
                            };
                        }
                        else
                        {
                            return new ErrorResponse
                            {
                                success = false,
                                errCode = "ERR_PASSWORD_INVALID",
                                msg = "原密码有误"
                            };
                        }
                    }
                }
                else
                {
                    var student = ctx.Students.FirstOrDefault(s => s.IdentityCode == username);
                    if (student != null)
                    {
                        if (Crypto.VerifyHashedPassword(student.Password, student.IdentityCode + vm.Old))
                        {
                            student.Password = Crypto.HashPassword(student.IdentityCode + vm.New);
                            ctx.SaveChanges();
                            return new ResponseBase
                            {
                                success = true,
                                msg = "修改成功"
                            };
                        }
                        else
                        {
                            return new ErrorResponse
                            {
                                success = false,
                                errCode = "ERR_PASSWORD_INVALID",
                                msg = "原密码有误"
                            };
                        }
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
    }
}