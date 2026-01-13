using AutoMapper;
using Sapienza.Lexus.autoFTP.Dtos;

namespace Sapienza.Lexus.autoFTP;

public class autoFTPAutoMapperProfile : Profile
{
    public autoFTPAutoMapperProfile()
    {
        CreateMap<autoFTP, autoFTPDto>();
        CreateMap<CreateUpdateautoFTPDto, autoFTP>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateautoFTPDto, autoFTP>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
