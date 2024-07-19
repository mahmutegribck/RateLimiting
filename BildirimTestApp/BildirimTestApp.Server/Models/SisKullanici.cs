namespace BildirimTestApp.Server.Models
{
    public partial class SisKullanici
    {
        public SisKullanici()
        {
            SisBildirims = new HashSet<SisBildirim>();
            GorevsGorevs = new HashSet<Gorev>();
        }

        public int KullaniciId { get; set; }
        public string KullaniciAdi { get; set; } = null!;
        public string KullaniciSifresi { get; set; } = null!;
        public string? Rol { get; set; }

        public virtual ICollection<SisBildirim> SisBildirims { get; set; }
        public virtual ICollection<Gorev> GorevsGorevs { get; set; }
    }
}
