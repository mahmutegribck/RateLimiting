using AutoMapper;
using BildirimTestApp.Server.DTOs;
using BildirimTestApp.Server.Models;
using BildirimTestApp.Server.Servisler.Bildirim;
using BildirimTestApp.Server.Servisler.Kullanici;
using BildirimTestApp.Server.Servisler.KullaniciGorev;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BildirimTestApp.Server.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class GorevController : ControllerBase
    {
        private readonly IGorevServisi _gorevServisi;

        public GorevController(IGorevServisi gorevServisi)
        {
            _gorevServisi = gorevServisi;
        }

        [HttpPost]
        public async Task<IActionResult> GorevOlustur([FromBody] GorevOlusturmaDTO gorevDTO)
        {
            try
            {
                await _gorevServisi.GorevAta(gorevDTO);
                return Ok("Gorev Olusturuldu.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
