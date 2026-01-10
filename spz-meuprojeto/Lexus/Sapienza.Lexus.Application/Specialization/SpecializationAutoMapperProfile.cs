using AutoMapper;
using Sapienza.Lexus.Specialization.Dtos;

namespace Sapienza.Lexus.Specialization;

public class SpecializationAutoMapperProfile : Profile
{
    public SpecializationAutoMapperProfile()
    {
        CreateMap<Specialization, SpecializationDto>();
        CreateMap<CreateUpdateSpecializationDto, Specialization>();
        CreateMap<CreateUpdateSpecializationDto, Specialization>();
    }
}
