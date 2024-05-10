namespace DataAccessLayer.Helpers;

public record PagedResult<T>
{
    public List<T> Data { get; }
    public int CurrentPage { get; }
    public int TotalPages { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;
    
    public PagedResult(List<T> data, int count, int pageNumber, int pageSize)
    {
        Data = data;
        TotalCount = count;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
    }
}