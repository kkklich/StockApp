namespace Netsoftware.Xanthos.Common.Resources;

public class SolutionStatsResource
{
    public int Id { get; set; }
    public int SolutionType { get; set; }
    public string SupportType { get; set; }
    public int? FireRate { get; set; }
    public string OpeningType { get; set; }
    public PenetrationItemType PenetrationType { get; set; }
    public string Solution { get; set; }
}