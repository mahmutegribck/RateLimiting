using BildirimTestApp.Server.Servisler.OturumYonetimi.DTO;
using BildirimTestApp.Server.Servisler.OturumYonetimi.JWT.Token;

namespace BildirimTestApp.Server.Servisler.OturumYonetimi
{
    public interface IOturumYonetimi
    {
        Task KayitOl(KullaniciKayitDto model);

        Task<JwtToken?> GirisYap(KullaniciGirisDto model);

    }
}
