using AutoMapper;
using Volo.FileManagement.Directories;
using Volo.FileManagement.Files;

namespace Volo.FileManagement;

public class FileManagementApplicationAutoMapperProfile : Profile
{
    public FileManagementApplicationAutoMapperProfile()
    {
        CreateMap<DirectoryDescriptor, DirectoryDescriptorDto>().MapExtraProperties();
        CreateMap<DirectoryDescriptorDto, RenameDirectoryInput>();

        CreateMap<FileDescriptor, FileDescriptorDto>().MapExtraProperties();

        CreateMap<DirectoryDescriptor, DirectoryContentDto>()
            .ForMember(dest => dest.IsDirectory, 
                opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.Size, 
                opt => opt.MapFrom(src => 0))
            .ForMember(dest => dest.IconInfo, 
                opt => opt.Ignore())
            .MapExtraProperties();
        
        CreateMap<FileDescriptor, DirectoryContentDto>()
            .ForMember(dest => dest.IsDirectory, 
                opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.IconInfo, 
                opt => opt.Ignore())
            .MapExtraProperties();
    }
}