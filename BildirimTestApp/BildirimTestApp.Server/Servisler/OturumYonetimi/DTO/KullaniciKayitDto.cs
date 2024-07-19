using System.ComponentModel.DataAnnotations;

namespace BildirimTestApp.Server.Servisler.OturumYonetimi.DTO
{
    public class KullaniciKayitDto
    {
        [Required(ErrorMessage = "Isim zorunlu")]
        public required string KullaniciAdi { get; set; }


        [Required(ErrorMessage = "sifre zorunlu")]
        [DataType(DataType.Password)]
        public required string KullaniciSifresi { get; set; }

        [Required(ErrorMessage = "Şifre tekrarı zorunlu")]
        [DataType(DataType.Password)]
        [Compare("KullaniciSifresi", ErrorMessage = "Girmiş olduğunuz parola birbiri ile eşleşmiyor.")]
        public required string KullaniciSifresiTekrar { get; set; }
    }
}
