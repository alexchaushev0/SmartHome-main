using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SmartHome.DL.Infrastructure.HostedServices
{
    public class HostedWorker : IHostedService
    {
        private readonly ILogger<HostedWorker> _logger;

        public HostedWorker(ILogger<HostedWorker> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    _logger.LogInformation($"SmartHome HostedWorker tick: {DateTime.UtcNow}");
                    await Task.Delay(5000, cancellationToken);
                }
            }, cancellationToken);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}