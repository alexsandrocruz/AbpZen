using AutoMapper;
using Sapienza.Dominus.SchedulerAvailability.Dtos;

namespace Sapienza.Dominus.SchedulerAvailability;

public class SchedulerAvailabilityAutoMapperProfile : Profile
{
    public SchedulerAvailabilityAutoMapperProfile()
    {
        CreateMap<SchedulerAvailability, SchedulerAvailabilityDto>()
            .ForMember(dest => dest.SchedulerTypeDisplayName, opt => opt.MapFrom(src => src.SchedulerType.Name));
        CreateMap<CreateUpdateSchedulerAvailabilityDto, SchedulerAvailability>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateSchedulerAvailabilityDto, SchedulerAvailability>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
