using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Volo.Abp;
using Volo.Abp.BlobStoring;
using Volo.Abp.Content;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;
using Volo.Abp.ObjectExtending;
using Volo.FileManagement.Directories;
using Volo.FileManagement.Localization;

namespace Volo.FileManagement.Files;

public class FileManager : DomainService, IFileManager
{
    protected IFileDescriptorRepository FileDescriptorRepository { get; }
    protected IDirectoryDescriptorRepository DirectoryDescriptorRepository { get; }
    protected IBlobContainer<FileManagementContainer> BlobContainer { get; }

    public FileManager(
        IFileDescriptorRepository fileDescriptorRepository,
        IBlobContainer<FileManagementContainer> blobContainer,
        IDirectoryDescriptorRepository directoryDescriptorRepository)
    {
        FileDescriptorRepository = fileDescriptorRepository;
        BlobContainer = blobContainer;
        DirectoryDescriptorRepository = directoryDescriptorRepository;
    }

    [Obsolete("Use CreateAsync with IRemoteStreamContent")]
    public virtual async Task<FileDescriptor> CreateAsync(
        string name,
        string mimeType,
        byte[] content,
        Guid? directoryId = null,
        Guid? tenantId = null,
        bool overrideExisting = false)
    {
        var fileDescriptor = await SaveFileDescriptorAsync(name, mimeType, content.Length, directoryId, tenantId, overrideExisting);

        await BlobContainer.SaveAsync(fileDescriptor.Id.ToString(), content, true);

        return fileDescriptor;
    }

    public virtual async Task<FileDescriptor> CreateAsync(string name, string mimeType, IRemoteStreamContent content, Guid? directoryId = null, Guid? tenantId = null, bool overrideExisting = false)
    {
        var fileDescriptor = await SaveFileDescriptorAsync(name, mimeType, (int)(content.ContentLength ?? 0), directoryId, tenantId, overrideExisting);

        await BlobContainer.SaveAsync(fileDescriptor.Id.ToString(), content.GetStream(), true);

        return fileDescriptor;
    }

    protected virtual async Task<FileDescriptor> SaveFileDescriptorAsync(string name, string mimeType, int contentLength, Guid? directoryId, Guid? tenantId, bool overrideExisting)
    {
        var fileDescriptor = await FileDescriptorRepository.FindAsync(name, directoryId);

        if (fileDescriptor != null)
        {
            if (!overrideExisting)
            {
                throw new FileAlreadyExistException(name);
            }

            fileDescriptor.Size = contentLength;
            await FileDescriptorRepository.UpdateAsync(fileDescriptor);
        }
        else
        {
            fileDescriptor = new FileDescriptor(GuidGenerator.Create(), name, mimeType, directoryId, contentLength, tenantId);
            await FileDescriptorRepository.InsertAsync(fileDescriptor);
        }

        return fileDescriptor;
    }

    public virtual async Task RenameAsync(FileDescriptor file, string newName)
    {
        var existingFile = await FileDescriptorRepository.FindAsync(newName, file.DirectoryId);
        if (existingFile != null)
        {
            throw new FileAlreadyExistException(newName);
        }

        file.SetName(newName);
    }

    public virtual async Task DeleteAllAsync(Guid? directoryId)
    {
        foreach (var file in await FileDescriptorRepository.GetListAsync(directoryId))
        {
            await DeleteAsync(file);
        }
    }

    public virtual async Task DeleteAsync(FileDescriptor file)
    {
        await BlobContainer.DeleteAsync(file.Id.ToString());
        await FileDescriptorRepository.DeleteAsync(file, cancellationToken: CancellationToken.None);
    }

    public virtual async Task MoveAsync(FileDescriptor file, Guid? newDirectoryId)
    {
        if (newDirectoryId.HasValue)
        {
            if (await DirectoryDescriptorRepository.FindAsync(newDirectoryId.Value) == null)
            {
                throw new DirectoryNotExistException();
            }
        }

        file.DirectoryId = newDirectoryId;
    }
}
