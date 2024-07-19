
using BildirimTestApp.Server.Servisler.Mail.DTO;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System.Net;
using System.Net.Mail;

namespace BildirimTestApp.Server.Servisler.Mail
{
    public class MailServisi : IMailServisi
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<MailServisi> _logger;

        public MailServisi(IConfiguration configuration, ILogger<MailServisi> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task MailGonder(MailDto mailDto)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_configuration["Mail:Username"]));

                if (mailDto.GonderilecekKullaniciMailleri == null) throw new Exception();

                foreach (var kullanici in mailDto.GonderilecekKullaniciMailleri)
                {
                    email.To.Add(MailboxAddress.Parse(kullanici));

                }
                email.Subject = mailDto.Baslik;
                email.Body = new TextPart(TextFormat.Html) { Text = mailDto.Aciklama };

                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                await smtp.ConnectAsync(_configuration["Mail:Host"], Convert.ToInt32(_configuration["Mail:Port"]), SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_configuration["Mail:Username"], _configuration["Mail:Password"]);
                smtp.Send(email);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception)
            {
                _logger.LogInformation("Mail Gönderilemedi");
                //throw new Exception("Mail gönderilirken bir hata oluştu. Lütfen daha sonra tekrar deneyin.");
            }

        }
    }
}
