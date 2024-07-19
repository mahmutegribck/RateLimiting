namespace BildirimTestApp.Server.Servisler.Bildirim;

public interface IBildirimDonusturucuKok { }

public interface IBildirimDonusturucu<TBildirim, TBildirimCevap> : IBildirimDonusturucuKok
{
    Task<TBildirimCevap> Donustur(TBildirim bildirim);
}
