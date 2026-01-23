using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using SmartHome.Models.Configurations;
using MongoDB.Bson;

namespace SmartHome.Host.HealthChecks
{
    public class MongoHealthCheck : IHealthCheck
    {
        private readonly IMongoDatabase _database;

        public MongoHealthCheck(IOptionsMonitor<MongoDbConfiguration> settings)
        {
            var client = new MongoClient(settings.CurrentValue.ConnectionString);
            _database = client.GetDatabase(settings.CurrentValue.DatabaseName);
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                
                await _database.RunCommandAsync((Command<BsonDocument>)"{ping:1}", cancellationToken: cancellationToken);

                return HealthCheckResult.Healthy("MongoDB е свързана успешно.");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy($"Грешка при връзка с MongoDB: {ex.Message}");
            }
        }
    }
}