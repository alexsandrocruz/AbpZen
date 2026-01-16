using AutoMapper;
using Sapienza.Lexus.advProOrgaos.Dtos;

namespace Sapienza.Lexus.advProOrgaos;

public class advProOrgaosAutoMapperProfile : Profile
{
    public advProOrgaosAutoMapperProfile()
    {
        CreateMap<advProOrgaos, advProOrgaosDto>();
        CreateMap<CreateUpdateadvProOrgaosDto, advProOrgaos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvProOrgaosDto, advProOrgaos>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
