

using Timetable.Web.OA.Infrastructure.PermissionManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Timetable.DbContext;

namespace Timetable.Web.OA.Infrastructure.PermissionManager
{
    public class PermissionCache
    {

        private static List<PermissionGroupCache> groups;
        private static List<PermissionLineCache> lines;
        private static Dictionary<long, List<long>> userRoles { get; set; }
        private static Dictionary<long,DateTime> bandTimes { get; set; } 

        /// <summary>
        ///     权限组静态数据
        /// </summary>
        public static List<PermissionGroupCache> Groups
        {
            get
            {
                if (groups == null)
                {
                    lock (PermissionLocks.GROUP_LOCK)
                    {
                        //双层验证
                        if (groups == null)
                        {
                            UpdateGroups();
                        }
                    }
                }
                return groups;
            }
        }

        /// <summary>
        ///     权限项静态数据
        /// </summary>
        public static List<PermissionLineCache> Lines
        {
            get
            {
                if (lines == null)
                {
                    lock (PermissionLocks.LINE_LOCK)
                    {
                        if (lines == null)
                        {
                            UpdateLines();
                        }
                    }
                }
                return lines;
            }
        }

        /// <summary>
        ///     用户角色静态数据
        /// </summary>
        public static Dictionary<long, List<long>> UserRoles
        {
            get
            {
                if (userRoles == null)
                {
                    lock (PermissionLocks.USERROLE_LOCK)
                    {
                        if (userRoles == null)
                        {
                            UpdateUserRoles();
                        }
                    }
                }
                return userRoles;
            }

        }


        /// <summary>
        ///     自动禁用时间
        /// </summary>
        public static Dictionary<long, DateTime> BandTimes
        {
            get
            {
                if (bandTimes == null)
                {
                    lock (PermissionLocks.USERROLE_LOCK)
                    {
                        if (bandTimes == null)
                        {
                            UpdateUserRoles();
                        }
                    }
                }
                return bandTimes;
            }
        }


 
 

        /// <summary>
        ///     更新权限组静态数据
        /// </summary>
        public static void UpdateGroups()
        {
            List<PermissionGroupCache> list = new List<PermissionGroupCache>();
            Dictionary<long, List<long>> map = new Dictionary<long, List<long>>();
            foreach (var line in Lines)
            {
                if (!map.ContainsKey(line.GroupId))
                {
                    map[line.GroupId] = new List<long>();
                }
                foreach (var roleId in line.Roles)
                {
                    if (!map[line.GroupId].Contains(roleId))
                    {
                        map[line.GroupId].Add(roleId);
                    }
                }
            }

            var usedGroupIds = Lines.Select(s => s.GroupId).Distinct().ToList();
            using (TimeTableDbContext ctx =new TimeTableDbContext() )
            {
                var usedGroups = ctx.PermissionGroups.Where(s => usedGroupIds.Contains(s.Id)).ToList();
                List<long> allGroupIds = usedGroups.Select(s => s.Id).Distinct().ToList();
                List<long> allParentIds = usedGroups.Where(s => s.ParentId != null).Select(s => s.ParentId.Value).Distinct().ToList();
                var missingGroupId = allParentIds.Where(s => !allGroupIds.Contains(s)).ToList();
                //由于权限组只有二级，所以只需要执行一次
                usedGroups.AddRange(ctx.PermissionGroups.Where(s => missingGroupId.Contains(s.Id)).ToList());

                var groupCaches = usedGroups.Select(s => new PermissionGroupCache
                {
                    Id = s.Id,
                    DisplayName = s.DisplayName,
                    Headshot = s.Headshot,
                    ParentId = s.ParentId,
                    RoleIds = new List<long>(),
                    SN = s.SN,
                    Tag = s.Key,
                    Url = s.Url==null?"":s.Url.ToLower()
                }).ToList();
                //开始设定Roles
                foreach (var line in Lines)
                {
                    var group = groupCaches.First(s => s.Id == line.GroupId);
                    foreach (var roleId in line.Roles)
                    {
                        if (!group.RoleIds.Contains(roleId))
                        {
                            group.RoleIds.Add(roleId);
                        }
                    }
                }
                foreach (var group in groupCaches)
                {
                    if (group.ParentId.HasValue)
                    {
                        var parent = groupCaches.First(s => s.Id == group.ParentId.Value);
                        foreach (var roleId in group.RoleIds)
                        {
                            if (!parent.RoleIds.Contains(roleId))
                            {
                                parent.RoleIds.Add(roleId);
                            }
                        }
                    }
                }
                groups = groupCaches;
            }
        }

        /// <summary>
        ///     更新权限项静态数据
        /// </summary>
        public static void UpdateLines()
        {
            List<PermissionLineCache> list = new List<PermissionLineCache>();
            using (var ctx = new TimeTableDbContext())
            {
                var rolePermissions = ctx.RolePermissions.ToList();
                Dictionary<long, List<long>> map = new Dictionary<long, List<long>>();
                foreach (var rolePermission in rolePermissions)
                {
                    if (!map.ContainsKey(rolePermission.PermissionLineId))
                    {
                        map[rolePermission.PermissionLineId] = new List<long>();
                    }
                    if (!map[rolePermission.PermissionLineId].Contains(rolePermission.RoleId))
                    {
                        map[rolePermission.PermissionLineId].Add(rolePermission.RoleId);
                    }
                }
                //当前系统有角色使用的权限项
                var permissionLines = ctx.PermissionLines.Where(s => map.Keys.Contains(s.Id));

                foreach (var line in permissionLines)
                {
                    list.Add(new PermissionLineCache()
                    {
                        Id = line.Id,
                        GroupId = line.GroupId,
                        Roles = map[line.Id],
                        Url= line.Url==null?"":line.Url.ToLower(),
                         Tag=line.Key
                    });
                }
                lines = list;
            }
        }

        /// <summary>
        ///     根性用户角色静态数据
        /// </summary>
        public static void UpdateUserRoles()
        {
            Dictionary<long, List<long>> dic = new Dictionary<long, List<long>>();
            Dictionary<long, DateTime> dic2 = new Dictionary<long, DateTime>(); 
            using (var ctx = new TimeTableDbContext())
            {
                var now = DateTime.Now;
                var emps =  ctx.Employees.ToList();
                foreach (var emp in emps)
                {
                    var willBandEmps = ctx.Employees.Where(s => s.Enable && s.PermissionEndTime > now).ToList();
                   
                    if(emp.PermissionEndTime.HasValue && emp.PermissionEndTime <= now)
                    {
                   
                            emp.Enable = false;
                            emp.PermissionEndTime = null; 
                      
                    }
                    else
                    {
                        if (emp.PermissionEndTime.HasValue && emp.PermissionEndTime > now)
                        {
                            dic2.Add(emp.Id, emp.PermissionEndTime.Value);

                        }
               
                        var roles = emp.EmployeeRoles.Select(s => s.RoleId).ToList();
                        if (emp.Enable)
                        {
                            dic.Add(emp.Id, roles);
                        }
                    }
                
                    ctx.SaveChanges();
                }
            }
            userRoles = dic;
            bandTimes = dic2;
        }

    }
}