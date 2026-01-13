using AutoMapper;
using Sapienza.Lexus.advClientesChecklist.Dtos;

namespace Sapienza.Lexus.advClientesChecklist;

public class advClientesChecklistAutoMapperProfile : Profile
{
    public advClientesChecklistAutoMapperProfile()
    {
        CreateMap<advClientesChecklist, advClientesChecklistDto>();
        CreateMap<CreateUpdateadvClientesChecklistDto, advClientesChecklist>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvClientesChecklistDto, advClientesChecklist>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
