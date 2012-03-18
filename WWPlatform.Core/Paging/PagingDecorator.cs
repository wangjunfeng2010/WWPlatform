using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WWPlatform.Core.Utilities;

namespace WWPlatform.Core.Paging
{
    public sealed class PagingDecorator<T> : IEnumerable<T>, IPageable
    {
        private readonly IEnumerable<T> _unpaged;
        private IEnumerable<T> _paged;

        private int? _pageNo;
        private int? _pageSize;
        private int? _totalCount;

        public PagingDecorator(IEnumerable<T> source)
        {
            Contract.ArgumentNotNull(source, "source");

            _unpaged = _paged = source;
        }

        #region IEnumerable<T> Members
        public IEnumerator<T> GetEnumerator()
        {
            try
            {
                return _paged.GetEnumerator();
            }
            catch (NotSupportedException)
            {
                return _unpaged.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region IPageable Members
        public int PageNo
        {
            get { return _pageNo ?? 1; }
        }

        public int PageSize
        {
            get { return _pageSize ?? TotalCount; }
        }

        public int PageCount
        {
            get
            {
                return (int)Math.Ceiling((decimal)TotalCount / PageSize);
            }
        }

        public int TotalCount
        {
            get
            {
                return (int)(_totalCount ?? (_totalCount = _unpaged.Count()));
            }
        }

        public void SetPage(int pageNo, int pageSize)
        {
            Contract.ArgumentPositive(pageNo, "pageNo");
            Contract.ArgumentPositive(pageSize, "pageSize");

            _pageNo = pageNo;
            _pageSize = pageSize;

            _paged = _unpaged
                .AsQueryable()
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize);
        }
        #endregion
    }
}
