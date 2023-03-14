using gymControl.Services;

namespace gymControl.Interfaces
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddGymControlInterfaces(this IServiceCollection services)
        {
            services.AddScoped<IPartnerService, PartnerService>();
            services.AddScoped<IUserService, UserService>();
            return services;
        }
        
    }
    
}
