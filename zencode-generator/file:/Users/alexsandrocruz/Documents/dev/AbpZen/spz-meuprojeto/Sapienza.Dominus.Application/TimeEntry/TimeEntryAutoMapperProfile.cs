using AutoMapper;
using Sapienza.Dominus.TimeEntry.Dtos;

namespace Sapienza.Dominus.TimeEntry;

public class TimeEntryAutoMapperProfile : Profile
{
    public TimeEntryAutoMapperProfile()
    {
        CreateMap<TimeEntry, TimeEntryDto>()
            .ForMember(dest => dest.ProjectDisplayName, opt => opt.MapFrom(src => src.Project.Title));
        CreateMap<CreateUpdateTimeEntryDto, TimeEntry>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateTimeEntryDto, TimeEntry>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
