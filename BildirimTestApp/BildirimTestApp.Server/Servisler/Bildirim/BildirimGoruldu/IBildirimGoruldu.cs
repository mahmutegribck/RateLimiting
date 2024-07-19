namespace BildirimTestApp.Server.Servisler.Bildirim.BildirimGoruldu
{
    public interface IBildirimGoruldu
    {
        Task SetBildirimGoruldu(int[] bildirimIDs, string kullaniciAdi);
    }
}
