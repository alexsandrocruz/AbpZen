using AutoMapper;
using Sapienza.Lexus.advPreArquivosStatus.Dtos;

namespace Sapienza.Lexus.advPreArquivosStatus;

public class advPreArquivosStatusAutoMapperProfile : Profile
{
    public advPreArquivosStatusAutoMapperProfile()
    {
        CreateMap<advPreArquivosStatus, advPreArquivosStatusDto>();
        CreateMap<CreateUpdateadvPreArquivosStatusDto, advPreArquivosStatus>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateadvPreArquivosStatusDto, advPreArquivosStatus>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
