using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Timetable.DbContext;
using Timetable.DbContext.Models;
using Timetable.Web.CommonViewModel;
using Timetable.Web.OA.Infrastructure.Authorization;
using Timetable.Web.OA.ViewModel;

namespace Timetable.Web.OA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Permission]
    public class CollegeDepartmentController : ControllerBase
    {
        /// <summary>
        ///     获取列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public ResponseBase List(PagerRequest  req)
        {
            
            using (var ctx = new TimeTableDbContext())
            {
                List<CollegeDepartmentTreeViewModel> colleges = new List<CollegeDepartmentTreeViewModel>();
                IQueryable<Department> query = ctx.Departments;
                List<College> collegesWithKeyword;
                if (!string.IsNullOrWhiteSpace(req.keyword))
                {
                    var k = "%" + req.keyword + "%";
                    query = ctx.Departments.Where(s => EF.Functions.ILike(s.College.ContactName, k) || EF.Functions.ILike(s.College.ContactPhone, k) || EF.Functions.ILike(s.College.Remark, k) || EF.Functions.ILike(s.College.Name, k) || EF.Functions.ILike(s.ContactName, k) || EF.Functions.ILike(s.ContactPhone, k) || EF.Functions.ILike(s.Remark, k) || EF.Functions.ILike(s.Name, k));
                    collegesWithKeyword=ctx.Colleges.Where(s => EF.Functions.ILike(s.ContactName, k) || EF.Functions.ILike(s.ContactPhone, k) || EF.Functions.ILike(s.Remark, k) || EF.Functions.ILike(s.Name, k)||s.Departments.Count(t=> EF.Functions.ILike(t.ContactName, k) || EF.Functions.ILike(t.ContactPhone, k) || EF.Functions.ILike(t.Remark, k) || EF.Functions.ILike(t.Name, k))>0).ToList();
                    List<CollegeDepartmentTreeViewModel> nodes = query.OrderBy(s => s.College.Id).ThenBy(s => s.Id).Skip((req.page - 1) * req.rows).Take(req.rows).Select(s => new CollegeDepartmentTreeViewModel
                    {
                        ContactName = s.ContactName,
                        ContactPhone = s.ContactPhone,
                        Id = s.Id,
                        CollegeId = s.CollegeId,
                        Enable = s.Enable,
                        Name = s.Name,
                        Remark = s.Remark,
                        SID = "D_" + s.Id
                    }).ToList();
               
                    collegesWithKeyword.ForEach(s => {
                        if (s.Name.Contains(req.keyword, StringComparison.OrdinalIgnoreCase) || s.ContactName.Contains(req.keyword, StringComparison.OrdinalIgnoreCase) || s.ContactPhone.Contains(req.keyword,StringComparison.OrdinalIgnoreCase) || s.Remark.Contains(req.keyword,StringComparison.OrdinalIgnoreCase))
                        {
                            colleges.Add(new CollegeDepartmentTreeViewModel {
                                ContactName = s.ContactName,
                                ContactPhone = s.ContactPhone,
                                Id = s.Id,
                                Enable = s.Enable,
                                Name = s.Name,
                                Remark = s.Remark,
                                SID = "C_" + s.Id,
                                children= s.Departments.OrderBy(t=>t.Id).Select(s => new CollegeDepartmentTreeViewModel
                                {
                                    ContactName = s.ContactName,
                                    ContactPhone = s.ContactPhone,
                                    Id = s.Id,
                                    CollegeId = s.CollegeId,
                                    Enable = s.Enable,
                                    Name = s.Name,
                                    Remark = s.Remark,
                                    SID = "D_" + s.Id
                                }).ToList()
                            });
                        }
                        else
                        {
                            colleges.Add(new CollegeDepartmentTreeViewModel
                            {
                                ContactName = s.ContactName,
                                ContactPhone = s.ContactPhone,
                                Id = s.Id,
                                Enable = s.Enable,
                                Name = s.Name,
                                Remark = s.Remark,
                                SID = "C_" + s.Id,
                                children = nodes.Where(s=>s.CollegeId==s.Id).ToList()
                            }); 
                        }
                    
                    });
                     
                }
                else {
                    collegesWithKeyword = ctx.Colleges.OrderBy(s=>s.Id).ToList();
                  
                    collegesWithKeyword.ForEach(s => {
                            colleges.Add(new CollegeDepartmentTreeViewModel
                            {
                                ContactName = s.ContactName,
                                ContactPhone = s.ContactPhone,
                                Id = s.Id,
                                Enable = s.Enable,
                                Name = s.Name,
                                Remark = s.Remark,
                                SID = "C_" + s.Id,
                                children = s.Departments.OrderBy(t => t.Id).Select(s => new CollegeDepartmentTreeViewModel
                                {
                                    ContactName = s.ContactName,
                                    ContactPhone = s.ContactPhone,
                                    Id = s.Id,
                                    CollegeId = s.CollegeId,
                                    Enable = s.Enable,
                                    Name = s.Name,
                                    Remark = s.Remark,
                                    SID = "D_" + s.Id
                                }).ToList()
                            });
                    });
                } 
             
                return new ResponseBase()
                {
                    success = true,
                    data = colleges,
                    msg = "获取成功"
                  
                };
            }

        }

        /// <summary>
        ///     学院下拉框数据
        /// </summary>
        /// <returns></returns>
        [HttpPost("[action]")]
        public ResponseBase CollegeCombo()
        {
            using (var ctx = new TimeTableDbContext())
            {
                return new ResponseBase
                {
                    data = ctx.Colleges.OrderBy(s => s.Id).Select(s => new
                    {
                        s.Id,
                        s.Name
                    }).ToList(),
                    success = true,
                    msg = "获取成功"
                };
            }
        }

        /// <summary>
        ///     获取可用院系combo数据
        /// </summary>
        /// <returns></returns>
        [HttpPost("[action]")]
        public ResponseBase ComboData() {
            using (var ctx = new TimeTableDbContext())
            {
                var colleges = new List<CollegeDepartmentTreeViewModel>();
                ctx.Colleges.Where(s => s.Enable).OrderBy(s => s.Id).ToList().ForEach(s => {
                    colleges.Add(new CollegeDepartmentTreeViewModel
                    {
                        ContactName = s.ContactName,
                        ContactPhone = s.ContactPhone,
                        Id = s.Id,
                        Enable = s.Enable,
                        Name = s.Name,
                        Remark = s.Remark,
                        SID = "C_" + s.Id+"D_",
                        children = s.Departments.Where(s => s.Enable).OrderBy(t => t.Id).Select(t => new CollegeDepartmentTreeViewModel
                        {
                            ContactName = t.ContactName,
                            ContactPhone = t.ContactPhone,
                            Id = t.Id,
                            CollegeId = t.CollegeId,
                            Enable = t.Enable,
                            Name = t.Name,
                            Remark = t.Remark,
                            SID = "C_" + s.Id + "D_" + t.Id
                        }).ToList()
                    });
                });
                return new ResponseBase() { 
                success=true,
                 data=colleges,
                 msg="获取成功"
                };    
                
                
            }
        }


        /// <summary>
        ///     添加节点
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public ResponseBase Add(CollegeDepartmentTreeViewModel req)
        {
            using (var ctx = new TimeTableDbContext())
            {
                if (!req.CollegeId.HasValue)
                {
                    //添加学院
                    College college = new College
                    {
                        ContactName = req.ContactName,
                        ContactPhone = req.ContactPhone,
                        Enable = req.Enable,
                        Name = req.Name,
                        Remark = req.Remark
                    };
                    ctx.Colleges.Add(college);
                    ctx.SaveChanges();
                    return new ResponseBase
                    {
                        success = true,
                        msg = "添加成功"
                    };
                }
                else
                {
                    //添加系
                    Department department = new Department()
                    {
                        ContactName = req.ContactName,
                        ContactPhone = req.ContactPhone,
                        Enable = req.Enable,
                        Name = req.Name,
                        Remark = req.Remark,
                        CollegeId = req.CollegeId.Value
                    };

                    ctx.Departments.Add(department);
                    try
                    {
                        ctx.SaveChanges();
                        return new ResponseBase
                        {
                            success = true,
                            msg = "添加成功"
                        };
                    }
                    catch (Exception)
                    {
                        return new ErrorResponse
                        {
                            success = false,
                            errCode = "ERR_PARENT_NOT_FOUND",
                            msg = "父级不存在"
                        };
                    }
                }
            }
        }

        /// <summary>
        ///     修改
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public ResponseBase Edit(CollegeDepartmentTreeViewModel req)
        {
            using (var ctx = new TimeTableDbContext())
            {
                if (!req.CollegeId.HasValue)
                {
                    //修改学院
                    var target = ctx.Colleges.FirstOrDefault(s => s.Id == req.Id);
                    if (target != null)
                    {
                        target.Name = req.Name;
                        target.Remark = req.Remark;
                        target.ContactName = req.ContactName;
                        target.ContactPhone = req.ContactPhone;
                        target.Enable = req.Enable;
                        ctx.SaveChanges();
                        return new ResponseBase
                        {
                            success = true,
                            msg = "修改成功"
                        };
                    }
                }
                else
                {
                    var target = ctx.Departments.FirstOrDefault(s => s.Id == req.Id);
                    var newParent = ctx.Colleges.FirstOrDefault(s => s.Id == req.CollegeId.Value);
                    if (newParent != null)
                    {
                        if (target != null)
                        {
                            target.Name = req.Name;
                            target.Remark = req.Remark;
                            target.ContactName = req.ContactName;
                            target.ContactPhone = req.ContactPhone;
                            target.Enable = req.Enable;
                            target.CollegeId = req.CollegeId.Value;
                            ctx.SaveChanges(); 
                            return new ResponseBase
                            {
                                success = true,
                                msg = "修改成功"
                            };
                        }
                        return new ErrorResponse { success = false, msg = "没有找到所属学院/系", errCode = "ERR_PARENT_NOT_FOUND" };
                    }

                }
                return new ErrorResponse { success = false, msg = "没有找到要修改的数据", errCode = "ERR_NOT_FOUND" };
            } 

        }
    }
}