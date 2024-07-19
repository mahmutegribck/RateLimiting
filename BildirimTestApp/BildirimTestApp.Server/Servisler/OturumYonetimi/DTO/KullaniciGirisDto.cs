using System.ComponentModel.DataAnnotations;

namespace BildirimTestApp.Server.Servisler.OturumYonetimi.DTO
{
    public class KullaniciGirisDto
    {
        [Required(ErrorMessage = "Isim zorunlu")]
        public required string KullaniciAdi { get; set; }


        [Required(ErrorMessage = "Sifre zorunlu")]
        [DataType(DataType.Password)]
        public required string  KullaniciSifresi { get; set; }
    }
}
