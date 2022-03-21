using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CryptoHelper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Timetable.DbContext;
using Timetable.DbContext.Models;
using Timetable.Tools;
using Timetable.Web.CommonViewModel;
using Timetable.Web.OA.Infrastructure;

namespace Timetable.Web.OA.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        [AllowAnonymous]
        [HttpPost("userlogin")]
        public ResponseBase userlogin([FromBody]LoginRequest req)
        {

            if (LoginLock.CanLogin(req.UserName))
            {
                //下面的变量claims是Claim类型的数组，Claim是string类型的键值对，所以claims数组中可以存储任意个和用户有关的信息，
                //不过要注意这些信息都是加密后存储在客户端浏览器cookie中的，所以最好不要存储太多特别敏感的信息，这里我们只存储了用户名到claims数组,
                //表示当前登录的用户是谁

                using (var ctx = new TimeTableDbContext())
                {

                    var emp = ctx.Employees.Where(s => s.LoginName == req.UserName).FirstOrDefault();
                    if (emp != null)
                    {
                        if (emp.Enable)
                        {
                            var ip = "";
                            try
                            {
                                 ip = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                                if (string.IsNullOrEmpty(ip))
                                {
                                    ip = HttpContext.Request.Headers["x-forwarded-for"].FirstOrDefault();
                                }
                                if (!string.IsNullOrEmpty(ip))
                                {
                                    ip = ip.Split(',')[ip.Split(',').Length - 1].Trim().Split(':')[0];
                                }
                                if (string.IsNullOrEmpty(ip))
                                {
                                    ip = HttpContext.Connection.RemoteIpAddress.ToString();
                                }
                            }
                            catch (Exception)
                            {
                            }
                            if (Crypto.VerifyHashedPassword(emp.Password, emp.LoginName + req.Password))
                            {
                                LoginLock.MemberLoginSuccess(emp.LoginName);
                                var claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                                claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, emp.Id.ToString()));
                                var hash =RandomHelper.CreateMixVerifyCode(16);
                                claimsIdentity.AddClaim(new Claim(ClaimTypes.Hash, hash));
                                claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, emp.Id.ToString()));
                                ClaimsPrincipal user = new ClaimsPrincipal(claimsIdentity);
                                Task.Run(async () =>
                                {
                                    //登录用户，相当于ASP.NET中的FormsAuthentication.SetAuthCookie
                                    //  await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user);

                                    //可以使用HttpContext.SignInAsync方法的重载来定义持久化cookie存储用户认证信息，例如下面的代码就定义了用户登录后60分钟内cookie都会保留在客户端计算机硬盘上，
                                    //即便用户关闭了浏览器，60分钟内再次访问站点仍然是处于登录状态，除非调用Logout方法注销登录。
                                    //注意其中的AllowRefresh属性，如果AllowRefresh为true，表示如果用户登录后在超过50%的ExpiresUtc时间间隔内又访问了站点，就延长用户的登录时间（其实就是延长cookie在客户端计算机硬盘上的保留时间），
                                    //例如本例中我们下面设置了ExpiresUtc属性为60分钟后，那么当用户登录后在大于30分钟且小于60分钟内访问了站点，那么就将用户登录状态再延长到当前时间后的60分钟。但是用户在登录后的30分钟内访问站点是不会延长登录时间的，
                                    //因为ASP.NET Core有个硬性要求，是用户在超过50%的ExpiresUtc时间间隔内又访问了站点，才延长用户的登录时间。
                                    //如果AllowRefresh为false，表示用户登录后60分钟内不管有没有访问站点，只要60分钟到了，立马就处于非登录状态（不延长cookie在客户端计算机硬盘上的保留时间，60分钟到了客户端计算机就自动删除cookie）

                                    await HttpContext.SignInAsync(
                                    CookieAuthenticationDefaults.AuthenticationScheme,
                                    user, new AuthenticationProperties()
                                    {
                                        IsPersistent = false
                                    });
                                }).Wait();
                                //设置单点登录Hash
                                SinglePointManager.UpdateSinglePointHash(emp.Id, hash);
                                ctx.EmployeeLoginLogs.Add(new EmployeeLoginLog { IP = ip, Success = true, EmployeeId = emp.Id, LoginTime = DateTime.Now, Remark = string.Format("ID：{0}，用户名：{1}，登录系统", emp.Id.ToString(), emp.LoginName) });
                                ctx.SaveChanges();
                                return new ResponseBase { success = true, msg = "登录成功", };
                            }
                            else
                            {
                                bool needlog;
                                int times;
                                if (LoginLock.MemberPasswordError(emp.LoginName, out needlog, out times))
                                {


                                    if (needlog)
                                    {
                                        ctx.EmployeeLoginLogs.Add(new EmployeeLoginLog { IP = ip, EmployeeId = emp.Id, LoginTime = DateTime.Now, Remark = string.Format("ID：{0}，用户名：{1}，登录失败次数过多，被锁定5分钟，当前第{2}次失败。", emp.Id.ToString(), emp.LoginName, times) });
                                        ctx.SaveChanges();
                                    }
                                    return new ErrorResponse
                                    {
                                        success = false,
                                        errCode = "ERR_LOGIN_LIMIT",
                                        msg = "连续尝试密码错误超过5次，请5分钟后再尝试登录",
                                    };
                                }
                                else
                                {
                                    return new ResponseBase
                                    {
                                        success = false,
                                        msg = "用户名或密码有误",
                                    };
                                }

                            }
                        }
                        else
                        {
                            return new ErrorResponse
                            {
                                success = false,
                                errCode = "ERR_ACCOUNT_DISABLED",
                                msg = "用户已禁用",
                            };
                        }



                    }
                    else
                    {
                        bool needlog;
                        int times;
                        //没有找到用户，也要拟真处理保证登录锁结果一致
                        if (LoginLock.MemberPasswordError(req.UserName, out needlog, out times))
                        {
                            return new ErrorResponse
                            {
                                success = false,
                                errCode = "ERR_LOGIN_LIMIT",
                                msg = "连续尝试密码错误超过5次，请5分钟后再尝试登录",
                            };
                        }
                        else
                        {
                            return new ResponseBase
                            {
                                success = false,
                                msg = "用户名或密码有误",
                            };
                        }


                    }
                }
            }
            else
            {
                return new ErrorResponse
                {
                    success = false,
                    errCode = "ERR_LOGIN_LIMIT",
                    msg = "连续尝试密码错误超过5次，请5分钟后再尝试登录",
                };
            }



        }



        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<ResponseBase> LogOut()
        {
            if (User.Identity.IsAuthenticated)
            {
                var id = Int64.Parse(User.Identity.Name);
                SinglePointManager.UpdateSinglePointHash(id, null);
                SinglePointManager.ClearSinglePointHashToRedis(id);
             
            }
            await HttpContext.SignOutAsync();
            return new ResponseBase
            {
                success = true,
                msg = "退出成功"
            };
        }
    }
}