using System.Collections.Generic;

namespace Netsoftware.Xanthos.Common.Resources;

public class ProjectStatsResource
{
    public int Id { get; set; }
    public string Name { get; set; }

    public string City { get; set; }
    public string Address { get; set; }
    public string State { get; set; }
    public string Region { get; set; }
    public string Country { get; set; }

    public string Location { get; set; }

    public ICollection<SolutionStatsResource> Solutions { get; set; }
}