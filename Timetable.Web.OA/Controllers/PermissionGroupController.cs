using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Timetable.Web.OA.Infrastructure.Authorization;
using Timetable.Web.OA.Infrastructure.PermissionManager;
using System;
using System.Collections.Generic;
using System.Linq;
using Timetable.Web.CommonViewModel;
using Timetable.DbContext;
using Timetable.DbContext.Models;
using Timetable.Web.OA.ViewModel;

namespace Timetable.Web.OA.Controllers
{
    [Permission]
    [ApiController]
    [Route("/api/[controller]")]
    public class PermissionGroupController : ControllerBase
    {
        [HttpPost("[action]")]
        public ResponseBase List(PagerRequest req)
        {
            using (var ctx = new TimeTableDbContext()) {
                var allGroups = ctx.PermissionGroups.ToList();
            IQueryable<PermissionGroup> query = ctx.PermissionGroups;
            if (!string.IsNullOrEmpty(req.keyword))
            {
                query = query.Where(s => s.DisplayName.Contains(req.keyword) || s.Headshot.Contains(req.keyword) || s.Key.Contains(req.keyword) || s.Url.Contains(req.keyword));
            }
            List<PermissionGroup> neededGroups = query.ToList();
            //找孩子
            List<long?> allParentIds = neededGroups.Select(s => (long?)s.Id).ToList();
            bool needFindChildren = allParentIds.Count > 0;
            while (needFindChildren)
            {
                List<PermissionGroup> children = allGroups.Where(s => allParentIds.Contains(s.ParentId)).ToList();
                needFindChildren = false;//假定为不需要继续查找，一旦 添加了任何一个项目到neededGroups，立即设为true
                foreach (var item in children)
                {
                    if (neededGroups.FirstOrDefault(s => s.Id == item.Id) == null)
                    {
                        neededGroups.Add(item);
                        needFindChildren = true;
                    }
                }
                if (needFindChildren) allParentIds = neededGroups.Select(s => (long?)s.Id).ToList();//需要继续查找，则还要更新下allParentIds
            }
            //找父亲
            allParentIds = neededGroups.Where(s => s.ParentId.HasValue).Select(s => (long?)s.ParentId).Distinct().ToList();
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
            List<PermissionGroupTreeViewModel> tree = new List<PermissionGroupTreeViewModel>();
            var levelModels = neededGroups.Where(s => s.ParentId == null).ToList();
            foreach (var model in levelModels)
            {
                PermissionGroupTreeViewModel nodeView = new PermissionGroupTreeViewModel
                {
                    DisplayName = model.DisplayName,
                    ParentId = model.ParentId,
                    Headshot = model.Headshot,
                    Id = model.Id,
                    SN = model.SN,
                    Key = model.Key,
                    Url = model.Url
                };
                tree.Add(nodeView);
                _LoopGetChildren(neededGroups, nodeView);

            }
            return new ResponseBase()
            {
                msg = "请求成功",
                data = tree,
                success = true
            };
            }
        }
        private void _LoopGetChildren(List<PermissionGroup> neededGroups, PermissionGroupTreeViewModel parentNodeView)
        {
            var levelModels = neededGroups.Where(s => s.ParentId == parentNodeView.Id).ToList();
            foreach (var model in levelModels)
            {
                PermissionGroupTreeViewModel nodeView = new PermissionGroupTreeViewModel
                {
                    DisplayName = model.DisplayName,
                    ParentId = model.ParentId,
                    Headshot = model.Headshot,
                    Id = model.Id,
                    SN = model.SN,
                    Key = model.Key,
                    Url = model.Url
                };
                parentNodeView.children.Add(nodeView);
                _LoopGetChildren(neededGroups, nodeView);
            }
        }

        [HttpPost("[action]")]
        public ResponseBase Add(PermissionGroup group)
        {
            var ctx = new TimeTableDbContext();
            //清理可能引发意外注入的数据
            group.Children = null;
            group.Parent = null;
            group.PermissionLines = null;
            if (string.IsNullOrWhiteSpace(group.Key)) group.Key = null;
            //开始处理添加
           
            if (ctx.PermissionGroups.Count(p => p.Id == group.ParentId) > 0)
            {
                if (group.ParentId == 0)
                {
                    group.ParentId = null;
                }
            }
            else
            {
                group.ParentId = null;
            }

            ctx.PermissionGroups.Add(group);
            try
            {
                ctx.SaveChanges();
                PermissionCache.UpdateUserRoles();
                PermissionCache.UpdateLines();
                PermissionCache.UpdateGroups();
                return new ResponseBase()
                {
                    success = true,
                    msg = "操作成功"
                };

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

        [HttpPost("[action]")]
        public ResponseBase Edit(PermissionGroup group)
        {
            if (string.IsNullOrWhiteSpace(group.Key)) group.Key = null;
            var ctx = new TimeTableDbContext();
            var target = ctx.PermissionGroups.FirstOrDefault(s => s.Id == group.Id);
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
                target.DisplayName = group.DisplayName;
                target.Key = group.Key;
                target.Url = group.Url;
                target.SN = group.SN;
                target.Headshot = group.Headshot;
                if (ctx.PermissionGroups.Count(p => p.Id == group.ParentId) > 0 && target.Id != group.ParentId)
                {
                    if (group.ParentId == 0)
                    {
                        target.ParentId = null;
                    }
                    else
                    {
                        target.ParentId = group.ParentId;
                    }
                }
                else
                {
                    target.ParentId = null;
                }
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
        }

        [HttpPost("[action]")]
        public ResponseBase Delete([FromForm]long id)
        {
            var ctx = new TimeTableDbContext();
            var target = ctx.PermissionGroups.FirstOrDefault(s => s.Id == id);
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
                var lines = target.PermissionLines.ToList();
                foreach (var line in lines)
                {
                    ctx.RolePermissions.RemoveRange(line.RolePermissions.ToList());
                }
                ctx.PermissionLines.RemoveRange(lines);
                ctx.PermissionGroups.Remove(target);
                try
                {
                    ctx.SaveChanges();
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
                        errCode="ERR_FOREIGN_KEY",
                        msg = "操作失败，权限页下有子权限页，请先删除子权限页。"
                    });
                   
                }

            }
        }
    }
}
