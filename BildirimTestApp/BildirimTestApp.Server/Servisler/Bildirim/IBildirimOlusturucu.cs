namespace BildirimTestApp.Server.Servisler.Bildirim;

public interface IBildirimOlusturucu
{
    Task BildirimGonder<T>(
        IBildirim<T> bildirim,
        IBildirimHedefOlusturucu bildirimHedefOlusturucu,
        string? aciklama = null
    );
    Task BildirimGonder(
        IBildirim bildirim,
        IBildirimHedefOlusturucu bildirimHedefOlusturucu,
        string? aciklama = null
    );
}
