using AutoMapper;
using Sapienza.Dominus.SchedulerType.Dtos;

namespace Sapienza.Dominus.SchedulerType;

public class SchedulerTypeAutoMapperProfile : Profile
{
    public SchedulerTypeAutoMapperProfile()
    {
        CreateMap<SchedulerType, SchedulerTypeDto>();
        CreateMap<CreateUpdateSchedulerTypeDto, SchedulerType>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateSchedulerTypeDto, SchedulerType>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
