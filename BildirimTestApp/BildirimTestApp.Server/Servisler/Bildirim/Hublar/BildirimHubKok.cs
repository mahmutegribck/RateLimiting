using BildirimTestApp.Server.Servisler.Kullanici;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace BildirimTestApp.Server.Servisler.Bildirim.Hublar;

[Authorize]
public class BildirimHubKok : Hub
{
    private readonly ILogger<BildirimHubKok> logger;
    private readonly IKullaniciBilgiServisi kullaniciBilgiServisi;
    public static List<int> BagliKullaniciIdler { get; } = new List<int>();

    public BildirimHubKok(
        ILogger<BildirimHubKok> logger,
        IKullaniciBilgiServisi kullaniciBilgiServisi
    )
    {
        this.logger = logger;
        this.kullaniciBilgiServisi = kullaniciBilgiServisi;
    }

    public override async Task OnConnectedAsync()
    {
        var kullaniciAdi = Context.GetHttpContext()!.User?.Identity?.Name;

        if (kullaniciAdi == null)
            throw new Exception("kullanici adı bulunamadı.");

        var kullaniciBilgi = kullaniciBilgiServisi.TryGetKullaniciBilgi(kullaniciAdi);
        if (kullaniciBilgi == null)
            throw new Exception("Kullanici bilgisi bulunamadı");


        lock (BagliKullaniciIdler)
        {
            if (!BagliKullaniciIdler.Contains(kullaniciBilgi.KullaniciId))
                BagliKullaniciIdler.Add(kullaniciBilgi.KullaniciId);
        }
        await Groups.AddToGroupAsync(Context.ConnectionId, kullaniciAdi);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var kullaniciAdi = Context.GetHttpContext()!.User?.Identity?.Name;

        if (kullaniciAdi == null)
        {
            var mesaj = $"kullanici adı bulunamadı. ex.Message: {exception?.Message}";
            logger.LogInformation(mesaj);
            throw new Exception(mesaj);
        }

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, kullaniciAdi);
        //var sisKullanici = kullaniciBilgiServisi.GetKullaniciBilgi(kullaniciAdi).KullaniciId;

        lock (BagliKullaniciIdler)
        {
            BagliKullaniciIdler.Remove(
                kullaniciBilgiServisi.GetKullaniciBilgi(kullaniciAdi).KullaniciId
            );
        }

        await base.OnDisconnectedAsync(exception);
    }
}
