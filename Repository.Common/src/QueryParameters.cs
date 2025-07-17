using System.Reflection;

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

    public string OrderBy { get; set; } = "";

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

    //audit idk how efficient reflection is in c# 
    public IComparer<T>? CreateComparer<T>(
        List<PropertyInfo?> allowedProperties,
        PropertyInfo? defaultProperty
    )
    {
        ArgumentNullException.ThrowIfNull(defaultProperty);
        if (allowedProperties.Contains(null))
        {
            throw new ArgumentException($"{nameof(allowedProperties)} must not contain null value");
        }

        PropertyInfo? property;
        if (string.IsNullOrWhiteSpace(OrderBy))
        {
            property = defaultProperty;
        }
        else
        {
            property = allowedProperties.FirstOrDefault(p =>
                string.Equals(p.Name, OrderBy, StringComparison.OrdinalIgnoreCase));
        }

        if (property == null)
        {
            return null;
        }

        if (!typeof(IComparable).IsAssignableFrom(property.PropertyType))
        {
            throw new ArgumentException(
                $"Property {property.Name} of type {property.PropertyType.Name} does not implement IComparable");
        }

        var descending = IsDescending();
        return Comparer<T>.Create((x, y) =>
        {
            var xValue = (IComparable)property.GetValue(x);
            var yValue = (IComparable)property.GetValue(y);

            if (xValue == null && yValue == null) return 0;
            if (xValue == null) return descending ? 1 : -1;
            if (yValue == null) return descending ? -1 : 1;

            int result = xValue.CompareTo(yValue);
            return descending ? -result : result;
        });
    }
}