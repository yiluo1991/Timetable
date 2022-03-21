using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
namespace Timetable.Web.OA.Infrastructure
{
    public class LoginLock
    {
        private static MemoryCache _memoryCache = new MemoryCache(new MemoryCacheOptions());
        public static bool MemberPasswordError(string loginName,out bool needLog,out int times)
        {
            needLog = false;
            times = 0;
            var name = "lock_" + loginName.ToLower();

            if (_memoryCache.Get(name) != null)
            {
                var newV = (int)_memoryCache.Get(name) + 1;
                _memoryCache.Set(name, newV, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                times = newV;
                if (newV >= 5)
                {
                    if (newV == 5||newV%100==0)
                    {
                        needLog = true;
                    }
                    
                    return true;
                }
                else {
                 
                    return false;
                }
            }
            else
            {
                var newV = 1;
                _memoryCache.Set(name, newV, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                return false;
            }
        }

        public static void MemberLoginSuccess(string loginName)
        {
            var name = "lock_" + loginName.ToLower();
            _memoryCache.Remove(name);
        }

        public static bool CanLogin(string loginName)
        {
            var name = "lock_" + loginName.ToLower();
            if (_memoryCache.Get(name) != null)
            {
                if ((int)_memoryCache.Get(name) >= 5)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else return true;
        }
    }
}
