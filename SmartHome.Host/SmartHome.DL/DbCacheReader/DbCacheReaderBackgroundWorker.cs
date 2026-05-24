using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace SmartHome.DL.DbCacheReader
{
    public class DbCacheReaderBackgroundWorker : BackgroundService
    {
        private readonly IDbCacheReaderService _service;
        private readonly int _intervalSeconds;

        public DbCacheReaderBackgroundWorker(IDbCacheReaderService service, IConfiguration config)
        {
            _service = service;
            _intervalSeconds = config.GetValue<int>("DbCacheReader:IntervalSeconds", 60);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _service.ReadAndPublishAsync(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(_intervalSeconds), stoppingToken);
                await _service.ReadAndPublishAsync(stoppingToken);
            }
        }
    }
}