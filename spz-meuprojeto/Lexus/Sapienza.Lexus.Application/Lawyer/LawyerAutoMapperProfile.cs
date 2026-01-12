using AutoMapper;
using Sapienza.Lexus.Lawyer.Dtos;

namespace Sapienza.Lexus.Lawyer;

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
