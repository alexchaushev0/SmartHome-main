using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SmartHome.DL.Kafka;
using SmartHome.Models.Responses;

namespace SmartHome.DL.Infrastructure.HostedServices
{
    public class KafkaConsumerWorker : BackgroundService
    {
        private readonly ILogger<KafkaConsumerWorker> _logger;
        private readonly KafkaSettings _kafkaSettings;

        public KafkaConsumerWorker(ILogger<KafkaConsumerWorker> logger, KafkaSettings kafkaSettings)
        {
            _logger = logger;
            _kafkaSettings = kafkaSettings;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Kafka Consumer Worker is starting in the background.");

            return Task.Run(() =>
            {
                var consumer = new GenericKafkaConsumer<string, RoomActivityResult>(
                    _kafkaSettings,
                    onMessageReceived: (key, receivedModel) =>
                    {
                        string roomName = receivedModel.Room != null
                            ? receivedModel.Room.Name
                            : "Unknown Room";

                        _logger.LogInformation(
                            "\n====== NEW ROOM ACTIVITY RECEIVED FROM KAFKA ======\n" +
                            "Kafka Key:      {Key}\n" +
                            "Room:           {RoomName}\n" +
                            "Active Devices: {ActiveDevices}\n" +
                            "Energy (kWh):   {Energy}\n" +
                            "Processed At:   {ProcessedAt}\n" +
                            "====================================================",
                            key, roomName, receivedModel.ActiveDevicesCount,
                            receivedModel.EstimatedEnergyUsage, receivedModel.ProcessedAt);
                    }
                );

                consumer.StartConsuming(stoppingToken);

                _logger.LogInformation("Kafka Consumer Worker has cleanly stopped.");
            }, stoppingToken);
        }
    }
}