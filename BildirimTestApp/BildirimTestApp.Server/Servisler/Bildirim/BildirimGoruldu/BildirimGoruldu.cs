
using BildirimTestApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace BildirimTestApp.Server.Servisler.Bildirim.BildirimGoruldu
{
    public class BildirimGoruldu : IBildirimGoruldu
    {
        private readonly TestDbContext _context;
        private readonly ILogger<BildirimGoruldu> _logger;
        public BildirimGoruldu(TestDbContext context, ILogger<BildirimGoruldu> logger)
        {
            _context = context;
            _logger = logger;

        }

        //Kullanici ve bildirim idleri alinir. Bildirim goruldu durumu setlenir.
        public async Task SetBildirimGoruldu(int[] bildirimIDs, string kullaniciAdi)
        {
            var bildirimler = await _context.SisBildirims
                .Where(b => bildirimIDs.Contains(b.BildirimId) && b.GonderilecekKullanici.KullaniciAdi == kullaniciAdi && b.GorulduDurumu == false)
                .ToListAsync();

            if (!bildirimler.Any()) throw new ArgumentException("Kullanıcı Bildirimi Bulunamadı.");

            if (bildirimler.Count != bildirimIDs.Length) _logger.LogInformation("Geçersiz veya Görülmüş Bildirim ID'leri Bulunuyor");


            foreach (var bildirim in bildirimler)
            {
                bildirim.GorulduDurumu = true;
            }

            await _context.SaveChangesAsync();

        }
    }
}
