namespace SmartHome.DL.DbCacheReader
{
    public interface IDbCacheReaderService
    {
        Task ReadAndPublishAsync(CancellationToken cancellationToken);
    }
}