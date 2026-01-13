using AutoMapper;
using Sapienza.Lexus.advClientesArquivos.Dtos;

namespace Sapienza.Lexus.advClientesArquivos;

public class advClientesArquivosAutoMapperProfile : Profile
{
    public advClientesArquivosAutoMapperProfile()
    {
        CreateMap<advClientesArquivos, advClientesArquivosDto>();
        CreateMap<CreateUpdateadvClientesArquivosDto, advClientesArquivos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvClientesArquivosDto, advClientesArquivos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
