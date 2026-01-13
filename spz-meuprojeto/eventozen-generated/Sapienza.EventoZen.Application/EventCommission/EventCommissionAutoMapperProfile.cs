using AutoMapper;
using Sapienza.EventoZen.EventCommission.Dtos;

namespace Sapienza.EventoZen.EventCommission;

public class EventCommissionAutoMapperProfile : Profile
{
    public EventCommissionAutoMapperProfile()
    {
        CreateMap<EventCommission, EventCommissionDto>()
            .ForMember(dest => dest.EventDisplayName, opt => opt.MapFrom(src => src.Event.Title));
        CreateMap<CreateUpdateEventCommissionDto, EventCommission>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateEventCommissionDto, EventCommission>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
