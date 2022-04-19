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


                    var pg3 = new PermissionGroup
                    {

                        DisplayName = "学年管理",
                        Headshot = "el-icon-date",
                        SN = 4,
                        Url = "/admin/schoolterm",
                        PermissionLines = new List<PermissionLine>{
                            new PermissionLine{
                                DisplayName="获取学年列表",
                                Url="/api/schoolterm/list",
                                Key="ST_G",
                                SN=1
                            },
                             new PermissionLine{
                                DisplayName="添加学年",
                                Url="/api/schoolterm/add",
                                Key="ST_A",
                                SN=2
                            },
                              new PermissionLine{
                                DisplayName="清空学年排课和选课信息",
                                Url="/api/schoolterm/clear",
                                Key="ST_C",
                                SN=3
                            },
                               new PermissionLine{
                                DisplayName="删除学年信息",
                                Url="/api/schoolterm/delete",
                                Key="ST_D",
                                SN=4
                            },
                                  new PermissionLine{
                                DisplayName="设为当前学期",
                                Url="/api/schoolterm/setdefault",
                                Key="ST_S",
                                SN=5
                            },
                        }
                    };

                    var pg4 = new PermissionGroup
                    {
                        DisplayName = "院系管理",
                        Headshot = "el-icon-school",
                        SN = 5,
                        Url = "/admin/collegedepartment",
                        PermissionLines = new List<PermissionLine>{
                            new PermissionLine{
                                DisplayName="获取院系列表列表",
                                Url="/api/collegedepartment/list",
                                Key="CD_G",
                                SN=1
                            },
                             new PermissionLine{
                                DisplayName="添加院系",
                                Url="/api/collegedepartment/add",
                                Key="CD_A",
                                SN=2
                            },
                              new PermissionLine{
                                DisplayName="院系下拉框数据",
                                Url="/api/collegedepartment/collegecombo",
                                Key="CD_GC",
                                SN=3
                            },

                                  new PermissionLine{
                                DisplayName="修改院系",
                                Url="/api/collegedepartment/edit",
                                Key="CD_M",
                                SN=4
                            },
                        }
                    };


                    var pg5 = new PermissionGroup
                    {
                        DisplayName = "科目与课程安排",
                        Headshot = "el-icon-s-management",
                        SN = 6,
                        Children = new List<PermissionGroup> {
                                new PermissionGroup{
                                  DisplayName = "科目",
                                  SN = 1,
                                  Url = "/admin/subject",
                                  PermissionLines = new List<PermissionLine>{
                                        new PermissionLine{
                                            DisplayName="获取科目",
                                            Url="/api/subject/list",
                                            Key="S_G",
                                            SN=1
                                        },
                                        new PermissionLine{
                                            DisplayName="添加科目",
                                            Url="/api/subject/add",
                                            Key="S_A",
                                            SN=2
                                        },
                                        new PermissionLine{
                                            DisplayName="修改科目",
                                            Url="/api/subject/edit",
                                            Key="S_M",
                                            SN=3
                                        },
                                        new PermissionLine{
                                            DisplayName="删除科目",
                                            Url="/api/subject/delete",
                                            Key="S_D",
                                            SN=4
                                        },
                                        new PermissionLine{
                                            DisplayName="检查科目名",
                                            Url="/api/subject/checkexist",
                                            Key="S_C",
                                            SN=5
                                        },
                                        new PermissionLine{
                                            DisplayName="获取院系下拉框数据",
                                            Url="/api/collegedepartment/combodata",
                                            Key="S_GCD",
                                            SN=6
                                        }
                                    }
                                },
                                new PermissionGroup{
                                  DisplayName = "本学期课程安排",
                                  SN = 2,
                                  Url = "/admin/course",
                                  PermissionLines = new List<PermissionLine>{
                                        new PermissionLine{
                                            DisplayName="获取本学期列表",
                                            Url="/api/course/currentlist",
                                            Key="C_GC",
                                            SN=1
                                        },

                                        new PermissionLine{
                                            DisplayName="添加课程安排",
                                            Url="/api/course/add",
                                            Key="C_A",
                                            SN=2
                                        },

                                        new PermissionLine{
                                            DisplayName="删除科目",
                                            Url="/api/course/delete",
                                            Key="C_D",
                                            SN=3
                                        },
                                         new PermissionLine{
                                            DisplayName="获取可用科目列表",
                                            Url="/api/subject/enablelist",
                                            Key="C_GS",
                                            SN=4
                                        },
                                          new PermissionLine{
                                            DisplayName="获取可用教师列表",
                                            Url="/api/teacher/enablelist",
                                            Key="C_GT",
                                            SN=5
                                        },
                                            new PermissionLine{
                                            DisplayName="获取可用班级",
                                            Url="/api/administrativeclass/currentlist",
                                            Key="C_GAC",
                                            SN=6
                                        },
                                             new PermissionLine{
                                            DisplayName="获取学期信息",
                                            Url="/api/schoolterm/getcurrentterm",
                                            Key="C_GCT",
                                            SN=7
                                        },
                                    }
                                },
                               new PermissionGroup{
                                  DisplayName = "全部课程安排",
                                  SN = 3,
                                  Url = "/admin/allcourse",
                                  PermissionLines = new List<PermissionLine>{
                                        new PermissionLine{
                                            DisplayName="获取列表",
                                            Url="/api/course/alllist",
                                            Key="C_GA",
                                            SN=1
                                        }


                                    }
                                },

                        }

                    };

                    var pg6 = new PermissionGroup
                    {
                        DisplayName = "教师和学生账号",
                        Headshot = "el-icon-s-custom",
                        SN = 7,
                        Children = new List<PermissionGroup> {
                                new PermissionGroup{
                                  DisplayName = "教师账号管理",
                                  SN = 1,
                                  Url = "/admin/teacher",
                                  PermissionLines = new List<PermissionLine>{
                                        new PermissionLine{
                                            DisplayName="获取教师账号列表",
                                            Url="/api/teacher/list",
                                            Key="T_G",
                                            SN=1
                                        },
                                        new PermissionLine{
                                            DisplayName="添加教师账号",
                                            Url="/api/teacher/add",
                                            Key="T_A",
                                            SN=2
                                        },
                                        new PermissionLine{
                                            DisplayName="修改教师账号",
                                            Url="/api/teacher/edit",
                                            Key="T_M",
                                            SN=3
                                        },
                                        new PermissionLine{
                                            DisplayName="删除教师账号",
                                            Url="/api/teacher/delete",
                                            Key="T_D",
                                            SN=4
                                        },
                                        new PermissionLine{
                                            DisplayName="检查教师账号",
                                            Url="/api/teacher/checkexist",
                                            Key="T_C",
                                            SN=5
                                        },
                                        new PermissionLine{
                                            DisplayName="重置教师密码",
                                            Url="/api/teacher/resetpassword",
                                            Key="T_R",
                                            SN=6
                                        }
                                    }
                                },
                                  new PermissionGroup{
                                  DisplayName = "班级管理",
                                  SN = 2,
                                  Url = "/admin/administrativeclass",
                                  PermissionLines = new List<PermissionLine>{
                                        new PermissionLine{
                                            DisplayName="获取班级列表",
                                            Url="/api/administrativeclass/list",
                                            Key="AC_G",
                                            SN=1
                                        },
                                        new PermissionLine{
                                            DisplayName="添加班级",
                                            Url="/api/administrativeclass/add",
                                            Key="AC_A",
                                            SN=2
                                        },
                                        new PermissionLine{
                                            DisplayName="修改班级",
                                            Url="/api/administrativeclass/edit",
                                            Key="AC_M",
                                            SN=3
                                        },
                                        new PermissionLine{
                                            DisplayName="删除班级",
                                            Url="/api/administrativeclass/delete",
                                            Key="AC_D",
                                            SN=4
                                        },
                                          new PermissionLine{
                                            DisplayName="获取院系下拉框数据",
                                            Url="/api/collegedepartment/combodata",
                                            Key="AC_GCD",
                                            SN=5
                                        }
                                    }
                                },
                                   new PermissionGroup{
                                  DisplayName = "学生账号管理",
                                  SN = 3,
                                  Url = "/admin/student",
                                  PermissionLines = new List<PermissionLine>{
                                        new PermissionLine{
                                            DisplayName="获取学生账号列表",
                                            Url="/api/student/list",
                                            Key="SU_G",
                                            SN=1
                                        },
                                        new PermissionLine{
                                            DisplayName="添加学生账号",
                                            Url="/api/student/add",
                                            Key="SU_A",
                                            SN=2
                                        },
                                        new PermissionLine{
                                            DisplayName="修改学生账号",
                                            Url="/api/student/edit",
                                            Key="SU_M",
                                            SN=3
                                        },
                                        new PermissionLine{
                                            DisplayName="删除学生账号",
                                            Url="/api/student/delete",
                                            Key="SU_D",
                                            SN=4
                                        },
                                        new PermissionLine{
                                            DisplayName="检查学生账号",
                                            Url="/api/student/checkexist",
                                            Key="SU_C",
                                            SN=5
                                        },
                                        new PermissionLine{
                                            DisplayName="重置学生密码",
                                            Url="/api/student/resetpassword",
                                            Key="SU_R",
                                            SN=6
                                        },
                                            new PermissionLine{
                                            DisplayName="获取班级数据",
                                            Url="/api/administrativeclass/list",
                                            Key="SU_GAC",
                                            SN=7
                                        }
                                    }
                                },

                        }

                    };
                    var pg7 = new PermissionGroup
                    {
                        DisplayName = "学生选课信息",
                        Headshot = "el-icon-s-claim",
                        SN = 8,
                        Url = "/admin/coursepick",
                        PermissionLines = new List<PermissionLine>{
                            new PermissionLine{
                                DisplayName="列表",
                                Url="/api/coursepick/list",
                                Key="CP_G",
                                SN=1
                            },
                             new PermissionLine{
                                DisplayName="添加",
                                Url="/api/coursepick/add",
                                Key="CP_A",
                                SN=2
                            }, 
                            new PermissionLine{
                                DisplayName="删除",
                                Url="/api/coursepick/delete",
                                Key="CP_D",
                                SN=3
                            },
                             new PermissionLine{
                                DisplayName="获取学生信息",
                                Url="/api/student/list",
                                Key="CP_GS",
                                SN=4
                            },
                               new PermissionLine{
                                DisplayName="获取课程信息",
                                Url="/api/course/alllist",
                                Key="CP_GC",
                                SN=5
                            },
                        }
                    };

                    var pg8 = new PermissionGroup
                    {
                        DisplayName = "小程序公告和问卷",
                        Headshot = "el-icon-bell",
                        SN = 9,
                        Children = new List<PermissionGroup> {
                                new PermissionGroup{
                                  DisplayName = "通知公告管理",
                                  SN = 1,
                                  Url = "/admin/notice",
                                  PermissionLines = new List<PermissionLine>{
                                        new PermissionLine{
                                            DisplayName="获取列表",
                                            Url="/api/notice/list",
                                            Key="N_G",
                                            SN=1
                                        },
                                        new PermissionLine{
                                            DisplayName="添加公告",
                                            Url="/api/notice/add",
                                            Key="N_A",
                                            SN=2
                                        },
                                        new PermissionLine{
                                            DisplayName="修改",
                                            Url="/api/notice/edit",
                                            Key="N_M",
                                            SN=3
                                        },
                                        new PermissionLine{
                                            DisplayName="删除",
                                            Url="/api/notice/delete",
                                            Key="N_D",
                                            SN=4
                                        }
                                      
                                    }
                                },
                                  new PermissionGroup{
                                  DisplayName = "问卷列表管理",
                                  SN = 2,
                                  Url = "/admin/paper",
                                  PermissionLines = new List<PermissionLine>{
                                        new PermissionLine{
                                            DisplayName="获取列表",
                                            Url="/api/paper/list",
                                            Key="P_G",
                                            SN=1
                                        },
                                        new PermissionLine{
                                            DisplayName="添加",
                                            Url="/api/paper/add",
                                            Key="P_A",
                                            SN=2
                                        },
                                        new PermissionLine{
                                            DisplayName="修改",
                                            Url="/api/paper/edit",
                                            Key="P_M",
                                            SN=3
                                        },
                                        new PermissionLine{
                                            DisplayName="删除",
                                            Url="/api/paper/delete",
                                            Key="P_D",
                                            SN=4
                                        },
                                       
                                    }
                                },
                               

                        }

                    };
                    ctx.PermissionGroups.Add(pg0);
                    ctx.PermissionGroups.Add(pg1);
                    ctx.PermissionGroups.Add(pg2);
                    ctx.PermissionGroups.Add(pg3);
                    ctx.PermissionGroups.Add(pg4);
                    ctx.PermissionGroups.Add(pg5);
                    ctx.PermissionGroups.Add(pg6);
                    ctx.PermissionGroups.Add(pg7);
                    ctx.PermissionGroups.Add(pg8);
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

    }
}