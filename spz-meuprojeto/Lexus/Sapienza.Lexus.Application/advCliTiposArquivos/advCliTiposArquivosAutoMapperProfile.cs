using AutoMapper;
using Sapienza.Lexus.advCliTiposArquivos.Dtos;

namespace Sapienza.Lexus.advCliTiposArquivos;

public class advCliTiposArquivosAutoMapperProfile : Profile
{
    public advCliTiposArquivosAutoMapperProfile()
    {
        CreateMap<advCliTiposArquivos, advCliTiposArquivosDto>()
            .ForMember(dest => dest.advClientesArquivosDisplayName, opt => opt.MapFrom(src => src.advCliTiposArquivosNav.descricao))
            .ForMember(dest => dest.advClientesChecklistDisplayName, opt => opt.MapFrom(src => src.advCliTiposArquivosNav1.Id));
        CreateMap<CreateUpdateadvCliTiposArquivosDto, advCliTiposArquivos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvCliTiposArquivosDto, advCliTiposArquivos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
