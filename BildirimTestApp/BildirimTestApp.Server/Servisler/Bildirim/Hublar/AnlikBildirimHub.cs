using BildirimTestApp.Server.Servisler.Kullanici;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace BildirimTestApp.Server.Servisler.Bildirim.Hublar;

public class AnlikBildirimHub : BildirimHubKok
{
    public AnlikBildirimHub(
        ILogger<BildirimHubKok> logger,
        IKullaniciBilgiServisi kullaniciBilgiServisi
    )
        : base(logger, kullaniciBilgiServisi) { }

    //public async Task NewMessage(string user, string message)
    //{
    //    await Clients.All.SendAsync("messageReceived", user, message);
    //}

    //public async Task SendMessageToUser(string fromUserName, string toUserName, string message)
    //{
    //    await Clients.Group(toUserName).SendAsync("messageToUserReceived", fromUserName, message);
    //}


}
