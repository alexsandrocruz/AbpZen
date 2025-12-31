using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.Features;
using Volo.Abp.ObjectExtending;
using Volo.FileManagement.Authorization;
using Volo.FileManagement.Files;

namespace Volo.FileManagement.Directories;

[RequiresFeature(FileManagementFeatures.Enable)]
[Authorize(FileManagementPermissions.DirectoryDescriptor.Default)]
public class DirectoryDescriptorAppService : FileManagementAppService, IDirectoryDescriptorAppService
{
    protected IDirectoryManager DirectoryManager { get; }
    protected IDirectoryDescriptorRepository DirectoryDescriptorRepository { get; }
    protected IFileManager FileManager { get; }
    protected IFileDescriptorRepository FileDescriptorRepository { get; }

    protected FileIconOption FileIconOption { get; }

    public DirectoryDescriptorAppService(
        IDirectoryManager directoryManager,
        IFileManager fileManager,
        IDirectoryDescriptorRepository directoryDescriptorRepository,
        IFileDescriptorRepository fileDescriptorRepository,
        IOptions<FileIconOption> fileIconOption)
    {
        DirectoryManager = directoryManager;
        FileManager = fileManager;
        DirectoryDescriptorRepository = directoryDescriptorRepository;
        FileDescriptorRepository = fileDescriptorRepository;

        FileIconOption = fileIconOption.Value;
    }

    public virtual async Task<DirectoryDescriptorDto> GetAsync(Guid id)
    {
        var directoryDescriptor = await DirectoryDescriptorRepository.GetAsync(id);

        return ObjectMapper.Map<DirectoryDescriptor, DirectoryDescriptorDto>(directoryDescriptor);
    }

    public virtual async Task<ListResultDto<DirectoryDescriptorInfoDto>> GetListAsync(Guid? parentId)
    {
        var subDirectories = await DirectoryDescriptorRepository.GetChildrenAsync(parentId);

        var result = new List<DirectoryDescriptorInfoDto>();

        foreach (var subDirectory in subDirectories)
        {
            result.Add(new DirectoryDescriptorInfoDto
            {
                Name = subDirectory.Name,
                Id = subDirectory.Id,
                ParentId = subDirectory.ParentId,
                HasChildren = await DirectoryDescriptorRepository.ContainsAnyAsync(subDirectory.Id, false)
            });
        }

        return new ListResultDto<DirectoryDescriptorInfoDto>(result);
    }

    [Authorize(FileManagementPermissions.DirectoryDescriptor.Create)]
    public virtual async Task<DirectoryDescriptorDto> CreateAsync(CreateDirectoryInput input)
    {
        var directoryDescriptor = await DirectoryManager.CreateAsync(input.Name, input.ParentId, CurrentTenant.Id);

        input.MapExtraPropertiesTo(directoryDescriptor);

        await DirectoryDescriptorRepository.InsertAsync(directoryDescriptor);

        return ObjectMapper.Map<DirectoryDescriptor, DirectoryDescriptorDto>(directoryDescriptor);
    }

    [Authorize(FileManagementPermissions.DirectoryDescriptor.Update)]
    public virtual async Task<DirectoryDescriptorDto> RenameAsync(Guid id, RenameDirectoryInput input)
    {
        var directory = await DirectoryDescriptorRepository.GetAsync(id);

        directory.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);

        await DirectoryManager.RenameAsync(directory, input.Name);
        await DirectoryDescriptorRepository.UpdateAsync(directory);

        return ObjectMapper.Map<DirectoryDescriptor, DirectoryDescriptorDto>(directory);
    }

    public virtual async Task<PagedResultDto<DirectoryContentDto>> GetContentAsync(DirectoryContentRequestInput input)
    {
        var result = new List<DirectoryContentDto>();
        var subDirectoryCount = await DirectoryDescriptorRepository.GetChildrenCountAsync(input.Id, input.Filter);
        var subFileCount = await FileDescriptorRepository.CountDirectoryFilesAsync(input.Id, input.Filter);

        // directory can be orderable for only its name
        var directorySorting =
                input.Sorting?.IndexOf("name asc", StringComparison.OrdinalIgnoreCase) >= 0 ?
                "name asc" :
                input.Sorting?.IndexOf("name desc", StringComparison.OrdinalIgnoreCase) >= 0 ?
                "name desc" :
                null;

        var subDirectories = await DirectoryDescriptorRepository.GetChildrenAsync(input.Id, input.Filter, directorySorting, input.MaxResultCount, input.SkipCount);
        result.AddRange(ObjectMapper.Map<List<DirectoryDescriptor>, List<DirectoryContentDto>>(subDirectories));

        if (await AuthorizationService.IsGrantedAsync(FileManagementPermissions.FileDescriptor.Default))
        {
            var fileSkipCount = input.SkipCount <= subDirectoryCount ? 0 : input.SkipCount - subDirectoryCount;
            var fileMaxResultCount = input.MaxResultCount - subDirectories.Count;

            var subFiles = await FileDescriptorRepository.GetListAsync(input.Id, input.Filter, input.Sorting, fileMaxResultCount, fileSkipCount);
            var subFilesDto = ObjectMapper.Map<List<FileDescriptor>, List<DirectoryContentDto>>(subFiles);
            foreach (var fileDto in subFilesDto)
            {
                fileDto.IconInfo = FileIconOption.GetFileIconInfo(fileDto.Name);
                
                result.Add(fileDto);
            }
        }

        return new PagedResultDto<DirectoryContentDto>(subDirectoryCount + subFileCount, result);
    }

    [Authorize(FileManagementPermissions.DirectoryDescriptor.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await DirectoryManager.DeleteAsync(id);
    }

    [Authorize(FileManagementPermissions.DirectoryDescriptor.Update)]
    public virtual async Task<DirectoryDescriptorDto> MoveAsync(MoveDirectoryInput input)
    {
        var directory = await DirectoryDescriptorRepository.GetAsync(input.Id);

        directory.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);

        await DirectoryManager.MoveAsync(directory, input.NewParentId);
        await DirectoryDescriptorRepository.UpdateAsync(directory);

        return ObjectMapper.Map<DirectoryDescriptor, DirectoryDescriptorDto>(directory);
    }
}
