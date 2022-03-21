using CryptoHelper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Timetable.DbContext;
using Timetable.DbContext.Models;
using Timetable.Tools;
using Timetable.Web.CommonViewModel;
using Timetable.Web.OA.Infrastructure.Authorization;
using Timetable.Web.OA.Infrastructure.PermissionManager;
using Timetable.Web.OA.ViewModel;

namespace Timetable.Web.OA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Permission]
    public class EmployeeController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public EmployeeController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost("[action]")]
        public PagerResponse List(PagerRequest req)
        {
            using (var ctx = new TimeTableDbContext())
            {



                IQueryable<Employee> query = ctx.Employees;
                if (!string.IsNullOrEmpty(req.keyword))
                    query = query.Where(s => EF.Functions.ILike(s.LoginName, "%" + req.keyword + "%") || EF.Functions.ILike(s.Email, "%" + req.keyword + "%") || EF.Functions.ILike(s.RealName, "%" + req.keyword + "%"));
                var total = query.Count();
                return (new PagerResponse
                {
                    total = total,
                    data = query.OrderByDescending(s => s.CreateTime).Skip((req.page - 1) * req.rows).Take(req.rows).Select(s => new
                    {
                        s.Address,
                        s.CreateTime,
                        Creator = s.Creator.RealName,
                        s.Email,
                        s.Enable,
                        s.Headshot,
                        s.Id,
                        s.LoginName,
                        s.PermissionEndTime,
                        s.Mobile,
                        s.RealName,
                        EmployeeRoleIds = s.EmployeeRoles.Select(t => t.RoleId).ToList(),
                        EmployeeRoleNames = s.EmployeeRoles.Select(t => t.Role.Name).ToList()
                    }).ToList()
                });
            }
        }

        /// <summary>
        ///     添加管理用户
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public ResponseBase Add(EmployeeWithRoleIds user)
        {
            var pass = RandomHelper.CreateMixVerifyCode(8);
            var ctx = new TimeTableDbContext();
            user.Creator = null;
            user.Articles = null;
            user.EmployeeRoles = null;
            user.Headshot = "/upload/default/default.jpg";
            user.CreateTime = DateTime.Now;
            user.CreatorId = Convert.ToInt64(User.Identity.Name);
            user.Password = Crypto.HashPassword(user.LoginName + pass);
            user.EmployeeRoles = new List<EmployeeRole>();


            if (user.RoleIds != null)
            {
                foreach (var id in user.RoleIds)
                {
                    user.EmployeeRoles.Add(new EmployeeRole() { RoleId = id });
                }
            }
            try
            {
                ctx.Employees.Add(user);
                ctx.SaveChanges();
                PermissionCache.UpdateUserRoles();
                return (new ResponseBase
                {
                    success = true,
                    msg = "添加成功，管理账号登录名：" + user.LoginName + "，默认密码：" + pass + "。请用户登录后及时修改密码。"
                });
            }
            catch (Exception)
            {
                return (new ErrorResponse
                {
                    success = false,
                    errCode = "ERR_UNIQUE",
                    msg = "添加失败，检查账号是否重复"
                });

            }





        }
        [HttpPost("edit")]
        public ResponseBase Edit(EmployeeWithRoleIds user)
        {
            var ctx = new TimeTableDbContext();
            var targetUser = ctx.Employees.FirstOrDefault(s => s.Id == user.Id);
            if (targetUser != null)
            {

                targetUser.CreatorId = Convert.ToInt64(User.Identity.Name);
                targetUser.Address = user.Address;
                targetUser.Mobile = user.Mobile;
                targetUser.Email = user.Email;
                targetUser.PermissionEndTime = user.PermissionEndTime;
                targetUser.RealName = user.RealName;
                ctx.EmployeeRoles.RemoveRange(targetUser.EmployeeRoles.ToList());

                if (user.RoleIds != null)
                {
                    foreach (var id in user.RoleIds)
                    {
                        targetUser.EmployeeRoles.Add(new EmployeeRole() { RoleId = id });
                    }
                }

                targetUser.Enable = user.Enable;
                ctx.SaveChanges();
                PermissionCache.UpdateUserRoles();
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

        [HttpPost("[action]")]
        public ResponseBase ResetPassword([FromForm]long id)
        {
            using (var ctx = new TimeTableDbContext())
            {
                var pass = RandomHelper.CreateMixVerifyCode(8);
                var targetUser = ctx.Employees.FirstOrDefault(s => s.Id == id);
                if (targetUser != null)
                {

                    targetUser.Password = Crypto.HashPassword(targetUser.LoginName + pass);
                    ctx.SaveChanges();
                    return (new ResponseBase
                    {
                        success = true,
                        msg = "密码已重置为" + pass
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
        public ResponseBase ChangePassword([FromForm(Name = "old")]string old, [FromForm(Name = "pass")] string pass)
        {
            var ctx = new TimeTableDbContext();
            var eid = Convert.ToInt32(User.Identity.Name);
            var emp = ctx.Employees.FirstOrDefault(s => s.Id == eid);
            if (Crypto.VerifyHashedPassword(emp.Password, emp.LoginName + old))
            {
                emp.Password = Crypto.HashPassword(emp.LoginName + pass);
                ctx.SaveChanges();
                return (new ResponseBase
                {
                    success = true,
                    msg = "修改成功，下次请使用新密码登录"
                });
            }
            else
            {
                return (new ErrorResponse
                {
                    success = false,
                    errCode = "ERR_PASSWORD",
                    msg = "原密码有误"
                });
            }
        }

        [HttpPost("[action]")]
        public ResponseBase Delete([FromForm]long id)
        {
            var ctx = new TimeTableDbContext();
            var targetUser = ctx.Employees.FirstOrDefault(s => s.Id == id);
            if (targetUser != null)
            {
                try
                {
                    ctx.EmployeeRoles.RemoveRange(targetUser.EmployeeRoles);
                    ctx.Employees.Remove(targetUser);
                    ctx.SaveChanges();
                    PermissionCache.UpdateUserRoles();
                    return (new ResponseBase
                    {
                        success = true,
                        msg = "删除成功"
                    });
                }
                catch (Exception)
                {
                    return (new ErrorResponse
                    {
                        success = false,
                        errCode = "ERR_FOREIGN_KEY",
                        msg = "操作失败，用户有工作数据，为了保证数据完整性，禁止删除，请禁用该用户。"
                    });
                }
            }
            else
            {
                return (new ErrorResponse
                {
                    success = false,
                    errCode = "ERR_NO_FOUND",
                    msg = "操作失败，未找到用户。"
                });
            }
        }
        [HttpPost("[action]")]
        public bool CheckLoginName([FromForm]string loginname)
        {
            var ctx = new TimeTableDbContext();
            return ctx.Employees.Count(s => EF.Functions.ILike(s.LoginName, loginname)) == 0;
        }


        /// <summary>
        ///  上传图片
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [RequestSizeLimit(uint.MaxValue)]

        public async Task<ResponseBase> UploadHeadshot(IFormFile file)
        {
            var folderName = DateTime.Now.ToString("yyyy-MM");

            var basePath = _hostingEnvironment.WebRootPath + @"\upload\";
            var fullPath = basePath + folderName;
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
            var ext = Path.GetExtension(file.FileName);
            var filename = DateTime.Now.ToString("ddHHmmssfff") + Guid.NewGuid().ToString().Split("-")[0];

            using (var stream = file.OpenReadStream())
            {
                ImageFormat imageFormat = ImageWorker.ValidateGetImageFormat(file.FileName, stream);
                if (imageFormat == null)
                {
                    return new ErrorResponse()
                    {
                        success = false,
                        msg = "只允许上传.png , .jpg , .jpeg , .bmp , .gif后缀的图片文件，如您的文件是上述后缀名，说明文件后缀和文件真实类型不符。",
                        errCode = "ERR_FILETYPE_INVALID"
                    };
                }
                await Task.Run(() =>
                {
                    ImageWorker.MakeThumbnail(stream, Path.Join(basePath, folderName, filename + ext), 200, 200, "HW", imageFormat);
                });
            }
            return new ResponseBase()
            {
                data = "/upload/" + folderName + "/" + filename + ext,
                msg = "上传成功",
                success = true
            };
        }


        /// <summary>
        ///     更新自己的用户信息，后台只处理Email，Headshot，Mobile、Address
        /// </summary>
        /// <returns></returns>
        [HttpPost("[action]")]
        public ResponseBase UpdateEmployeeInfo(Employee employee)
        {

            var eid = Convert.ToInt64(User.Identity.Name);
            var ctx = new TimeTableDbContext();
            var emp = ctx.Employees.FirstOrDefault(s => s.Id == eid);
            emp.Email = employee.Email;
            emp.Headshot = employee.Headshot;
            emp.Mobile = employee.Mobile;
            emp.Address = employee.Address;
            ctx.SaveChanges();
            return new ResponseBase()
            {
                success = true,
                msg = "用户信息修改成功"
            };
        }

        [HttpPost("[action]")]
        public ResponseBase GetEmployeeInfo()
        {
            var ctx = new TimeTableDbContext();
            var id = Convert.ToInt64(User.Identity.Name);
            var emp = ctx.Employees.FirstOrDefault(s => s.Id == id);
            return new ResponseBase
            {
                success = true,
                data = new
                {
                    emp.Headshot,
                    emp.Mobile,
                    emp.Email,
                    emp.Address,
                    emp.RealName,
                    emp.CreateTime,
                    Roles = emp.EmployeeRoles.Select(t => t.Role.Name).ToList()
                }
            };
        }
    }
}