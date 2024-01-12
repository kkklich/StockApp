using System.Collections.Generic;

namespace Netsoftware.Xanthos.Common.Resources.GridResources;

public class GridResponseResource<T> where T : class
{
    public List<T> Rows { get; set; }
    public int ElementsCount { get; set; }
}