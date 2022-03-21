using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Timetable.Web.OA.Infrastructure.PermissionManager;
using Timetable.Web.OA.Infrastructure.PermissionManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Timetable.Web.CommonViewModel;

namespace Timetable.Web.OA.Infrastructure.Authorization
{
    public class PermissionAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var uid = Convert.ToInt64(context.HttpContext.User.Identity.Name);
            if (PermissionCache.UserRoles.ContainsKey(uid))
            {
                if (!PermissionCache.BandTimes.ContainsKey(uid) || PermissionCache.BandTimes[uid] > DateTime.Now)
                {
                    var claims = context.HttpContext.User.Claims;
                    var hashClaim = claims.FirstOrDefault(s => s.Type == ClaimTypes.Hash);
                    if (hashClaim != null)
                    {
                        if(hashClaim.Value== SinglePointManager.GetSinglePointHash(uid))
                        {
                            List<long> roles = PermissionCache.UserRoles[uid];
                            List<PermissionGroupCache> PermissionGroups = PermissionCache.Groups.Where(s => s.Tag != ("hidden") && s.RoleIds.Count(t => roles.Contains(t)) > 0).OrderBy(s => s.SN).ToList();
                            var controllerName = context.ActionDescriptor.RouteValues["controller"];
                            var actionName = context.ActionDescriptor.RouteValues["action"];
                            var url = "/api/" + controllerName.ToLower() + "/" + actionName.ToLower();
                            var line = PermissionCache.Lines.FirstOrDefault(s => s.Url == url);
                            if (line == null)
                            {
                                context.Result = new ContentResult()
                                {
                                    Content = JsonConvert.SerializeObject(new ErrorResponse
                                    {
                                        success = false,
                                        errCode = "ERR_AUTH_INVALID",
                                        msg = "您无权访问接口:" + url
                                    }),
                                    ContentType = "application/json; charset=utf-8",
                                    StatusCode = 403
                                };
                            }
                            else
                            {
                                if (line.Roles.FindIndex(t => roles.Contains(t)) == -1)
                                {
                                    context.Result = new ContentResult()
                                    {
                                        Content = JsonConvert.SerializeObject(new ErrorResponse
                                        {
                                            success = false,
                                            errCode = "ERR_AUTH_INVALID",
                                            msg = "您无权访问接口:" + url
                                        }),
                                        ContentType = "application/json; charset=utf-8",
                                        StatusCode = 403
                                    };
                                }
                            }
                        }
                        else
                        {
                            context.Result = new ContentResult()
                            {
                                Content = JsonConvert.SerializeObject(new ErrorResponse
                                {
                                    success = false,
                                    errCode = "ERR_AUTH_INVALID",
                                    msg = "您的账号在其他地点登录，请重新登录"
                                }),
                                ContentType = "application/json; charset=utf-8",
                                StatusCode = 403
                            };
                        } 
                    }
                    else
                    {
                        context.Result = new ContentResult()
                        {
                            Content = JsonConvert.SerializeObject(new ErrorResponse
                            {
                                success = false,
                                errCode = "ERR_AUTH_INVALID",
                                msg = "您的登录凭证版本已过期，请重新登录"
                            }),
                            ContentType = "application/json; charset=utf-8",
                            StatusCode = 403
                        };
                    }  
                }
                else
                {
                    lock (PermissionLocks.USERROLE_LOCK)
                    {
                        PermissionCache.UpdateUserRoles();
                    }

                    context.Result = new ContentResult()
                    {
                        Content = JsonConvert.SerializeObject(new ErrorResponse
                        {
                            success = false,
                            errCode = "ERR_AUTH_INVALID",
                            msg = "用户不存在或已被禁用/已过期"
                        }),
                        ContentType = "application/json; charset=utf-8",
                        StatusCode = 403
                    };
                }
            }
            else
            {
                context.Result = new ContentResult()
                {
                    Content = JsonConvert.SerializeObject(new ErrorResponse
                    {
                        success = false,
                        errCode = "ERR_AUTH_INVALID",
                        msg = "用户不存在或已被禁用/已过期"
                    }),
                    ContentType = "application/json; charset=utf-8",
                    StatusCode = 403
                };
            }

        }
    }
}



