using Microsoft.AspNetCore.RateLimiting;

namespace BildirimTestApp.Server.Configurators
{
    public class RateLimiterConfigurator
    {
        public static void ConfigureRateLimiter(IServiceCollection services)
        {

            services.AddRateLimiter(options =>
            {
                //Her 10 saniyede 5 request işlenir 2 tanesi kuyruğa alınır daha fazlası boşa çıkar bir sonraki 10 saniyede kuyruktaki requestlerden eskiden yeniye başlanır.
                options.AddFixedWindowLimiter("Fixed", _options =>
                {
                    _options.Window = TimeSpan.FromSeconds(10); //Her 10 saniyede bir politika geçerli olur
                    _options.PermitLimit = 5; //5 request hakkı
                    _options.QueueLimit = 2; //2 tane kuyruğa alınır. 7 requestden sonrası boşa çıkar.
                    _options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst; //Kuyruktaki requestler eskiden yeniye işlenir.


                });
                options.OnRejected = (context, cancellationToken) =>
                {
                    //rate limite takılan istek olunca çalışır.
                    return new();
                };
            });

            services.AddRateLimiter(options =>
            {
                //Fixed ile aynı çalışır ama periyodun(10 saniye) yarısından sonra request hakkından fazla gelen istekleri bir sonraki periyodun request hakkından belirtilen miktar kadar request işler.
                options.AddSlidingWindowLimiter("SlidingWindow", _options =>
                {
                    _options.Window = TimeSpan.FromSeconds(10); //Her 10 saniyede bir politika geçerli olur
                    _options.PermitLimit = 5; //5 request hakkı
                    _options.QueueLimit = 2; //2 tane kuyruğa alınır. 7 requestden sonrası boşa çıkar.
                    _options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst; //Kuyruktaki requestler eskiden yeniye işlenir.
                    _options.SegmentsPerWindow = 1; //Her periyotta bir sonraki periyottan 1 tane request hakkı alabilir.


                });
                options.OnRejected = (context, cancellationToken) =>
                {
                    //rate limite takılan istek olunca çalışır.
                    return new();
                };
            });

            services.AddRateLimiter(options =>
            {
                //Her periyotta işlenecek request sayısı kadar token üretilmektedir. Eğer ki bu tokenler kullanıldıysa diğer periyottan borç alınabilir. Lakin her periyotta token üretim miktarı kadar token üretilecek ve bu şekilde rate limit uygulanacaktır. her Periyotun max token limiti verilen sabit sayı kadar olacaktır.
                options.AddTokenBucketLimiter("TokenBucket", _options =>
                {
                    _options.ReplenishmentPeriod = TimeSpan.FromSeconds(10); //Her 10 saniyede bir politika geçerli olur
                    _options.TokenLimit = 5;// Token limiti
                    _options.TokensPerPeriod = 5; //Her periyotta üretilecek token limiti TokenLimit ile aynı olmalıdır.
                    _options.QueueLimit = 2; //2 tane kuyruğa alınır. 7 requestden sonrası boşa çıkar.
                    _options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst; //Kuyruktaki requestler eskiden yeniye işlenir.



                });
                options.OnRejected = (context, cancellationToken) =>
                {
                    //rate limite takılan istek olunca çalışır.
                    //Log
                    return new();
                };
            });

            services.AddRateLimiter(options =>
            {
                //Sadece asenkron çalışan requestleri sınırlandırmak için kullanılır. Sadece asenkron endpointleri sınırlandırır. Her istek limitin sayısını bir azaltır. Asenkron işlem bitince limit sayısının bir arttırır.
                options.AddConcurrencyLimiter("Concurrency", _options =>
                {
                    _options.PermitLimit = 10; //5 request hakkı
                    _options.QueueLimit = 2; //2 tane kuyruğa alınır. 7 requestden sonrası boşa çıkar.
                    _options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst; //Kuyruktaki requestler eskiden yeniye işlenir.

                });
                options.OnRejected = (context, cancellationToken) =>
                {
                    //rate limite takılan istek olunca çalışır.
                    return new();
                };

            });
        }
    }
}
