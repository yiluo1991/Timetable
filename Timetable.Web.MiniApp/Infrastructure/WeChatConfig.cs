using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Timetable.Web.MiniApp.Infrastructure
{
    public static class WeChatConfig
    {
        private static String appId;

        public static String AppId
        {
            get
            {
                if (appId == null)
                {
                    appId = ConfigurationManager.AppSettings["AppId"];
                }
                return appId;

            }
        }
        private static String appSecret;

        public static String AppSecret
        {
            get
            {
                if (appSecret == null)
                {
                    appSecret = ConfigurationManager.AppSettings["AppSecret"];
                }
                return appSecret;
            }
        }
    }
}
