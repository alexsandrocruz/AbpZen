using AutoMapper;
using Sapienza.Cursos.LegalProcess.Dtos;

namespace Sapienza.Cursos.LegalProcess;

public class LegalProcessAutoMapperProfile : Profile
{
    public LegalProcessAutoMapperProfile()
    {
        CreateMap<LegalProcess, LegalProcessDto>()
            .ForMember(dest => dest.LawyerDisplayName, opt => opt.MapFrom(src => src.Lawyer.FullName))
            .ForMember(dest => dest.ClientDisplayName, opt => opt.MapFrom(src => src.Client.Name));
        CreateMap<CreateUpdateLegalProcessDto, LegalProcess>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateLegalProcessDto, LegalProcess>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
