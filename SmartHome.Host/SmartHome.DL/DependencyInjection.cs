using SmartHome.DL.Interfaces;
using SmartHome.DL.Repositories;
using SmartHome.Models.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;

namespace SmartHome.DL
{
    public static class DependencyInjection
    {
        public static IServiceCollection
            AddDataLayer(this IServiceCollection services, IConfiguration configs)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

            // Регистрираме конфигурацията и нашето ново репозитори
            services
                .AddConfigurations(configs)
                .AddSingleton<IRoomRepository, RoomMongoRepository>();

            return services;
        }

        private static IServiceCollection
           AddConfigurations(this IServiceCollection services, IConfiguration configs)
        {
            // Това е частта с IOptionsMonitor, която преподавателят изисква
            services.Configure<MongoDbConfiguration>(configs.GetSection(nameof(MongoDbConfiguration)));

            return services;
        }
    }
}