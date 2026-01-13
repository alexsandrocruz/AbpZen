using AutoMapper;
using Sapienza.Lexus.advAgeTiposTarefas.Dtos;

namespace Sapienza.Lexus.advAgeTiposTarefas;

public class advAgeTiposTarefasAutoMapperProfile : Profile
{
    public advAgeTiposTarefasAutoMapperProfile()
    {
        CreateMap<advAgeTiposTarefas, advAgeTiposTarefasDto>();
        CreateMap<CreateUpdateadvAgeTiposTarefasDto, advAgeTiposTarefas>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvAgeTiposTarefasDto, advAgeTiposTarefas>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
