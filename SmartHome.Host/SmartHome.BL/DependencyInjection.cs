using SmartHome.BL.Interfaces;
using SmartHome.BL.Services;
using SmartHome.Models.KafkaCache;
using Microsoft.Extensions.DependencyInjection;

namespace SmartHome.BL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLayer(this IServiceCollection services)
        {
            services.AddSingleton<IRoomService, RoomService>();
            services.AddSingleton<IDeviceService, DeviceService>();
            services.AddScoped<ISmartHomeManager, SmartHomeManager>();

            // Ново: бизнес действието
            services.AddSingleton<IProcessRoomActivity, ProcessRoomActivity>();

            // Ново: in-memory кеш на стаите + consumer-а, който го пълни
            services.AddSingleton<DatabaseCache>();
            services.AddHostedService<KafkaCacheConsumer>();

            return services;
        }
    }
}