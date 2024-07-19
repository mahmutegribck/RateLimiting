using BildirimTestApp.Server.Servisler.Mail.DTO;

namespace BildirimTestApp.Server.Servisler.Mail
{
    public interface IMailServisi
    {
        Task MailGonder(MailDto mailDto);
    }
}
