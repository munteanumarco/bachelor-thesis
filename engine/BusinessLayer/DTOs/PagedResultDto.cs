namespace BusinessLayer.DTOs;

public record PagedResultDto<T>
{
    public List<T> Data { get; init; }
    public int CurrentPage { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
    public int TotalCount { get; init; }
    public bool HasPrevious { get; init; }
    public bool HasNext { get; init; }
}