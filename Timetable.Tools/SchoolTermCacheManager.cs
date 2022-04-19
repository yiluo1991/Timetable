using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Timetable.DbContext;
using Timetable.Redis;
using Timetable.Security;
using Timetable.Tools.Model;

namespace Timetable.Tools
{
   public  class SchoolTermCacheManager
    {
        public static SchoolTermCacheViewModel GetCurrentTerm() {
               using (RedisOperater db = new RedisOperater(WebConfigDES.ConfigDES.Decrypt(ConfigurationManager.AppSettings["redis"])))
            {
                SchoolTermCacheViewModel vm=db.GetValue<SchoolTermCacheViewModel>("_cache_current_term");
                if (vm == null)
                {
                   var m= UpdateCache(db);
                    if (m == null)
                    {
                        return null;
                    }
                    else{
                        return m;
                    }
                }
                else
                {
                    return vm;
                }
            }
        }

        private static SchoolTermCacheViewModel UpdateCache(RedisOperater db)
        {
            using (TimeTableDbContext ctx = new TimeTableDbContext())
            {
                var df = ctx.SchoolTerms.FirstOrDefault(s => s.IsDefault);
                if (df != null)
                {
                    var vm = new SchoolTermCacheViewModel
                    {
                        Id = df.Id,
                        CustomEnd = df.CustomEnd,
                        CustomStart = df.CustomStart,
                        FixedStart = df.FixedStart
                    };
                    db.SetValue<SchoolTermCacheViewModel>("_cache_current_term",vm );
                    return vm;
                }
                else
                {
                    return null;
                }
            }
        }

        public static void ClearCache()
        {
            using (RedisOperater db = new RedisOperater(WebConfigDES.ConfigDES.Decrypt(ConfigurationManager.AppSettings["redis"])))
            {
                db.DeleteValue("_cache_current_term");
            }
            }
    }
}
