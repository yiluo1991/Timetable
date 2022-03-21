using System;
using System.Collections.Generic;

namespace Timetable.Web.OA.Infrastructure.UserCache
{
    public class UserCacheInfo
    {
        public Guid Id { get; set; }
        public string RealName { get; set; }

        public bool Ok { get; set; }
        public long AnswerLimit { get; set; }

        public List<string> Permissions { get; set; }
        public int PermissoinLevle { get; set; }
 
        public string AvatarUrl { get; set; }
        public string NickName { get; set; }
        public uint xmin { get; set; }
        public long? GroupId { get; set; }
        public long UseGroupId { get; set; }
        public string OrganizationName { get; internal set; }
    }
}