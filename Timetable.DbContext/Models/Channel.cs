
using System;
using System.Collections.Generic;
using System.Text;

namespace Timetable.DbContext.Models
{
    /// <summary>
    ///     频道，放着备用，需求中没有
    /// </summary>
    public class Channel
    {
    
            /// <summary>
            ///     Id
            /// </summary>
            public long Id { get; set; }

            /// <summary>
            ///     频道唯一标识
            /// </summary>
            public string Key { get; set; }

            /// <summary>
            ///     频道中文名
            /// </summary>
            public string Name_CN { get; set; }

            /// <summary>
            ///     频道英文名
            /// </summary>
            public string Name_EN { get; set; }

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
            ///     父频道Id
            /// </summary>
            public long? ParentId { get; set; }

            /// <summary>
            ///     可用
            /// </summary>
            public bool Enable { get; set; }

            /// <summary>
            ///     在文章管理中可选中该类别
            /// </summary>
            public bool VisibleInArticle { get; set; }

            /// <summary>
            ///     使用特定跳转URL
            /// </summary>
            public bool HasSpecialUrl { get; set; }

            /// <summary>
            ///     强制指定跳转Url，系统内置频道会用到。管理系统中无需管理该项
            /// </summary>
            public string SpecialUrl { get; set; }

            /// <summary>
            ///     排序号
            /// </summary>
            public int SortNum { get; set; }


        
        /// <summary>
        ///     导航属性：子频道
        /// </summary>
        public virtual ICollection<Channel> Children { get; set; }
        
        /// <summary>
        ///     导航属性：父频道
        /// </summary>
        public virtual Channel Parent { get; set; }
        
        /// <summary>
        ///     导航属性：文章
        /// </summary>
        public virtual ICollection<Article> Articles { get; set; }
        
    }
}
