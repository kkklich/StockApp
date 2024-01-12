using System;

namespace Netsoftware.Xanthos.Common.Resources.GridResources;

public enum Operator
{
    AND,
    OR
}

public enum Type
{
    Contains,
    NotContains,
    Equals,
    NotEqual,
    StartsWith,
    EndsWith,
    GreaterThan,
    GreaterThanOrEqual,
    LessThan,
    LessThanOrEqual,
    InRange
}

public enum FilterType
{
    Text,
    Number,
    Date
}

public class FiltersResource
{
    public FilterType FilterType { get; set; }
    public Type Type { get; set; }
#nullable enable
    public DateTime? DateFrom { get; set; }

    public DateTime? DateTo { get; set; }

    // Filter can be integer or string
    public object? Filter { get; set; }

    // FilterTo property is used for number filter
    public int? FilterTo { get; set; }
    public Operator? Operator { get; set; }
    public FiltersResource? Condition1 { get; set; }
    public FiltersResource? Condition2 { get; set; }
#nullable disable
}