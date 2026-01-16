using AutoMapper;
using Sapienza.Lexus.advPreLogStatus.Dtos;

namespace Sapienza.Lexus.advPreLogStatus;

public class advPreLogStatusAutoMapperProfile : Profile
{
    public advPreLogStatusAutoMapperProfile()
    {
        CreateMap<advPreLogStatus, advPreLogStatusDto>();
        CreateMap<CreateUpdateadvPreLogStatusDto, advPreLogStatus>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvPreLogStatusDto, advPreLogStatus>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
