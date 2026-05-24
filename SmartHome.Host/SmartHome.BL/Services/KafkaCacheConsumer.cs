using Confluent.Kafka;
using MessagePack;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmartHome.DL.Kafka;
using SmartHome.Models;
using SmartHome.Models.KafkaCache;

namespace SmartHome.BL.Services
{
    public class KafkaCacheConsumer : BackgroundService
    {
        private readonly IConsumer<Ignore, byte[]> _consumer;
        private readonly DatabaseCache _cache;
        private readonly ILogger<KafkaCacheConsumer> _logger;

        public KafkaCacheConsumer(
            DatabaseCache cache,
            IOptionsMonitor<KafkaSettings> optionsMonitor,
            ILogger<KafkaCacheConsumer> logger)
        {
            _cache = cache;
            _logger = logger;
            var settings = optionsMonitor.CurrentValue;

            _logger.LogInformation($"Initializing KafkaCacheConsumer with broker: {settings.BootstrapServers}");

            var config = new ConsumerConfig
            {
                BootstrapServers = settings.BootstrapServers,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.ScramSha256,
                SaslUsername = settings.SaslUsername,
                SaslPassword = settings.SaslPassword,
                EnableSslCertificateVerification = false,
                GroupId = "rooms-cache-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Ignore, byte[]>(config).Build();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Subscribing to rooms-cache topic");
            _consumer.Subscribe("rooms-cache");

            return Task.Run(() =>
            {
                try
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var result = _consumer.Consume(stoppingToken);

                        if (result?.Message?.Value != null && result.Message.Value.Length > 0)
                        {
                            try
                            {
                                var room = MessagePackSerializer.Deserialize<Room>(result.Message.Value);
                                if (room != null)
                                {
                                    _cache.Add(room);
                                    _logger.LogInformation($"Cached room: {room.Id} - {room.Name}");
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError($"Failed to deserialize room: {ex.Message}");
                            }
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Consumer stopped");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Consumer error: {ex.Message}");
                }
            }, stoppingToken);
        }

        public override void Dispose()
        {
            _consumer?.Close();
            _consumer?.Dispose();
            base.Dispose();
        }
    }
}