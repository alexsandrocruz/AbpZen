using AutoMapper;
using Sapienza.Cursos.Lawyer.Dtos;

namespace Sapienza.Cursos.Lawyer;

public class LawyerAutoMapperProfile : Profile
{
    public LawyerAutoMapperProfile()
    {
        CreateMap<Lawyer, LawyerDto>();
        CreateMap<CreateUpdateLawyerDto, Lawyer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateLawyerDto, Lawyer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
