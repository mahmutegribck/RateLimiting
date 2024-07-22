using BildirimTestApp.Server.Servisler.Kullanici;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BildirimTestApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    //[EnableRateLimiting("Concurrency")]

    public class KullaniciController : ControllerBase
    {
        private readonly IKullaniciBilgiServisi _kullaniciBilgiServisi;

        public KullaniciController(
            IKullaniciBilgiServisi kullaniciBilgiServisi
        )
        {
            _kullaniciBilgiServisi = kullaniciBilgiServisi;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetKullanici([FromQuery] int kullaniciId)
        {
            var sisKullanici = await _kullaniciBilgiServisi.GetKullaniciBilgiAsync(kullaniciId);

            if (sisKullanici is not null)
            {
                return Ok(sisKullanici);
            }
            return NotFound();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> KullaniciGetir([FromQuery] string kullaniciAdi)
        {
            var sisKullanici = await _kullaniciBilgiServisi.GetKullaniciBilgiAsync(kullaniciAdi);

            if (sisKullanici is not null)
            {
                return Ok(sisKullanici);
            }
            return NotFound();
        }
    }
}
