using AutoMapper;
using Sapienza.Lexus.LegalProcess.Dtos;

namespace Sapienza.Lexus.LegalProcess;

public class LegalProcessAutoMapperProfile : Profile
{
    public LegalProcessAutoMapperProfile()
    {
        CreateMap<LegalProcess, LegalProcessDto>()
            .ForMember(dest => dest.LawyerDisplayName, opt => opt.MapFrom(src => src.Lawyer.FullName))
            .ForMember(dest => dest.ClientDisplayName, opt => opt.MapFrom(src => src.Client.Name));
        CreateMap<CreateUpdateLegalProcessDto, LegalProcess>();
        CreateMap<CreateUpdateLegalProcessDto, LegalProcess>();
    }
}
