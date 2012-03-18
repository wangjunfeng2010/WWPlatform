using System;
using WWPlatform.Core.Paging;
using WWPlatform.Model.ATK;

namespace WWPlatform.Web.ViewModels.ATK
{
    public class ContentViewModel : AtkViewModel, IPageable
    {
        /// <summary>
        /// 文章
        /// </summary>
        public Article Article { get; set; }

        /// <summary>
        /// 当前也内容
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; set; }

        private int? pagesize;
        private int? pageno;

        #region IPageable Members
        int IPageable.PageCount
        {
            get { return this.PageCount; }
        }

        int IPageable.PageSize
        {
            get { return pagesize ?? 1; }
        }

        int IPageable.PageNo
        {
            get { return pageno ?? 1; }
        }

        int IPageable.TotalCount
        {
            get { throw new NotImplementedException(); }
        }

        void IPageable.SetPage(int pageNo, int pageSize)
        {
            this.pagesize = pageSize;
            this.pageno = pageNo;
        }
        #endregion
    }
}