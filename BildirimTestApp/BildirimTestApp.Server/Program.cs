using System.Text;
using AspNetCoreRateLimit;
using BildirimTestApp.Server.Configurators;
using BildirimTestApp.Server.Models;
using BildirimTestApp.Server.RateLimitingMiddleware;
using BildirimTestApp.Server.Servisler;
using BildirimTestApp.Server.Servisler.Bildirim;
using BildirimTestApp.Server.Servisler.Bildirim.BildirimGoruldu;
using BildirimTestApp.Server.Servisler.Kullanici;
using BildirimTestApp.Server.Servisler.KullaniciGorev;
using BildirimTestApp.Server.Servisler.Mail;
using BildirimTestApp.Server.Servisler.OturumYonetimi;
using BildirimTestApp.Server.Servisler.OturumYonetimi.JWT;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using static BildirimTestApp.Server.Servisler.Bildirim.BildirimAyarla;

namespace BildirimTestApp.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddHttpContextAccessor();

        SwaggerConfigurator.ConfigureSwaggerGen(builder.Services);

        //RateLimitingConfigurator.ConfigureRateLimiting(builder.Services, builder.Configuration);             //Hazir kutuphane ile rate limit uygulamsi.
        //RateLimiterConfigurator.ConfigureRateLimiter(builder.Services);                                      //Hazir kutuphane ile rate limit uygulamsi.

        builder.Services.AddDbContext<TestDbContext>();
        builder.Services.BildirimEkle(builder.Environment.IsDevelopment(), builder.Configuration);


        builder.Services.AddTransient<ISuAnkiKullaniciAdiServisi, SuAnkiKullaniciAdiServisi>();
        builder.Services.AddScoped<IBildirimHedefOlusturucu, BildirimHedefOlusturucu>();
        builder.Services.AddScoped<IOturumYonetimi, OturumYonetimi>();
        builder.Services.AddScoped<IJwtServisi, JwtServisi>();
        builder.Services.AddScoped<IGorevServisi, GorevServisi>();
        builder.Services.AddScoped<IBildirimGoruldu, BildirimGoruldu>();

        builder.Services.AddSingleton<IKullaniciBilgiServisi, KullaniciBilgiServisi>();
        builder.Services.AddSingleton<IMailServisi, MailServisi>();

        builder.Services.AddTransient<RateLimit>();
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        builder.Services.AddMemoryCache();

        builder
            .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,

                    ValidAudience = builder.Configuration["JWT:Audience"],
                    ValidIssuer = builder.Configuration["JWT:Issuer"],

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"] ?? string.Empty)
                    ),
                    LifetimeValidator = (notBefore, expires, securityToken, validationParameters) =>
                        expires != null ? expires > DateTime.UtcNow : false
                };
                options.Events = new JwtBearerEvents();
            });

        builder.Services.AddTransient<IClaimsTransformation, ClaimsTransformerGelistirici>();

        builder.Services.AddTransient<IPostConfigureOptions<JwtBearerOptions>, SignalRJwtTokenAyarlaPostConf>();


        var app = builder.Build();

        app.UseDefaultFiles();
        app.UseStaticFiles();
        app.UseHttpsRedirection();

        //app.UseIpRateLimiting();              //Hazir kutuphane ile rate limit uygulamsi.
        //app.UseRateLimiter();                 //Hazir kutuphane ile rate limit uygulamsi.

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.BildirimKullan();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.MapFallbackToFile("/index.html");

        app.RateLimit();                    //Middleware yazilarak rate limit uygulamsi.

        app.Run();
    }
}
