
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Timetable.DbContext;
using Timetable.Redis;
using Timetable.Security;

namespace Timetable.Web.OA.Infrastructure
{
    public class SinglePointManager
    {
        public static String GetSinglePointHash(long eid)
        {
            //从redis读取_cache_e_{eid}的值
            //无则从数据库读取，读取后写入到_cache_e{eid}
            string hash= GetSinglePointHashFromRedis(eid);
            if (hash == null)
            {
                using(var ctx = new TimeTableDbContext())
                {
                    var target = ctx.Employees.First(s => s.Id == eid);
                    if (!string.IsNullOrEmpty(target.SinglePointHsah))
                    {
                        SetSinglePointHashToRedis(eid, target.SinglePointHsah);
                    } 
                    return target.SinglePointHsah;
                }
            }
            return hash;
        }

        public static void UpdateSinglePointHash(long eid,string hash)
        {
            using (var ctx = new TimeTableDbContext())
            {
                var target = ctx.Employees.First(s => s.Id == eid);
                target.SinglePointHsah = hash;
                ctx.SaveChanges();
                SetSinglePointHashToRedis(eid, hash);
            }
        }

        private static string GetSinglePointHashFromRedis(long eid)
        {
            using (RedisOperater db = new RedisOperater(WebConfigDES.ConfigDES.Decrypt(ConfigurationManager.AppSettings["redis"])))
            { 
                return db.GetValue("_cache_e_" + eid);
            }
        }
        private  static bool  SetSinglePointHashToRedis(long eid,string hash)
        {
            using (RedisOperater db = new RedisOperater(WebConfigDES.ConfigDES.Decrypt(ConfigurationManager.AppSettings["redis"])))
            {
             return    db.SetValue("_cache_e_" + eid,hash, TimeSpan.FromDays(1));

            }
        }

        public static void ClearSinglePointHashToRedis(long eid)
        {
            using (RedisOperater db = new RedisOperater(WebConfigDES.ConfigDES.Decrypt(ConfigurationManager.AppSettings["redis"])))
            {
                 db.DeleteValue("_cache_e_" + eid);

            }
        }
    }
}
