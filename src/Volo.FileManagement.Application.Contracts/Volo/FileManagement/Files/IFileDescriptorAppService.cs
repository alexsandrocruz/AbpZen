using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace Volo.FileManagement.Files;

public interface IFileDescriptorAppService : IApplicationService
{
    Task<FileDescriptorDto> GetAsync(Guid id);

    Task<ListResultDto<FileDescriptorDto>> GetListAsync(Guid? directoryId);

    Task<FileDescriptorDto> RenameAsync(Guid id, RenameFileInput input);

    Task<IRemoteStreamContent> DownloadAsync(Guid id, string token);

    Task DeleteAsync(Guid id);

    Task<FileDescriptorDto> CreateAsync(Guid? directoryId, [NotNull] CreateFileInputWithStream inputWithStream);

    Task<FileDescriptorDto> MoveAsync(MoveFileInput input);

    Task<List<FileUploadPreInfoDto>> GetPreInfoAsync(List<FileUploadPreInfoRequest> input);

    Task<byte[]> GetContentAsync(Guid id);

    Task<DownloadTokenResultDto> GetDownloadTokenAsync(Guid id);
}
