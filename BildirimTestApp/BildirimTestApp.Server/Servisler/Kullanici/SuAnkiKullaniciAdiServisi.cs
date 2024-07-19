namespace BildirimTestApp.Server.Servisler.Kullanici
{
    public interface ISuAnkiKullaniciAdiServisi
    {
        public static string? SKullaniciAdiEditor { get; set; }
        string? KullaniciAdi { get; }
    }

    public class SuAnkiKullaniciAdiServisi(IHttpContextAccessor httpContextAccessor)
        : ISuAnkiKullaniciAdiServisi
    {
        private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;

        public string? KullaniciAdi => httpContextAccessor.HttpContext?.User?.Identity?.Name;
    }
}
