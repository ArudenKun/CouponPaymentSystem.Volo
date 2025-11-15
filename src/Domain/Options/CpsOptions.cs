using Humanizer;

namespace Domain.Options;

public class CpsOptions
{
    public required string SysId { get; init; }
    public int RetentionDays { get; init; }
    public string FileNamePattern { get; init; } = "^\\d{4}.*";
    public IReadOnlyList<string> FileExtensions { get; init; } = ["xls", "xlsx"];
    public int FileMaxSizeMb { get; init; } = 10;
    public string MakerRole { get; init; } = "CPS_MAKER";
    public string CheckerRole { get; init; } = "CPS_CHECKER";
    public string PingUrl { get; init; } = "";
    public long PingIntervalMs { get; init; } = (long)5.Minutes().TotalMilliseconds;
    public required DatabaseOptions Aso { get; init; }
    public required DatabaseOptions Cps { get; init; }
}
