using AutoMapper;
using BildirimTestApp.Server.DTOs;
using BildirimTestApp.Server.Models;
using BildirimTestApp.Server.Servisler.Bildirim;
using Microsoft.EntityFrameworkCore;


namespace BildirimTestApp.Server.Servisler.KullaniciGorev
{
    public class GorevServisi : IGorevServisi
    {
        private readonly IMapper _mapper;
        private readonly TestDbContext _context;
        private readonly IBildirimHedefOlusturucu _bildirimHedefOlusturucu;
        private readonly IBildirimOlusturucu _bildirimOlusturucu;


        public GorevServisi(IMapper mapper, TestDbContext context, IBildirimHedefOlusturucu bildirimHedefOlusturucu, IBildirimOlusturucu bildirimOlusturucu)
        {
            _mapper = mapper;
            _context = context;
            _bildirimHedefOlusturucu = bildirimHedefOlusturucu;
            _bildirimOlusturucu = bildirimOlusturucu;
        }

        public async Task GorevAta(GorevOlusturmaDTO gorevDTO)
        {
            var gorev = _mapper.Map<Gorev>(gorevDTO);

            if (gorevDTO.GonderilecekKullaniciIdleri != null && gorevDTO.GonderilecekKullaniciIdleri.Any())
            {
                foreach (var kullaniciId in gorevDTO.GonderilecekKullaniciIdleri)
                {
                    var kullanici = await _context.SisKullanicis.FindAsync(kullaniciId);

                    if (kullanici == null)
                        throw new Exception("Kullanici Bulunamadi.");

                    gorev.SisKullanicisKullanicis.Add(kullanici);

                }

                
                await _context.Gorevs.AddAsync(gorev);
                await _context.SaveChangesAsync();

                //Gorev olusturulan kullaniciya hakkinda gorev olusturuldu bildirimi atilir.
                var gorevAtandiAnlikBildirim = _mapper.Map<GorevAtandiAnlikBildirim>(gorevDTO);
                await _bildirimHedefOlusturucu.BildirimGonderilecekKullancilar(gorevDTO.GonderilecekKullaniciIdleri);
                await _bildirimOlusturucu.BildirimGonder(gorevAtandiAnlikBildirim, _bildirimHedefOlusturucu, gorevDTO.Aciklama);

            }
            else
            {
                throw new Exception("Gorev Olusturulamadi.");

            }
        }
    }
}
