using BildirimTestApp.Server.Servisler.Mail;
using BildirimTestApp.Server.Servisler.Mail.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace BildirimTestApp.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IMailServisi _mailServisi;

        public EmailController(IMailServisi mailServisi)
        {
            _mailServisi = mailServisi;
        }

        [HttpPost]
        //[EnableRateLimiting("Concurrency")]
        public async Task<IActionResult> EpostaGonder([FromBody] MailDto mailDto)
        {
            await _mailServisi.MailGonder(mailDto);
            return Ok();
        }
    }
}
