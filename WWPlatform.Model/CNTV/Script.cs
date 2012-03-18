using System;
using WWPlatform.Core.Model;
using WWPlatform.Core.Utilities;
using System.ComponentModel.DataAnnotations;

namespace WWPlatform.Model.CNTV
{
    /// <summary>
    /// Script代表视频对应的讲稿
    /// </summary>
    //[Table("script",Schema="cntv")]
    public class Script : PersistObject
    {
        /// <summary>
        /// 讲稿编撰时间
        /// </summary>
        public DateTime Compiled
        { private set; get; }

        /// <summary>
        /// 讲稿正文内容
        /// </summary>
        public string Text
        { private set; get; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Brief
        {
            get
            {
                return this.Text.Replace("<h1>", "").Replace("</h1>", "").Replace("<p>", "").Replace("</p>", "").Truncate(53);
            }
        }

        /// <summary>
        /// 文字讲稿阅读次数
        /// </summary>
        //private int visit;
        public int Visit
        { get; set; }

        public virtual Webcast Webcast
        { get; set; }
    }
}