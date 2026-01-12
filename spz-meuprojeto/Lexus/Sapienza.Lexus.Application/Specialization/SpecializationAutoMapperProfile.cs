#nullable enable
using AutoMapper;
using Sapienza.Lexus.Specialization.Dtos;

namespace Sapienza.Lexus.Specialization;

public class SpecializationAutoMapperProfile : Profile
{
    public SpecializationAutoMapperProfile()
    {
        CreateMap<Specialization, SpecializationDto>();
        CreateMap<CreateUpdateSpecializationDto, Specialization>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateSpecializationDto, Specialization>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
