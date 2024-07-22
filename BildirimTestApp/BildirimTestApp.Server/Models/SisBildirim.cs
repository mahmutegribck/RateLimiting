namespace BildirimTestApp.Server.Models
{
    public partial class SisBildirim
    {
        public SisBildirim()
        {
            SisBildirimOutboxes = new HashSet<SisBildirimOutbox>();
        }

        public int BildirimId { get; set; }
        public int GonderilecekKullaniciId { get; set; }
        public int GonderimDurumu { get; set; }
        public int BildirimIcerikId { get; set; }
        public bool GorulduDurumu { get; set; }

        public virtual SisBildirimIcerik BildirimIcerik { get; set; } = null!;
        public virtual SisKullanici GonderilecekKullanici { get; set; } = null!;
        public virtual ICollection<SisBildirimOutbox> SisBildirimOutboxes { get; set; }
    }
}
