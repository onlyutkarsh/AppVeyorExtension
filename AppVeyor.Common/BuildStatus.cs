namespace AppVeyor.Common
{
    public enum BuildStatus
    {
        Running,
        Success,
        Queued,
        Failed,
        Cancelled,
        Unknown,
    }
}