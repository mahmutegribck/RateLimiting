using Newtonsoft.Json;

namespace BildirimTestApp.Server.Servisler.Bildirim;

public class BildirimSerilestirmeKonteyner
{
    public required string BildirimId { get; set; }
    public required string BildirimJSon { get; set; }

    public static BildirimSerilestirmeKonteyner BildirimdenOlustur(IBildirim bildirim) =>
        new BildirimSerilestirmeKonteyner
        {
            BildirimJSon = JsonConvert.SerializeObject(bildirim),
            BildirimId = bildirim.BildirimIdOlustur()
        };

    public static BildirimSerilestirmeKonteyner BildirimdenOlustur<T>(IBildirim<T> bildirim) =>
        new BildirimSerilestirmeKonteyner
        {
            BildirimJSon = JsonConvert.SerializeObject(bildirim),
            BildirimId = bildirim.BildirimIdOlustur()
        };
}
