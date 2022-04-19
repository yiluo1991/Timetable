using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Timetable.DbContext.Models;
using Timetable.Tools;
using Timetable.Tools.Model;

namespace Timetable.Web.MiniApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolTermController : ControllerBase
    {

       [HttpGet]
        public SchoolTermCacheViewModel Get() {
           return  SchoolTermCacheManager.GetCurrentTerm();
        }
    }
}