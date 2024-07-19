namespace BildirimTestApp.Server.Servisler.Mail.DTO
{
    public class MailDto
    {
        public List<string>? GonderilecekKullaniciMailleri { get; set; }
        public string? Baslik { get; set; }
        public string? Aciklama { get; set; }
    }
}
