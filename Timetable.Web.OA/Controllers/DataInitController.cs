using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Timetable.DbContext;
using Timetable.DbContext.Models;

namespace Timetable.Web.OA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataInitController : ControllerBase
    {

        [HttpGet("[action]")]
        public bool Init()
        {
            using (var ctx = new TimeTableDbContext())
            {
                if (ctx.Employees.Count() == 0)
                {
                    var pg0 = new PermissionGroup
                    {
                        DisplayName = "公共权限",
                        Key = "hidden",
                        Headshot = "",
                        SN = 0,
                        Url = "",
                        PermissionLines = new List<PermissionLine> {
                     new PermissionLine{
                         DisplayName ="修改自己的密码",
                         Url ="/api/employee/changepassword",
                         Key="C_P",
                         SN =1
                     },
                      new PermissionLine{
                         DisplayName ="获取自己的用户信息",
                         Url ="/api/employee/getemployeeinfo",
                         Key="E_I",
                         SN =1
                     },
                 }
                    };
                    //首页
                    var pg1 = new PermissionGroup
                    {
                        DisplayName = "首页",
                        Headshot = "el-icon-s-home",
                        SN = 1,
                        Url = "/admin",
                        PermissionLines = new List<PermissionLine> {
                     new PermissionLine{
                         DisplayName ="（必选）获取首页信息",
                         Url ="/api/home/info",
                         SN =1,
                         Key="H"
                     }
                 }
                    };

                    //账号和权限管理
                    var pg2 = new PermissionGroup
                    {
                        DisplayName = "账号和权限管理",
                        Headshot = "el-icon-user",
                        SN = 3,
                        Children = new List<PermissionGroup>
               {
                   new PermissionGroup
                   {
                       DisplayName="权限页管理",
                       SN=1,
                       Url="/admin/permissiongroup",
                       PermissionLines= new List<PermissionLine>{
                           new PermissionLine{
                               DisplayName="获取权限页列表",
                               Url="/api/permissiongroup/list",
                               Key="PG_G",
                               SN=1
                           },
                           new PermissionLine{
                                DisplayName="添加权限页",
                                Url="/api/permissiongroup/add",
                                Key="PG_A",
                                SN=2
                           },
                           new PermissionLine{
                                DisplayName="修改权限页",
                                Url="/api/permissiongroup/edit",
                                Key="PG_M",
                                SN=3
                           },
                           new PermissionLine{
                                DisplayName="删除权限页",
                                Url="/api/permissiongroup/delete",
                                Key="PG_D",
                                SN=4
                           }
                       }
                   },
                    new PermissionGroup
                   {
                       DisplayName="页详细权限项管理",
                       SN=2,
                       Url="/admin/permissionline",
                       PermissionLines= new List<PermissionLine>{
                           new PermissionLine
                           {
                               DisplayName="获取权限项列表",
                               Url="/api/permissionline/list",
                               SN=1,
                               Key="PL_G"
                           },
                           new PermissionLine
                           {
                               DisplayName="添加权限项",
                               Url="/api/permissionline/add",
                               SN=2,
                               Key="PL_A"
                           },
                            new PermissionLine
                           {
                               DisplayName="修改权限项",
                               Url="/api/permissionline/edit",
                               SN=3,
                               Key="PL_M"
                           },
                             new PermissionLine
                           {
                               DisplayName="删除权限项",
                               Url="/api/permissionline/delete",
                               SN=4,
                               Key="PL_D"
                           },
                           new PermissionLine
                           {
                               DisplayName="获取权限页下拉列表",
                               Url="/api/permissiongroup/list",
                               SN=4,
                               Key="PL_PGG"
                           }
                       }
                   },
                   new PermissionGroup
                   {
                       DisplayName="角色权限",
                       SN=3,
                       Url="/admin/role",
                       PermissionLines= new List<PermissionLine>{
                           new PermissionLine
                           {
                               DisplayName="获取角色列表",
                               Url="/api/role/list",
                               SN=1,
                               Key="R_G"
                           },
                           new PermissionLine
                           {
                               DisplayName="添加角色",
                               Url="/api/role/add",
                               SN=2,
                               Key="R_A"
                           },
                           new PermissionLine
                           {
                               DisplayName="修改角色",
                               Url="/api/role/edit",
                               SN=3,
                               Key="R_M"
                           },
                           new PermissionLine
                           {
                               DisplayName="删除角色",
                               Url="/api/role/delete",
                               SN=4,
                               Key="R_D"
                           },
                             new PermissionLine
                           {
                               DisplayName="获取权限项树形数据",
                               Url="/api/permissionline/tree",
                               SN=5,
                               Key="R_PLG"
                           }
                       }
                   },
                    new PermissionGroup
                   {
                       DisplayName="管理员账号",
                       SN=4,
                       Url="/admin/employee",
                       PermissionLines= new List<PermissionLine>{
                           new PermissionLine
                           {
                               DisplayName="获取管理员列表",
                               Url="/api/employee/list",
                               SN=1,
                               Key="E_G"
                           },
                           new PermissionLine
                           {
                               DisplayName="添加管理员",
                               Url="/api/employee/add",
                               SN=2,
                               Key="E_A"
                           },
                           new PermissionLine
                           {
                               DisplayName="修改管理员",
                               Url="/api/employee/edit",
                               SN=3,
                               Key="E_M"
                           },
                              new PermissionLine
                           {
                               DisplayName="删除管理员",
                               Url="/api/employee/delete",
                               SN=4,
                               Key="E_D"
                           },
                             new PermissionLine
                           {
                               DisplayName="重置管理员密码",
                               Url="/api/employee/resetpassword",
                               SN=5,
                               Key="E_R"
                           },

                               new PermissionLine
                           {
                               DisplayName="获取角色下拉列表",
                               Url="/api/role/list",
                               SN=6,
                               Key="E_RG"
                           },
                                  new PermissionLine
                           {
                               DisplayName="验证用户名是否可用",
                               Url="/api/employee/checkloginname",
                               SN=7,
                               Key="E_V"
                           },
                   }
                   },
                        new PermissionGroup
                   {
                       DisplayName="管理员登录日志",
                       SN=5,
                       Url="/admin/employeeloginlog",
                       PermissionLines= new List<PermissionLine>{

                                  new PermissionLine
                           {
                               DisplayName="获取日志",
                               Url="/api/employeeloginlog/list",
                               SN=1,
                               Key="EL_G"
                           },
                   }
                   },

               }
                    };
                    ctx.PermissionGroups.Add(pg0);
                    ctx.PermissionGroups.Add(pg1);
                    ctx.PermissionGroups.Add(pg2);
                    ctx.SaveChanges();
                    Role adminRole = new Role
                    {
                        Name = "管理员",

                    };
                    ctx.Roles.Add(adminRole);
                    adminRole.RolePermissions = new List<RolePermission>();
                    var permissionLines = ctx.PermissionLines.ToList();

                    foreach (var line in permissionLines)
                    {
                        adminRole.RolePermissions.Add(new RolePermission
                        {
                            PermissionLineId = line.Id
                        });
                    }
                    ctx.SaveChanges();
                    var admin = new Employee()
                    {
                        CreateTime = DateTime.Now,
                        Email = "sgliyang@126.com",
                        Enable = true,
                        LoginName = "admin",
                        Headshot = "/upload/default/default.jpg",
                        Mobile = "18559819573",
                        RealName = "李小阳",
                        Password = Crypto.HashPassword("admin123456")
                    };
                    admin.EmployeeRoles = new List<EmployeeRole>();
                    foreach (var item in ctx.Roles.ToList())
                    {
                        admin.EmployeeRoles.Add(new EmployeeRole { RoleId = item.Id });
                    };
                    ctx.Employees.Add(admin);
                    ctx.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        [HttpGet("[action]")]
        public string test()
        {

            return Crypto.HashPassword("admin123456"); 
        }
    }
}