using AutoMapper;
using BildirimTestApp.Server.DTOs;
using BildirimTestApp.Server.DTOs.BildirimDTO;
using BildirimTestApp.Server.Models;
using BildirimTestApp.Server.Servisler.OturumYonetimi.DTO;

namespace BildirimTestApp.Server.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<SisKullanici, KullaniciGirisDto>().ReverseMap();
            CreateMap<SisKullanici, KullaniciKayitDto>().ReverseMap();

            CreateMap<SisKullanici, GetKullaniciDto>().ReverseMap();

            CreateMap<ToplantiDuyuruBildirim, ToplantiDuyuruBildirimDTO>().ReverseMap();
            CreateMap<EtkinlikDuyuruBildirim, EtkinlikDuyuruBildirimDTO>().ReverseMap();
            CreateMap<YemekhaneDuyuruBildirim, YemekhaneDuyuruBildirimDTO>().ReverseMap();
            CreateMap<GorevAtandiAnlikBildirim, GorevAtandiAnlikBildirimDTO>().ReverseMap();

            CreateMap<Gorev, GorevOlusturmaDTO>().ReverseMap();
            CreateMap<GorevAtandiAnlikBildirim, GorevOlusturmaDTO>().ReverseMap();
        }
    }
}
