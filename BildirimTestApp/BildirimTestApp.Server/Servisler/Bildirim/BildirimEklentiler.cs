namespace BildirimTestApp.Server.Servisler.Bildirim;

public static class BildirimEklentiler
{
    public static string BildirimIdOlustur(this IBildirim bildirim) => bildirim.GetType().Name;

    public static string BildirimIdOlustur<T>(this IBildirim<T> bildirim) =>
        bildirim.GetType().Name;
}
