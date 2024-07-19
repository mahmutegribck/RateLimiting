using BildirimTestApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace BildirimTestApp.Server.Servisler.Bildirim;

public interface IBildirimHedefOlusturucu
{
    int[] HedefKullaniciIdler { get; }
    Task BildirimGonderilecekKullancilar(List<int>? gonderilecekKullaniciIdleri);
}


public class BildirimHedefOlusturucu : IBildirimHedefOlusturucu
{
    private readonly TestDbContext _testDbContext;
    private List<int>? _gonderilecekKullaniciIdleri = null;

    public BildirimHedefOlusturucu(TestDbContext testDbContext)
    {
        _testDbContext = testDbContext;
    }

    public async Task BildirimGonderilecekKullancilar(List<int>? gonderilecekKullaniciIdleri)
    {
        try
        {
            _gonderilecekKullaniciIdleri = gonderilecekKullaniciIdleri;

            if (_gonderilecekKullaniciIdleri == null || _gonderilecekKullaniciIdleri.Count == 0)
            {
                _gonderilecekKullaniciIdleri = null;
                return;
            }

            var varOlanIdler = await _testDbContext.SisKullanicis.Where(k => gonderilecekKullaniciIdleri!.Contains(k.KullaniciId)).Select(k => k.KullaniciId).ToListAsync();

            if (varOlanIdler.Count != gonderilecekKullaniciIdleri?.Count)
            {
                throw new ArgumentException("Gecersiz Id'ler Iceriyor");
            }
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public int[] HedefKullaniciIdler => _gonderilecekKullaniciIdleri?.ToArray() ?? _testDbContext.SisKullanicis.Select(k => k.KullaniciId).ToArray();
}

