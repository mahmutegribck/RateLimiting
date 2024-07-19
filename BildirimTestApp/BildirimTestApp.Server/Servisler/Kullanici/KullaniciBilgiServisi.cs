using BildirimTestApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace BildirimTestApp.Server.Servisler.Kullanici
{
    public class KullaniciBilgiServisi : IKullaniciBilgiServisi
    {
        private readonly ILogger<KullaniciBilgiServisi> _logger;

        public KullaniciBilgiServisi(ILogger<KullaniciBilgiServisi> logger)
        {
            _logger = logger;
        }

        public SisKullanici GetKullaniciBilgi<T>(T param)
        {
            try
            {
                if (param == null)
                    throw new ArgumentException("Parametre geçerli değil.");

                using (var context = new TestDbContext())
                {
                    if (param is int kullaniciID)
                    {
                        return context.SisKullanicis
                            .SingleOrDefault(k => k.KullaniciId == kullaniciID)
                            ?? throw new Exception("Kullanıcı bulunamadı.");
                    }
                    else if (param is string kullaniciAdi)
                    {
                        return context.SisKullanicis
                            .SingleOrDefault(k => k.KullaniciAdi == kullaniciAdi)
                            ?? throw new Exception("Kullanıcı bulunamadı.");
                    }
                    else
                    {
                        throw new ArgumentException("Geçersiz parametre türü.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Kullanıcı bilgisi getirilirken bir hata oluştu.");
                throw;
            }
        }

        public async Task<SisKullanici> GetKullaniciBilgiAsync<T>(T param)
        {
            try
            {
                if (param == null)
                    throw new ArgumentException("Parametre geçerli değil.");

                using (var context = new TestDbContext())
                {
                    if (param is int kullaniciID)
                    {
                        return await context.SisKullanicis
                            .SingleOrDefaultAsync(k => k.KullaniciId == kullaniciID)
                            ?? throw new Exception("Kullanıcı bulunamadı.");
                    }
                    else if (param is string kullaniciAdi)
                    {
                        return await context.SisKullanicis
                            .SingleOrDefaultAsync(k => k.KullaniciAdi == kullaniciAdi)
                            ?? throw new Exception("Kullanıcı bulunamadı.");
                    }
                    else
                    {
                        throw new ArgumentException("Geçersiz parametre türü.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Kullanıcı bilgisi getirilirken bir hata oluştu.");
                throw;
            }
        }


        public async Task<SisKullanici> TryGetKullaniciBilgiAsync(string kullaniciAdi)
        {
            try
            {
                if (kullaniciAdi == null)
                    throw new Exception("Kullanici Adi Bulunamadi.");

                using (var context = new TestDbContext())
                {
                    var sisKullanici = await context.SisKullanicis
                        .SingleOrDefaultAsync(k => k.KullaniciAdi == kullaniciAdi);

                    return sisKullanici ?? throw new Exception("Kullanici Bulunamadi.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Kullanıcı bilgisi getirilirken bir hata oluştu.");
                throw;
            }
        }

        public SisKullanici TryGetKullaniciBilgi(string kullaniciAdi)
        {
            try
            {
                if (kullaniciAdi == null)
                    throw new Exception("Kullanici Adi Bulunamadi.");

                using (var context = new TestDbContext())
                {
                    var sisKullanici = context.SisKullanicis.SingleOrDefault(k => k.KullaniciAdi == kullaniciAdi);

                    if (sisKullanici == null)
                        throw new Exception("Kullanici Bulunamadi.");

                    return sisKullanici;
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Kullanıcı bilgisi getirilirken bir hata oluştu.");
                throw;
            }
        }
    }
}
