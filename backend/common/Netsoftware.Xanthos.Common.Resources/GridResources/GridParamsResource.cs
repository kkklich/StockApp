using System.Collections.Generic;

namespace Netsoftware.Xanthos.Common.Resources.GridResources;

public class GridParamsResource
{
    public int StartRow { get; set; }
    public int EndRow { get; set; }
    public Dictionary<string, FiltersResource> FilterModel { get; set; }
    public List<SortResource> SortModel { get; set; }
}