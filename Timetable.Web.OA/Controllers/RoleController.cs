using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Timetable.DbContext;
using Timetable.DbContext.Models;
using Timetable.Web.CommonViewModel;
using Timetable.Web.OA.Infrastructure.Authorization;
using Timetable.Web.OA.Infrastructure.PermissionManager;
using Timetable.Web.OA.ViewModel;

namespace Timetable.Web.OA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Permission]
    public class RoleController : ControllerBase
    {
        [HttpPost("[action]")]
        public ResponseBase List(PagerRequest req)
        {
            using (var ctx = new TimeTableDbContext())
            {


                bool simple = string.IsNullOrWhiteSpace(req.extend);
                var data = ctx.Roles.Where(s => s.Name.Contains(req.keyword) || s.Remark.Contains(req.keyword)).Select(s => new
                {
                    s.Id,
                    s.Name,
                    s.Remark,
                    LineIds = s.RolePermissions.Where(s => simple).Select(t => t.PermissionLineId).ToList()
                }).ToList();
                return new ResponseBase()
                {
                    data = data,
                    msg = "请求成功",
                    success = true
                };
            }
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="permissionlineIds"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public ResponseBase Add(RoleWithLineIds role)
        {
            var ctx = new TimeTableDbContext();
            role.EmployeeRoles = null;
            role.RolePermissions = new List<RolePermission>();
            ctx.Roles.Add(role);
            if (role.LineIds != null && role.LineIds.Count > 0)
            {

                foreach (long id in role.LineIds)
                {
                    role.RolePermissions.Add(new RolePermission() { PermissionLineId = id });
                }

            }
            try
            {
                ctx.SaveChanges();
                return new ResponseBase()
                {
                    msg = "角色添加成功",
                    success = true
                };
            }
            catch (Exception)
            {

                return (new ErrorResponse
                {
                    success = false,
                    errCode = "ERR_NO_FOUND",
                    msg = "操作失败，可能权限数据有变更，请刷新页面重试"
                });
            }
        }

        [HttpPost("[action]")]
        public ResponseBase Edit(RoleWithLineIds role)
        {
            using (var ctx = new TimeTableDbContext())
            {


                Role targetRole = ctx.Roles.Where(p => p.Id == role.Id).FirstOrDefault();
                if (targetRole != null)
                {
                    targetRole.Name = role.Name;
                    targetRole.Remark = role.Remark;
                    var oldRolePermissions = ctx.RolePermissions.Where(p => p.RoleId == targetRole.Id);
                    ctx.RolePermissions.RemoveRange(oldRolePermissions);

                    if (role.LineIds != null && role.LineIds.Count > 0)
                    {
                        List<RolePermission> rolePsermissionList = new List<RolePermission>();
                        foreach (Int32 pid in role.LineIds)
                        {
                            rolePsermissionList.Add(new RolePermission() { PermissionLineId = pid, RoleId = role.Id });
                        }
                        ctx.RolePermissions.AddRange(rolePsermissionList);
                    }

                    try
                    {
                        ctx.SaveChanges();
                        PermissionCache.UpdateUserRoles();
                        PermissionCache.UpdateLines();
                        PermissionCache.UpdateGroups();
                    }
                    catch (Exception)
                    {

                        return (new ErrorResponse
                        {
                            success = false,
                            errCode = "ERR_NO_FOUND",
                            msg = "操作失败，可能权限数据有变更，请刷新页面重试"
                        });
                    }

                    return new ResponseBase()
                    {
                        msg = "修改成功，角色的系统权限已更新",
                        success = true
                    };
                }
                else
                {
                    return (new ErrorResponse
                    {
                        success = false,
                        errCode = "ERR_NO_FOUND",
                        msg = "没有找到要操作的角色数据"
                    });
                }
            }

        }

        /// <summary>
        ///     移除角色
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [HttpPost("[action]")]
        public ResponseBase Delete([FromForm]long id)
        {
            var ctx = new TimeTableDbContext();
            Role removeItem = ctx.Roles.FirstOrDefault(s => s.Id == id);
            if (removeItem == null)
            {

                return (new ErrorResponse
                {
                    success = false,
                    errCode = "ERR_NO_FOUND",
                    msg = "删除失败，没有找到要删除的角色数据"
                });
            }
            else
            {
                try
                {
                    ctx.Roles.Remove(removeItem);
                    ctx.SaveChanges();
                    PermissionCache.UpdateUserRoles();
                    PermissionCache.UpdateLines();
                    PermissionCache.UpdateGroups();
                    return new ResponseBase()
                    {
                        msg = "删除成功，角色的系统权限已更新",
                        success = true
                    };
                }
                catch (Exception ex)
                {
                    return (new ErrorResponse
                    {
                        success = false,
                        errCode = "ERR_FOREIGN_KEY",
                        msg = "删除失败，权限正被用户使用，为了系统安全，请先手动移除用户对该角色的使用。"
                    });
                }
            }

        }
    }
}