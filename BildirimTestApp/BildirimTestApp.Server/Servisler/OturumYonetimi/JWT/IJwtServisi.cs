using BildirimTestApp.Server.Models;
using BildirimTestApp.Server.Servisler.OturumYonetimi.JWT.Token;

namespace BildirimTestApp.Server.Servisler.OturumYonetimi.JWT
{
    public interface IJwtServisi
    {
        JwtToken JwtTokenOlustur(SisKullanici kullanici);

    }
}
