using BildirimTestApp.Server.Servisler.Kullanici;
using System.Collections.Concurrent;

namespace BildirimTestApp.Server.RateLimitingMiddleware
{
    public static class MuafKullanicilarListesi
    {
        public static List<string> MuafKullanicilar { get; } = new List<string>(); // Rate limit uygulanmayacak kullanıcılar
    }

    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RateLimit _rateLimiter;
        private readonly ISuAnkiKullaniciAdiServisi suAnkiKullaniciAdiServisi;

        public RateLimitingMiddleware(
            RequestDelegate next,
            RateLimit rateLimiter,
            ISuAnkiKullaniciAdiServisi suAnkiKullaniciAdiServisi
        )
        {
            _next = next;
            _rateLimiter = rateLimiter;
            this.suAnkiKullaniciAdiServisi = suAnkiKullaniciAdiServisi;
        }

        public async Task Invoke(HttpContext context)
        {
            var kullaniciAdi = suAnkiKullaniciAdiServisi.KullaniciAdi;

            if (kullaniciAdi != null && !MuafKullanicilarListesi.MuafKullanicilar.Contains(kullaniciAdi))
            {
                if (!_rateLimiter.CheckRateLimit(kullaniciAdi))
                {
                    context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    await context.Response.WriteAsync("Too many requests. Please try again later.");
                    return;
                }
            }

            await _next(context);
        }
    }

    public class RateLimit
    {
        //her bir kullanıcının istek sayısını ve son istek zamanını takip eder.
        private readonly ConcurrentDictionary<string, (long, long)> _requestCount;

        public RateLimit()
        {
            _requestCount = new ConcurrentDictionary<string, (long, long)>();
        }

        public bool CheckRateLimit(string kullaniciAdi)
        {
            //Anlık tarih
            var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            if (!_requestCount.ContainsKey(kullaniciAdi))
            {
                _requestCount.TryAdd(kullaniciAdi, (1, currentTime));//kullanici adi, request sayisi(1) ve istek anındaki zaman değeri 
            }
            else
            {
                var (requestCount, lastRequestTime) = _requestCount[kullaniciAdi];
                if (currentTime - lastRequestTime <= 60) // 60 saniyede en fazla 5 istek olarak ayarlandi. Istek sayisi opsiyonel.
                {
                    if (requestCount > 5)
                    {
                        return false;
                    }
                    _requestCount[kullaniciAdi] = (requestCount + 1, currentTime);//belirlenene zaman araligi icinde iste gelirse istek sayisi 1 artar
                }
                else
                {
                    _requestCount[kullaniciAdi] = (1, currentTime);//belirlenen zaman araligini disinda istek atilirsa istek sayisi 1 de kalir
                }
            }

            return true;
        }
    }

    public static class RateLimitingMiddlewareExtensions
    {
        public static IApplicationBuilder RateLimit(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RateLimitingMiddleware>();
        }
    }
}
