using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BildirimTestApp.Server.Models;
using BildirimTestApp.Server.Servisler.Bildirim.Hublar;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;

namespace BildirimTestApp.Server.Servisler.Bildirim;

public static class BildirimAyarla
{
    public static IServiceCollection BildirimEkle(
        this IServiceCollection services,
        bool isDev,
        IConfiguration configuration
    )
    {
        services.AddSignalR();
        services.AddTransient<AnlikBildirimHub>();
        services.AddTransient<IBildirimOlusturucu, BildirimOlusturucu>();
        services.AddHostedService<PeriyodikBildirimOkuyucu>();

        //services.AddTransient<PeriyodikBildirimOkuyucu>();
       
        services.AddSingleton<IUserIdProvider, NameUserIdProvider>();

        return services;
    }

    public static void BildirimKullan(this WebApplication app)
    {
        app.UseCors("signalRCors");
        app.UseCors(
           x =>
              x.AllowAnyMethod()
                 .AllowAnyHeader()
                 .SetIsOriginAllowed(origin => true) // allow any origin
                 .AllowCredentials()
        ); // allow credentials
        app.UseWebSockets();
        app.MapHub<AnlikBildirimHub>("/hub/anlikBildirim");
    }

    public class NameUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return connection.User?.Identity?.Name;
#pragma warning restore CS8603 // Possible null reference return.
        }
    }

    public class SignalRJwtTokenAyarlaPostConf : IPostConfigureOptions<JwtBearerOptions>
    {
        public void PostConfigure(string? name, JwtBearerOptions options)
        {
            var originalOnMessageReceived = options.Events.OnMessageReceived;
            options.Events.OnMessageReceived = async context =>
            {
                await originalOnMessageReceived(context);

                //if (!string.IsNullOrEmpty(context.Token) || !context.HttpContext.Request.Path.StartsWithSegments(HubSabitler.hubBaslangicAdresi))
                //   return;

                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;

                if (string.IsNullOrEmpty(accessToken))
                    return;

                var token = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
                var identity = new ClaimsPrincipal(new ClaimsIdentity(token.Claims));

                context.Token = accessToken;
            };
        }
    }
}
