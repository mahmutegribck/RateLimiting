using AspNetCoreRateLimit;

namespace BildirimTestApp.Server.Configurators
{
    public class RateLimitingConfigurator
    {
        public static void ConfigureRateLimiting(IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions(); //Bu servis sayesinde appsettings.json içerisindeki key-value çiftlerini bir class üzerinden okuma işlemi gerçekleştirebilecek.

            services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting")); //Konfigürasyon ayarlari
            services.Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies")); //IP kurallari ve politikalari

            //Politika ve verilerin memory’de tutulacagini bildiriyoruz.
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>(); // istek sayılarını izleyerek hız sınırı sayaçlarını bellekte depolar

            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();  //Eşzamanlı istekleri işlemek ve hız sınırlama verilerine erişirken iş parçacığı güvenliğini sağlar
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();//HTTP bağlam bilgilerine erişim sağlar


            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>(); //Tüm yapilanmalari inşa eden servis. Genel hız sınırlama konfigürasyon ayarlarını içerir
        }
    }
}
