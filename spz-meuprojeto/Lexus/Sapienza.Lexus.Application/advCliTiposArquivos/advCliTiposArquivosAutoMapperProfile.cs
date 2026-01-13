using AutoMapper;
using Sapienza.Lexus.advCliTiposArquivos.Dtos;

namespace Sapienza.Lexus.advCliTiposArquivos;

public class advCliTiposArquivosAutoMapperProfile : Profile
{
    public advCliTiposArquivosAutoMapperProfile()
    {
        CreateMap<advCliTiposArquivos, advCliTiposArquivosDto>();
        CreateMap<CreateUpdateadvCliTiposArquivosDto, advCliTiposArquivos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvCliTiposArquivosDto, advCliTiposArquivos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
