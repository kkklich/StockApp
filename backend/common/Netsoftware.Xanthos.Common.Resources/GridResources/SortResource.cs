namespace Netsoftware.Xanthos.Common.Resources.GridResources;

public enum Sort
{
    Asc,
    Desc
}

public class SortResource
{
    public string ColId { get; set; }
    public Sort Sort { get; set; }
}