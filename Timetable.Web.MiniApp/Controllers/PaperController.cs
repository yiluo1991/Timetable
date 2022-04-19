﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Timetable.DbContext;
using Timetable.DbContext.Models;
using Timetable.Web.CommonViewModel;

namespace Timetable.Web.MiniApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaperController : ControllerBase
    {
        [HttpPost("[action]")]
        public PagerResponse List(PagerRequest req)
        {
            using (var ctx = new TimeTableDbContext())
            {
                IQueryable<Paper> query = ctx.Papers.Where(s=>s.Enable);
                if (!string.IsNullOrEmpty(req.keyword))
                    query = query.Where(s => EF.Functions.ILike(s.Title, "%" + req.keyword + "%") || EF.Functions.ILike(s.Url, "%" + req.keyword + "%"));
                var total = query.Count();
                return (new PagerResponse
                {
                    total = total,
                    data = query.OrderBy(s => s.SN).Skip((req.page - 1) * req.rows).Take(req.rows).Select(s => new
                    {
                        s.Id,
                        s.SN,
                        s.Title,
                        s.Url,
                        s.Enable
                    }).ToList()
                });
            }
        }
    }
}