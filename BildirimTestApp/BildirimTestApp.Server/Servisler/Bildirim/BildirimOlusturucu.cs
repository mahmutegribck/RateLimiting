using BildirimTestApp.Server.Models;
using Newtonsoft.Json;

namespace BildirimTestApp.Server.Servisler.Bildirim;

public class BildirimOlusturucu(TestDbContext testDbContext) : IBildirimOlusturucu
{
    public async Task BildirimGonder<T>(
        IBildirim<T> bildirim,
        IBildirimHedefOlusturucu bildirimHedefOlusturucu,
        string? aciklama = null
    )
    {
        var icerik = new SisBildirimIcerik
        {
            OlusturulanTarih = DateTime.Now,
            Aciklama = aciklama,
            Json = JsonConvert.SerializeObject(
                BildirimSerilestirmeKonteyner.BildirimdenOlustur(bildirim)
            ),
        };
        testDbContext.SisBildirimIceriks.Add(icerik);

        var bildirimler = bildirimHedefOlusturucu.HedefKullaniciIdler.Select(x => new SisBildirim
        {
            BildirimIcerik = icerik,
            GonderilecekKullaniciId = x,
            SisBildirimOutboxes = new SisBildirimOutbox[1] { new SisBildirimOutbox { } },
            GonderimDurumu = 0,
        });
        testDbContext.SisBildirims.AddRange(bildirimler);

        await testDbContext.SaveChangesAsync();
    }

    public async Task BildirimGonder(
        IBildirim bildirim,
        IBildirimHedefOlusturucu bildirimHedefOlusturucu,
        string? aciklama = null
    )
    {
        if (bildirimHedefOlusturucu.HedefKullaniciIdler.Length == 0)
            throw new Exception("Bildirim Gönderilecek Kullanıcı Bulunamadı.");

        var icerik = new SisBildirimIcerik
        {
            OlusturulanTarih = DateTime.Now,
            Aciklama = aciklama,
            Json = JsonConvert.SerializeObject(
                BildirimSerilestirmeKonteyner.BildirimdenOlustur(bildirim)
            ),
        };
        testDbContext.SisBildirimIceriks.Add(icerik);

        var bildirimler = bildirimHedefOlusturucu.HedefKullaniciIdler.Select(x => new SisBildirim
        {
            BildirimIcerik = icerik,
            GonderilecekKullaniciId = x,
            SisBildirimOutboxes = new SisBildirimOutbox[1] { new SisBildirimOutbox { } },
            GonderimDurumu = 0,
            GorulduDurumu = false
        });
        testDbContext.SisBildirims.AddRange(bildirimler);

        await testDbContext.SaveChangesAsync();
    }
}
