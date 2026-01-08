using AutoMapper;
using Sapienza.Lexus.Lawyer.Dtos;

namespace Sapienza.Lexus.Lawyer;

public class LawyerAutoMapperProfile : Profile
{
    public LawyerAutoMapperProfile()
    {
        CreateMap<Lawyer, LawyerDto>();
        CreateMap<CreateUpdateLawyerDto, Lawyer>();
        CreateMap<CreateUpdateLawyerDto, Lawyer>();
    }
}
