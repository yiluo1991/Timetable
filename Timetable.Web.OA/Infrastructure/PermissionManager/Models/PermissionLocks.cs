using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Timetable.Web.OA.Infrastructure.PermissionManager.Models
{
    public class PermissionLocks
    {

        public static object GROUP_LOCK=new object();

        public static object LINE_LOCK = new object();

        public static object USERROLE_LOCK = new object();
        public static object BAND_LOCK = new object();
    }
}