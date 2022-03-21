using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timetable.DbContext.Models;

namespace Timetable.Web.OA.ViewModel
{
    public class EmployeeWithRoleIds : Employee
    {
        public List<long> RoleIds { get; set; }
    }
}
