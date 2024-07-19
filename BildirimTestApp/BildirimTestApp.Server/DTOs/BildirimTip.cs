using BildirimTestApp.Server.Servisler.Bildirim;

namespace BildirimTestApp.Server.DTOs
{
    public class AnlikBildirim : IAnlikBildirim
    {
        public string? Aciklama { get; set; }

    }

    public class GorevAtandiAnlikBildirim : IAnlikBildirim
    {
        public string? Aciklama { get; set; }
        public DateTime GorevSonTarih { get; set; }

    }

    public class EtkinlikDuyuruBildirim : IDuyuruBildirim
    {
        public string? Aciklama { get; set; }
        public DateTime EtkinlikZamani { get; set; }
        public string? EtkinlikKonumu { get; set; }
    }

    public class ToplantiDuyuruBildirim : IDuyuruBildirim
    {
        public string? BirimAdi { get; set; }
        public string? Aciklama { get; set; }
        public string? ToplantiKonumu { get; set; }
        public DateTime ToplantiZamani { get; set; }

    }

    public class YemekhaneDuyuruBildirim : IDuyuruBildirim
    {
        public string? Aciklama { get; set; }
        public DateTime YemekZamani { get; set; }

    }


    public class EpostaBildirim : IEPostaBildirim
    {
        public string? Aciklama { get; set; }
        public string? Baslik { get; set; }

    }
}
