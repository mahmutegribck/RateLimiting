using AutoMapper;
using BildirimTestApp.Server.DTOs;
using BildirimTestApp.Server.DTOs.BildirimDTO;
using BildirimTestApp.Server.Servisler.Bildirim;
using BildirimTestApp.Server.Servisler.Bildirim.BildirimGoruldu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace BildirimTestApp.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize]
    public class BildirimController : ControllerBase
    {
        private readonly IBildirimOlusturucu _bildirimOlusturucu;
        private readonly IBildirimHedefOlusturucu _bildirimHedefOlusturucu;
        private readonly IMapper _mapper;
        private readonly IBildirimGoruldu _bildirimGoruldu;
        public BildirimController(IBildirimOlusturucu bildirimOlusturucu, IBildirimHedefOlusturucu bildirimHedefOlusturucu, IMapper mapper, IBildirimGoruldu bildirimGoruldu)
        {
            _bildirimOlusturucu = bildirimOlusturucu;
            _bildirimHedefOlusturucu = bildirimHedefOlusturucu;
            _mapper = mapper;
            _bildirimGoruldu = bildirimGoruldu;
        }

        [Authorize]
        [HttpPatch]
        public async Task<IActionResult> SetBildirimGoruldu([FromBody] int[] bildirimIDs)
        {
            try
            {
                var currentUser = User?.Identity?.Name;
                if (currentUser == null) return NotFound("Kullanıcı Bulunamdı.");

                await _bildirimGoruldu.SetBildirimGoruldu(bildirimIDs, currentUser);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> YemehaneDuyuruBildirimGonder([FromBody] YemekhaneDuyuruBildirimDTO yemekhaneDuyuruBildirimDTO)
        {
            try
            {
                var yemehaneDuyuruBildirim = _mapper.Map<YemekhaneDuyuruBildirim>(yemekhaneDuyuruBildirimDTO);

                await _bildirimOlusturucu.BildirimGonder(yemehaneDuyuruBildirim, _bildirimHedefOlusturucu, yemekhaneDuyuruBildirimDTO.Aciklama);

                return Ok("Yemekhane Duyurusu Basariyla Olusturuldu.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata olustu: {ex.Message}");
            }
        }



        [HttpPost]
        public async Task<IActionResult> EkipToplantiDuyuruBildirimGonder([FromBody] ToplantiDuyuruBildirimDTO toplantiDuyuruBildirimDTO)
        {
            try
            {
                var toplantiDuyuruBildirim = _mapper.Map<ToplantiDuyuruBildirim>(toplantiDuyuruBildirimDTO);

                await _bildirimHedefOlusturucu.BildirimGonderilecekKullancilar(toplantiDuyuruBildirimDTO.GonderilecekKullaniciIdleri);

                await _bildirimOlusturucu.BildirimGonder(toplantiDuyuruBildirim, _bildirimHedefOlusturucu, toplantiDuyuruBildirimDTO.Aciklama);

                return Ok("Anlik Bildirim Basariyla Olusturuldu.");
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, $"Hata: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata olustu: {ex.Message}");
            }
        }


        [HttpPost]
        public async Task<IActionResult> EtkinlikDuyuruBildirimGonder([FromBody] EtkinlikDuyuruBildirimDTO etkinlikDuyuruBildirimDTO)
        {
            try
            {
                var etkinlikDuyuruBildirim = _mapper.Map<EtkinlikDuyuruBildirim>(etkinlikDuyuruBildirimDTO);


                await _bildirimOlusturucu.BildirimGonder(etkinlikDuyuruBildirim, _bildirimHedefOlusturucu, etkinlikDuyuruBildirimDTO.Aciklama);
                return Ok("Anlik Bildirim Başarıyla Oluşturuldu.");
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }


        [HttpPost]
        public async Task<IActionResult> EpostaBildirimGonder([FromBody] EpostaBildirim epostaBildirim)
        {
            try
            {
                await _bildirimOlusturucu.BildirimGonder(epostaBildirim, _bildirimHedefOlusturucu, epostaBildirim.Aciklama);
                return Ok("E-Posta Bildirimi Başarıyla Oluşturuldu.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }
    }
}
