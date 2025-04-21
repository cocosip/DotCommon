namespace DotCommon.Timing
{
    public interface ICurrentTimezoneProvider
    {
        string? TimeZone { get; set; }
    }
}
