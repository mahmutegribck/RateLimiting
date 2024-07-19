using Newtonsoft.Json;
using System.ComponentModel;

namespace BildirimTestApp.Server.DTOs.BildirimDTO
{
    public class ToplantiDuyuruBildirimDTO
    {
        public string? BirimAdi { get; set; }
        public string? Aciklama { get; set; }
        public string? ToplantiKonumu { get; set; }
        public DateTime ToplantiZamani { get; set; }

        public List<int>? GonderilecekKullaniciIdleri { get; set; }
    }
}
