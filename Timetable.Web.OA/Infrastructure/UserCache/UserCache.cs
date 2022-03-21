using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Timetable.DbContext;
using Timetable.Redis;
using Timetable.Security;

namespace Timetable.Web.OA.Infrastructure.UserCache
{
    public class UserCacheManager
    {
        public static UserCacheInfo GetUserCache(Guid id) {
            using (RedisOperater db = new RedisOperater(WebConfigDES.ConfigDES.Decrypt(ConfigurationManager.AppSettings["redis"])))
            {
          
                return db.GetValue<UserCacheInfo>("_cache_" + id);
            }
        }

        public static UserCacheInfo UpdateUserCache(Guid guid)
        {
            UserCacheInfo usercache;
            using (var ctx = new TimeTableDbContext())
            {
                var user = ctx.Users.First(s => s.Id == guid);
                var groups = ctx.UserGroups.ToList();
                var group = user.UserGroupId.HasValue ? groups.First(s => s.Id == user.UserGroupId.Value) : groups.First(s => s.IsDefault);


                usercache = new UserCacheInfo
                {
                    Id = user.Id,
                    AnswerLimit = 1000000,
                    AvatarUrl = user.AvatarUrl,
                    RealName = user.RealName,
                    GroupId = user.UserGroupId,
                    UseGroupId = group.Id,
                    OrganizationName=  user.OrganizationName,
                    Permissions = JsonConvert.DeserializeObject<List<string>>(group.Permissions.GetRawText()), 
                    NickName = user.NickName,
                    xmin = group.xmin,
                    PermissoinLevle = group.PermissionLevel
                };
                using (RedisOperater db = new RedisOperater(WebConfigDES.ConfigDES.Decrypt(ConfigurationManager.AppSettings["redis"])))
                {
                    db.SetValue<UserCacheInfo>("_cache_" + usercache.Id, usercache, TimeSpan.FromDays(1));
                }
                return usercache;
            }

        }

    }


}
