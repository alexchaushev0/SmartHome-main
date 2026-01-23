using SmartHome.BL.Interfaces;
using SmartHome.BL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace SmartHome.BL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLayer(this IServiceCollection services)
        {
            
            services.AddSingleton<IRoomService, RoomService>();
            services.AddScoped<IDeviceService, DeviceService>();
            services.AddScoped<ISmartHomeManager, SmartHomeManager>();
            return services;
        }
    }
}