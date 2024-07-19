using BildirimTestApp.Server.Models;

namespace BildirimTestApp.Server.Servisler.Kullanici
{
    public interface IKullaniciBilgiServisi
    {
        SisKullanici GetKullaniciBilgi<T>(T param);
        Task<SisKullanici> GetKullaniciBilgiAsync<T>(T param);

        Task<SisKullanici> TryGetKullaniciBilgiAsync(string kullaniciAdi);

        SisKullanici TryGetKullaniciBilgi(string kullaniciAdi);
    }
}
