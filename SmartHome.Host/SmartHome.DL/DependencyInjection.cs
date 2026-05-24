using SmartHome.DL.DbCacheReader;
using SmartHome.DL.Infrastructure.HostedServices;
using SmartHome.DL.Interfaces;
using SmartHome.DL.Kafka;
using SmartHome.DL.Repositories;
using SmartHome.Models;
using SmartHome.Models.Configurations;
using SmartHome.Models.Responses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace SmartHome.DL
{
    public static class DependencyInjection
    {
        public static IServiceCollection
            AddDataLayer(this IServiceCollection services, IConfiguration configs)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

            services
                .AddHostedService<HostedWorker>()
                .AddConfigurations(configs)
                .AddSingleton<IRoomRepository, RoomMongoRepository>();

            return services;
        }

        private static IServiceCollection
           AddConfigurations(this IServiceCollection services, IConfiguration configs)
        {
            services.Configure<MongoDbConfiguration>(configs.GetSection(nameof(MongoDbConfiguration)));

            // Чете KafkaSettings от appsettings.json
            var kafkaSettings = configs.GetSection(nameof(KafkaSettings)).Get<KafkaSettings>()
                ?? throw new InvalidOperationException("KafkaSettings missing from appsettings.json");

            services.AddSingleton(kafkaSettings);
            services.Configure<KafkaSettings>(configs.GetSection(nameof(KafkaSettings)));

            // Producer за бизнес събитията (RoomActivityResult)
            services.AddSingleton(sp => new GenericKafkaProducer<string, RoomActivityResult>(kafkaSettings));

            // Producer за кеша на стаите (върви на отделен topic)
            var cacheKafkaSettings = new KafkaSettings
            {
                BootstrapServers = kafkaSettings.BootstrapServers,
                SaslUsername = kafkaSettings.SaslUsername,
                SaslPassword = kafkaSettings.SaslPassword,
                Topic = configs["DbCacheReader:Topic"] ?? "rooms-cache",
                GroupId = kafkaSettings.GroupId
            };
            services.AddSingleton(sp => new GenericKafkaProducer<string, Room>(cacheKafkaSettings));

            // Consumer worker за бизнес събитията
            services.AddHostedService<KafkaConsumerWorker>();

            // DB cache reader (всеки 60 сек чете всички стаи и ги пуска в Kafka)
            services.AddSingleton<IDbCacheReaderService, DbCacheReaderService>();
            services.AddHostedService<DbCacheReaderBackgroundWorker>();

            return services;
        }
    }
}