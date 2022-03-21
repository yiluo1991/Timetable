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

    [Permission]
    [ApiController]
    [Route("/api/[controller]")]
    public class PermissionLineController : ControllerBase
    {
        /// <summary>
        ///     分页获取权限项列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        /// 
        [HttpPost("[action]")]
        public PagerResponse List(PagerRequest req)
        {
            using (var ctx = new TimeTableDbContext())
            {

                var allGroups = ctx.PermissionGroups.ToList();
                IQueryable<PermissionLine> query = ctx.PermissionLines;
                if (!string.IsNullOrEmpty(req.keyword))
                {
                    query = query.Where(s => s.Group.DisplayName.Contains(req.keyword) | s.Group.Url.Contains(req.keyword) || s.Key.Contains(req.keyword) || s.Url.Contains(req.keyword) || s.DisplayName.Contains(req.keyword));
                }
                var total = query.Count();
                List<PermissionLine> neededLines = query.OrderBy(s => s.Group.Parent != null ? s.Group.Parent.SN : s.Group.SN).ThenBy(s => s.Group.SN).ThenBy(s => s.SN).Skip((req.page - 1) * req.rows).Take(req.rows).ToList();
                var groupIds = neededLines.Select(s => s.GroupId).Distinct().ToList();
                List<PermissionGroup> neededGroups = allGroups.Where(s => groupIds.Contains(s.Id)).ToList();


                //补全权限组父节点
                List<long?> allParentIds = neededGroups.Where(s => s.ParentId.HasValue).Select(s => (long?)s.ParentId).Distinct().ToList();
                List<long> allId = neededGroups.Select(s => s.Id).ToList();
                List<long> missingParentIds = allParentIds.Where(s => !allId.Contains(s.Value)).Select(s => s.Value).ToList();
                while (missingParentIds.Count > 0)
                {
                    List<PermissionGroup> parents = allGroups.Where(s => missingParentIds.Contains(s.Id)).ToList();
                    foreach (var parent in parents)
                    {
                        neededGroups.Add(parent);
                        allId.Add(parent.Id);
                    }
                    allParentIds = neededGroups.Where(s => s.ParentId.HasValue).Select(s => (long?)s.ParentId).Distinct().ToList();
                    missingParentIds = allParentIds.Where(s => !allId.Contains(s.Value)).Select(s => s.Value).ToList();
                }
                neededGroups.Sort((a, b) =>
                {
                    return a.SN - b.SN;
                });

                //到此，已经得到完整树形所叙节点，开始构建树形；
                List<PermissionLineTreeViewModel> tree = new List<PermissionLineTreeViewModel>();
                var levelModels = neededGroups.Where(s => s.ParentId == null).ToList();
                foreach (var model in levelModels)
                {
                    PermissionLineTreeViewModel nodeView = new PermissionLineTreeViewModel
                    {
                        DisplayName = model.DisplayName,
                        GroupId = -model.ParentId,
                        Id = -model.Id,
                        SN = model.SN,
                        Key = model.Key,
                        Url = model.Url
                    };
                    foreach (var line in neededLines.Where(s => s.GroupId == model.Id).ToList())
                    {
                        nodeView.children.Add(new PermissionLineTreeViewModel
                        {
                            DisplayName = line.DisplayName,
                            GroupId = -line.GroupId,
                            Id = line.Id,
                            SN = line.SN,
                            Key = line.Key,
                            Url = line.Url
                        });
                    }
                    tree.Add(nodeView);
                    _LoopGetChildren(neededGroups, nodeView, neededLines);
                }

                return new PagerResponse
                {
                    total = total,
                    data = tree,
                    success = true
                };
            }
        }
        /// <summary>
        ///     分页获取权限项树形数据（角色管理中使用）
        /// </summary>
        /// <returns></returns>

        [HttpPost("[action]")]
        public PagerResponse Tree()
        {
            using (var ctx = new TimeTableDbContext())
            {
                var allGroups = ctx.PermissionGroups.ToList();

                IQueryable<PermissionLine> query = ctx.PermissionLines;

                List<PermissionLine> neededLines = query.OrderBy(s => s.Group.Parent.SN).ThenBy(s => s.Group.SN).ThenBy(s => s.SN).ToList();

                var groupIds = neededLines.Select(s => s.GroupId).Distinct().ToList();

                List<PermissionGroup> neededGroups = allGroups.Where(s => groupIds.Contains(s.Id)).ToList();



                //补全权限组父节点
                List<long?> allParentIds = neededGroups.Where(s => s.ParentId.HasValue).Select(s => (long?)s.ParentId).Distinct().ToList();
                List<long> allId = neededGroups.Select(s => s.Id).ToList();
                List<long> missingParentIds = allParentIds.Where(s => !allId.Contains(s.Value)).Select(s => s.Value).ToList();
                while (missingParentIds.Count > 0)
                {
                    List<PermissionGroup> parents = allGroups.Where(s => missingParentIds.Contains(s.Id)).ToList();
                    foreach (var parent in parents)
                    {
                        neededGroups.Add(parent);
                        allId.Add(parent.Id);
                    }
                    allParentIds = neededGroups.Where(s => s.ParentId.HasValue).Select(s => (long?)s.ParentId).Distinct().ToList();
                    missingParentIds = allParentIds.Where(s => !allId.Contains(s.Value)).Select(s => s.Value).ToList();
                }
                neededGroups.Sort((a, b) =>
                {
                    return a.SN - b.SN;
                });

                //到此，已经得到完整树形所叙节点，开始构建树形；
                List<PermissionLineTreeViewModel> tree = new List<PermissionLineTreeViewModel>();
                var levelModels = neededGroups.Where(s => s.ParentId == null).ToList();
                foreach (var model in levelModels)
                {
                    PermissionLineTreeViewModel nodeView = new PermissionLineTreeViewModel
                    {
                        DisplayName = model.DisplayName,
                        GroupId = -model.ParentId,
                        Id = -model.Id,
                        SN = model.SN,
                        Key = model.Key,
                        Url = model.Url
                    };
                    foreach (var line in neededLines.Where(s => s.GroupId == model.Id).ToList())
                    {
                        nodeView.children.Add(new PermissionLineTreeViewModel
                        {
                            DisplayName = line.DisplayName,
                            GroupId = -line.GroupId,
                            Id = line.Id
                        });
                    }
                    tree.Add(nodeView);
                    _LoopGetChildren(neededGroups, nodeView, neededLines);
                }

                return new PagerResponse
                {

                    data = tree,
                    success = true
                };
            }
        }


        private void _LoopGetChildren(List<PermissionGroup> neededGroups, PermissionLineTreeViewModel parentNodeView, List<PermissionLine> neededLines)
        {
            var levelModels = neededGroups.Where(s => s.ParentId == -parentNodeView.Id).ToList();
            foreach (var model in levelModels)
            {
                PermissionLineTreeViewModel nodeView = new PermissionLineTreeViewModel
                {
                    DisplayName = model.DisplayName,
                    GroupId = -model.ParentId,
                    Id = -model.Id,
                    SN = model.SN,
                    Key = model.Key,
                    Url = model.Url
                };
                foreach (var line in neededLines.Where(s => s.GroupId == model.Id).ToList())
                {
                    nodeView.children.Add(new PermissionLineTreeViewModel
                    {
                        DisplayName = line.DisplayName,
                        GroupId = -line.GroupId,
                        Id = line.Id,
                        SN = line.SN,
                        Key = line.Key,
                        Url = line.Url
                    });
                }
                parentNodeView.children.Add(nodeView);
                _LoopGetChildren(neededGroups, nodeView, neededLines);
            }
        }

        /// <summary>
        /// 添加权限项
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public ResponseBase Add(PermissionLine line)
        {
            //删除可能被注入的数据
            line.Group = null;
            line.RolePermissions = null;
            if (string.IsNullOrWhiteSpace(line.Key)) line.Key = null;
            //添加
            var ctx = new TimeTableDbContext();
            ctx.PermissionLines.Add(line);
            try
            {
                ctx.SaveChanges();
                PermissionCache.UpdateUserRoles();
                PermissionCache.UpdateLines();
                PermissionCache.UpdateGroups();
                return (new ResponseBase
                {
                    success = true,
                    msg = "操作成功"
                });
            }
            catch (Exception)
            {
                return (new ErrorResponse
                {
                    success = false,
                    errCode = "ERR_UNIQUE",
                    msg = "标志名重复，请更换标志名"
                });
            }
        }

        /// <summary>
        ///     修改权限项
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public ResponseBase Edit(PermissionLine line)
        {
            var ctx = new TimeTableDbContext();
            var target = ctx.PermissionLines.FirstOrDefault(s => s.Id == line.Id);
            if (string.IsNullOrWhiteSpace(line.Key)) line.Key = null;
            if (target == null)
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
                target.GroupId = line.GroupId;
                target.SN = line.SN;
                target.Key = line.Key;
                target.Url = line.Url;
                target.DisplayName = line.DisplayName;
                try
                {
                    ctx.SaveChanges();
                    PermissionCache.UpdateUserRoles();
                    PermissionCache.UpdateLines();
                    PermissionCache.UpdateGroups();
                    return (new ResponseBase
                    {
                        success = true,
                        msg = "操作成功"
                    });
                }
                catch (Exception e)
                {

                    return (new ErrorResponse
                    {
                        success = false,
                        errCode = "ERR_UNIQUE",
                        msg = "标志名重复，请更换标志名"

                    });
                }

            }

        }

        /// <summary>
        ///     删除权限项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public ResponseBase Delete([FromForm]long id)
        {
            var ctx = new TimeTableDbContext();
            var target = ctx.PermissionLines.FirstOrDefault(s => s.Id == id);
            if (target == null)
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
                ctx.RolePermissions.RemoveRange(target.RolePermissions);
                ctx.PermissionLines.Remove(target);
                ctx.SaveChanges();
                PermissionCache.UpdateUserRoles();
                PermissionCache.UpdateLines();
                PermissionCache.UpdateGroups();
                return (new ResponseBase
                {
                    success = true,
                    msg = "操作成功"
                });
            }
        }
    }
}