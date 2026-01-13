using AutoMapper;
using Sapienza.Lexus.advTarefas.Dtos;

namespace Sapienza.Lexus.advTarefas;

public class advTarefasAutoMapperProfile : Profile
{
    public advTarefasAutoMapperProfile()
    {
        CreateMap<advTarefas, advTarefasDto>();
        CreateMap<CreateUpdateadvTarefasDto, advTarefas>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvTarefasDto, advTarefas>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
