using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Content;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Services;
using Volo.Abp.ObjectExtending;

namespace Volo.FileManagement.Files;

public interface IFileManager : IDomainService
{
    [Obsolete("Use CreateAsync with IRemoteStreamContent")]
    Task<FileDescriptor> CreateAsync(
        [NotNull] string name,
        [NotNull] string mimeType,
        [NotNull] byte[] content,
        Guid? directoryId = null,
        Guid? tenantId = null,
        bool overrideExisting = false);

    Task<FileDescriptor> CreateAsync(
        [NotNull] string name,
        [NotNull] string mimeType,
        [NotNull] IRemoteStreamContent content,
        Guid? directoryId = null,
        Guid? tenantId = null,
        bool overrideExisting = false);

    Task RenameAsync(FileDescriptor file, [NotNull] string newName);

    Task DeleteAllAsync(Guid? directoryId);

    Task DeleteAsync(FileDescriptor id);

    Task MoveAsync(FileDescriptor file, Guid? newDirectoryId);
}
