namespace BildirimTestApp.Server.DTOs.BildirimDTO
{
    public class GorevAtandiAnlikBildirimDTO
    {
        public string? Aciklama { get; set; }
        public DateTime GorevSonTarih { get; set; }

        public List<int>? GonderilecekKullaniciIdleri { get; set; }

    }
}
