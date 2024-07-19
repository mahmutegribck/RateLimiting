using AutoMapper;
using BildirimTestApp.Server.Models;
using BildirimTestApp.Server.Servisler.Kullanici;
using BildirimTestApp.Server.Servisler.OturumYonetimi.DTO;
using BildirimTestApp.Server.Servisler.OturumYonetimi.JWT;
using BildirimTestApp.Server.Servisler.OturumYonetimi.JWT.Token;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;

namespace BildirimTestApp.Server.Servisler.OturumYonetimi
{
    public class OturumYonetimi : IOturumYonetimi
    {
        private readonly IMapper _mapper;
        private readonly TestDbContext _context;
        private readonly IJwtServisi _jwtServisi;
        private readonly IKullaniciBilgiServisi _kullaniciBilgiServisi;

        public OturumYonetimi(IMapper mapper, TestDbContext context, IJwtServisi jwtServisi, IKullaniciBilgiServisi kullaniciBilgiServisi)
        {
            _mapper = mapper;
            _context = context;
            _jwtServisi = jwtServisi;
            _kullaniciBilgiServisi = kullaniciBilgiServisi;
        }

        public async Task<JwtToken?> GirisYap(KullaniciGirisDto model)
        {
            try
            {
                var sha = SHA256.Create();
                var byteArray = Encoding.Default.GetBytes(model.KullaniciSifresi);
                var hashedSifre = Convert.ToBase64String(sha.ComputeHash(byteArray));

                model.KullaniciSifresi = hashedSifre;
                model.KullaniciAdi = model.KullaniciAdi.Trim().ToLower();

                SisKullanici? kullanici = await _context.SisKullanicis.Where(k => k.KullaniciSifresi == model.KullaniciSifresi && k.KullaniciAdi == model.KullaniciAdi).FirstOrDefaultAsync();
                if (kullanici == null)
                {
                    return null;
                }

                JwtToken token = _jwtServisi.JwtTokenOlustur(kullanici);

                return token;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task KayitOl(KullaniciKayitDto model)
        {
            try
            {
                if (model == null)
                {
                    throw new ArgumentNullException(nameof(model), "Model boş olamaz.");
                }
                var mevcutKullanici = await _context.SisKullanicis.FirstOrDefaultAsync(k => k.KullaniciAdi == model.KullaniciAdi);
                if (mevcutKullanici != null)
                {
                    throw new ArgumentException("Bu kullanıcı adı mevcut.", model.KullaniciAdi);
                }
                model.KullaniciAdi = model.KullaniciAdi.Trim().ToLower();
                SisKullanici yeniKullanici = _mapper.Map<SisKullanici>(model);

                var sha = SHA256.Create();
                var byteArray = Encoding.Default.GetBytes(yeniKullanici.KullaniciSifresi);
                var hashedSifre = Convert.ToBase64String(sha.ComputeHash(byteArray));

                yeniKullanici.KullaniciSifresi = hashedSifre;
                await _context.SisKullanicis.AddAsync(yeniKullanici);
                await _context.SaveChangesAsync();

            }
            catch (ArgumentNullException)
            {
                throw;
            }
        }
    }
}
