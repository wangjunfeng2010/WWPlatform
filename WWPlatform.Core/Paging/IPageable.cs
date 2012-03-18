
namespace WWPlatform.Core.Paging
{
    public interface IPageable
    {
        //properties
        int PageNo { get; }
        int PageSize { get; }
        int PageCount { get; }
        int TotalCount { get; }

        //methods
        void SetPage(int pageNo, int pageSize);
    }
}
