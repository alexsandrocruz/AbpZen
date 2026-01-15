using AutoMapper;
using Sapienza.Dominus.File.Dtos;

namespace Sapienza.Dominus.File;

public class FileAutoMapperProfile : Profile
{
    public FileAutoMapperProfile()
    {
        CreateMap<File, FileDto>();
        CreateMap<CreateUpdateFileDto, File>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
        CreateMap<CreateUpdateFileDto, File>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());
    }
}
