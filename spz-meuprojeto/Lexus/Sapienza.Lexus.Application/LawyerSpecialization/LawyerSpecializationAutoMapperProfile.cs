#nullable enable
using AutoMapper;
using Sapienza.Lexus.LawyerSpecialization.Dtos;

namespace Sapienza.Lexus.LawyerSpecialization;

public class LawyerSpecializationAutoMapperProfile : Profile
{
    public LawyerSpecializationAutoMapperProfile()
    {
        CreateMap<LawyerSpecialization, LawyerSpecializationDto>()
            .ForMember(dest => dest.LawyerDisplayName, opt => opt.MapFrom(src => src.Lawyer.FullName))
            .ForMember(dest => dest.SpecializationDisplayName, opt => opt.MapFrom(src => src.Specialization.Name));
        CreateMap<CreateUpdateLawyerSpecializationDto, LawyerSpecialization>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateLawyerSpecializationDto, LawyerSpecialization>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
