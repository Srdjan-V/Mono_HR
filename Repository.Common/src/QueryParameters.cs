namespace Mono.Repository.Common;

public class QueryParameters
{
    private const int MaxPageCount = 50;
    public int Page { get; set; } = 1;

    private int _pageCount = MaxPageCount;

    public int PageCount
    {
        get => _pageCount;
        set => _pageCount = (value > MaxPageCount) ? MaxPageCount : value;
    }

    public string? Query { get; set; } = "";

    public string OrderBy { get; set; } = "Name";


    public double GetTotalPages(int totalCount)
    {
        return Math.Ceiling(totalCount / (double)PageCount);
    }

    public bool HasQuery()
    {
        return !string.IsNullOrEmpty(Query);
    }

    public bool IsDescending()
    {
        if (!string.IsNullOrEmpty(OrderBy))
        {
            return OrderBy.Split(' ').Last().StartsWith("desc", StringComparison.InvariantCultureIgnoreCase);
        }

        return false;
    }
}