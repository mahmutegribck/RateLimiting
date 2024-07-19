namespace BildirimTestApp.Server.DTOs
{
    public class GorevOlusturmaDTO
    {
        public string? Aciklama { get; set; }
        public DateTime GorevSonTarih { get; set; }
        public List<int>? GonderilecekKullaniciIdleri { get; set; }

    }
}
