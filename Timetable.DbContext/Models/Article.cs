
using System;
using System.Collections.Generic;
using System.Text;

namespace Timetable.DbContext.Models
{
    /// <summary>
    ///     文章，备用
    /// </summary>
    public class Article
    {
        public long Id { get; set; }

        /// <summary>
        ///     中文标题
        /// </summary>
        public string Title_CN { get; set; }

        /// <summary>
        ///     英文标题
        /// </summary>
        public string Title_EN { get; set; }

        /// <summary>
        ///     作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        ///     中文SEO关键词
        /// </summary>
        public string SEOKeywords_CN { get; set; }

        /// <summary>
        ///     英文SEO关键词
        /// </summary>
        public string SEOKeywords_EN { get; set; }

        /// <summary>
        ///     中文SEO描述
        /// </summary>
        public string Description_CN { get; set; }

        /// <summary>
        ///     英文SEO描述
        /// </summary>
        public string Description_EN { get; set; }

        /// <summary>
        ///     启用
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        ///     排序号
        /// </summary>
        public int SortNum { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        ///     阅读次数
        /// </summary>
        public long ViewCount { get; set; }

        /// <summary>
        ///     创建人Id
        /// </summary>
        public long EmployeeId { get; set; }

        /// <summary>
        ///     频道Id
        /// </summary>
        public long ChannelId { get; set; }

        /// <summary>
        ///     中文内容
        /// </summary>
        public string Content_CN { get; set; }

        /// <summary>
        ///     英文内容
        /// </summary>
        public string Content_EN { get; set; }

        /// <summary>
        ///     图片地址
        /// </summary>
        public string Src { get; set; }

        /// <summary>
        ///     发布时间
        /// </summary>
        public Nullable<DateTime> PublishData { get; set; }

        /// <summary>
        ///     导航属性：频道
        /// </summary>
        public virtual Channel Channel { get; set; }





        /// <summary>
        ///     导航属性：创建人
        /// </summary>
        
        public virtual Employee Employee { get; set; }
    }
}
