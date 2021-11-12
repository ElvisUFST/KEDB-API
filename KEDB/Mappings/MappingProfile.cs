using AutoMapper;
using KEDB.Dto;
using KEDB.Model;

namespace KEDB.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Kontrolrapport, KontrolrapportDto>()
                .ForMember(dest => dest.KontrolrapportId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Sagsbehandler, opt => opt.MapFrom(src => ADUserDto.Parse(src.Sagsbehandler)))
                .ReverseMap()
                .ForMember(dest => dest.Sagsbehandler, opt => opt.MapFrom(src => src.Sagsbehandler == null ? null : src.Sagsbehandler.ToString()));

            CreateMap<Toldrapport, ToldrapportDto>()
                .ForMember(dest => dest.ToldrapportTransportmiddel, opt => opt.MapFrom(src => src.ToldrapportTransportmiddel.Tekst))
                .ForMember(dest => dest.ToldrapportOpdagendeAktoer, opt => opt.MapFrom(src => src.ToldrapportOpdagendeAktoer.Tekst))
                .ForMember(dest => dest.ToldrapportFejlKategori, opt => opt.MapFrom(src => src.ToldrapportFejlKategori.Tekst))
                .ForMember(dest => dest.ToldrapportKommunikation, opt => opt.MapFrom(src => src.ToldrapportKommunikation.Tekst))
                .ForMember(dest => dest.ToldrapportOvertraedelsesAktoer, opt => opt.MapFrom(src => src.ToldrapportOvertraedelsesAktoer.Tekst))
                .ReverseMap()
                .ForPath(dest => dest.ToldrapportTransportmiddel.Tekst, opt => opt.Ignore())
                .ForPath(dest => dest.ToldrapportOpdagendeAktoer.Tekst, opt => opt.Ignore())
                .ForPath(dest => dest.ToldrapportFejlKategori.Tekst, opt => opt.Ignore())
                .ForPath(dest => dest.ToldrapportKommunikation.Tekst, opt => opt.Ignore())
                .ForPath(dest => dest.ToldrapportOvertraedelsesAktoer.Tekst, opt => opt.Ignore());

            CreateMap<Rubrik, RubrikDto>()
                .ForMember(dest => dest.RubrikId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.RubrikTypeId, opt => opt.MapFrom(src => src.RubrikTypeId))
                .ForMember(dest => dest.RubrikTypeNummer, opt => opt.MapFrom(src => src.RubrikType.Nummer))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.RubrikType.Navn))
                .ForMember(dest => dest.MuligeFejl, opt => opt.MapFrom(src => src.RubrikType.RubrikMuligeFejl))
                .ForMember(dest => dest.ValgteFejl, opt => opt.MapFrom(src => src.RubrikValgteFejl))
                .ReverseMap()
                .ForMember(dest => dest.RubrikType, opt => opt.Ignore())
                .ForMember(dest => dest.RubrikValgteFejl, opt => opt.MapFrom(src => src.ValgteFejl));

            CreateMap<RubrikMuligFejl, FejltekstDto>()
                .ForMember(dest => dest.Tekst, opt => opt.MapFrom(src => src.Fejltekst.Tekst));

            CreateMap<RubrikValgtFejl, FejltekstDto>()
                .ForMember(dest => dest.Tekst, opt => opt.MapFrom(src => src.Fejltekst.Tekst))
                .ReverseMap()
                .ForMember(dest => dest.Fejltekst, opt => opt.Ignore());
        }
    }
}