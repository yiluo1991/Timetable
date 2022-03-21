using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using Timetable.Web.CommonViewModel;
using Timetable.DbContext;
using Timetable.Web.OA.Infrastructure.PermissionManager;
using Timetable.Web.OA.Infrastructure;
using Timetable.Web.OA.Infrastructure.PermissionManager.Models;
using Timetable.Web.OA.Infrastructure.Authorization;

namespace Timetable.Web.OA.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AuthController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("getPermissionMenu")]
        public ResponseBase getPermissionMenu()
        {
            using (var ctx = new TimeTableDbContext())
            {
                if (User.Identity.IsAuthenticated)
                {
                    long id = Convert.ToInt64(User.Identity.Name);
                    if (PermissionCache.UserRoles.ContainsKey(id))
                    {


                        var claims = User.Claims;
                        var hashClaim = claims.FirstOrDefault(s => s.Type == ClaimTypes.Hash);
                        if (hashClaim != null)
                        {
                            if (hashClaim.Value == SinglePointManager.GetSinglePointHash(id))
                            {
                                List<long> roles = PermissionCache.UserRoles[id];
                                List<PermissionGroupCache> PermissionGroups = PermissionCache.Groups.Where(s => !(s.Tag != null && s.Tag.StartsWith("hidden")) && s.RoleIds.Count(t => roles.Contains(t)) > 0).OrderBy(s => s.SN).ToList();

                                return new ResponseBase
                                {
                                    success = true,
                                    data = new
                                    {
                                        logined = true,
                                        menu = PermissionGroups.Where(s => s.ParentId == null).Select(s => new
                                        {
                                            s.Url,
                                            Name = s.DisplayName,
                                            Icon = s.Headshot,
                                            s.Id,
                                            Children = PermissionGroups.Where(t => t.ParentId == s.Id).Select(t => new
                                            {
                                                t.Url,
                                                Name = t.DisplayName,
                                                Icon = t.Headshot,
                                                t.Id
                                            }).ToList()
                                        }).ToList(),
                                        pages = PermissionGroups.Where(s => !string.IsNullOrEmpty(s.Url)).Select(s => s.Url).ToList(),
                                        permissionline = PermissionCache.Lines.Where(s => !string.IsNullOrEmpty(s.Tag) && s.Roles.Count(t => roles.Contains(t)) > 0).Select(s => s.Tag).ToList()
                                    },
                                    msg = "请求成功"
                                };
                            }
                            else
                            {
                                return new ErrorResponse
                                {
                                    success = false,
                                    errCode = "ERR_NO_AUTH",
                                    msg = "您的账号已在其他位置登录"
                                };
                            }
                        }
                        else
                        {
                            return new ErrorResponse
                            {
                                success = false,
                                errCode = "ERR_NO_AUTH",
                                msg = "令牌版本不正确，请重新登录"
                            };
                        }

                    }
                    else
                    {
                        return new ErrorResponse
                        {
                            success = false,
                            errCode = "ERR_NO_AUTH",
                            msg = "账户已禁用或不存在"
                        };
                    }
                }
                else
                {
                    return new ErrorResponse
                    {
                        success = false,
                        errCode = "ERR_NO_AUTH",
                        msg = "未登录"
                    };
                }

            }
        }

 
    }
}