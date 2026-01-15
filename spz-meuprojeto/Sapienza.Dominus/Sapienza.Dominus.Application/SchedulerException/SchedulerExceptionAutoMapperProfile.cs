using AutoMapper;
using Sapienza.Dominus.SchedulerException.Dtos;

namespace Sapienza.Dominus.SchedulerException;

public class SchedulerExceptionAutoMapperProfile : Profile
{
    public SchedulerExceptionAutoMapperProfile()
    {
        CreateMap<SchedulerException, SchedulerExceptionDto>()
            .ForMember(dest => dest.SchedulerTypeDisplayName, opt => opt.MapFrom(src => src.SchedulerType.Name));
        CreateMap<CreateUpdateSchedulerExceptionDto, SchedulerException>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateSchedulerExceptionDto, SchedulerException>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
