using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MimeTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.Content;
using Volo.FileManagement.Authorization;
using Volo.FileManagement.Directories;
using Volo.FileManagement.Files;
using BreadcrumbItem = Volo.Abp.BlazoriseUI.BreadcrumbItem;

namespace Volo.FileManagement.Blazor.Pages.FileManagement;

public partial class FileManagement
{
    protected const string DownloadEndpoint = "api/file-management/file-descriptor/download";

    [Inject]
    protected IDirectoryDescriptorAppService DirectoryDescriptorAppService { get; set; }

    [Inject]
    protected IFileDescriptorAppService FileDescriptorAppService { get; set; }

    //TODO: Enable again
    // [Inject]
    // protected IOptions<AbpRemoteServiceOptions> RemoteServiceOptions { get; set; }

    [Inject]
    protected IServerUrlProvider ServerUrlProvider { get; set; }

    protected int CurrentPage = 1;

    protected virtual int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;

    protected List<BreadcrumbItem> BreadcrumbItems;

    protected Modal CreateDirectoryModal;

    protected Validations CreateDirectoryValidationsRef;

    protected CreateDirectoryInput CreateDirectoryInput;

    protected Modal RenameDirectoryModal;

    protected Validations RenameDirectorValidationsRef;

    protected RenameDirectoryInput RenameDirectoryInput;

    protected Modal RenameFileModal;

    protected Validations RenameFileValidationsRef;

    protected RenameFileInput RenameFileInput;

    protected Guid RenameContentId;

    protected Modal UploadFilesModal;

    protected Modal MoveContentModal;

    protected List<DirectoryContentDto> MoveDirectoryContents;

    protected Guid MovingContentId;

    protected string MovingContentConcurrencyStamp;

    protected Guid? NewDirectoryId;

    protected bool MoveDirectory;

    protected bool ReBuildRoute;

    protected bool HasFileCreatePermission;

    protected bool HasFileUpdatePermission;

    protected bool HasFileDeletePermission;

    protected bool HasDirectoryCreatePermission;

    protected bool HasDirectoryUpdatePermission;

    protected bool HasDirectoryDeletePermission;

    protected PagedResultDto<DirectoryContentDto> DirectoryContents;

    protected List<DirectoryDescriptorTreeModel> DirectoryDescriptorTree;

    protected DirectoryContentRequestInput DirectoryContentRequestInput;

    protected List<DirectoryContentDto> DirectoryRoutes;

    protected List<FileManagementUploadFileModel> FileManagementUploadFileModels;

    protected List<DirectoryContentDto> MoveFileDirectoryRoutes;

    public FileManagement()
    {
        BreadcrumbItems = new List<BreadcrumbItem>();
        CreateDirectoryInput = new CreateDirectoryInput();
        RenameDirectoryInput = new RenameDirectoryInput();
        DirectoryContentRequestInput = new DirectoryContentRequestInput();
        RenameFileInput = new RenameFileInput();
        DirectoryRoutes = new List<DirectoryContentDto>();
        FileManagementUploadFileModels = new List<FileManagementUploadFileModel>();
        MoveFileDirectoryRoutes = new List<DirectoryContentDto>();
    }

    protected async override Task OnInitializedAsync()
    {
        HasFileCreatePermission = await AuthorizationService.IsGrantedAsync(FileManagementPermissions.FileDescriptor.Create);
        HasFileUpdatePermission = await AuthorizationService.IsGrantedAsync(FileManagementPermissions.FileDescriptor.Update);
        HasFileDeletePermission = await AuthorizationService.IsGrantedAsync(FileManagementPermissions.FileDescriptor.Delete);

        HasDirectoryCreatePermission = await AuthorizationService.IsGrantedAsync(FileManagementPermissions.DirectoryDescriptor.Create);
        HasDirectoryUpdatePermission = await AuthorizationService.IsGrantedAsync(FileManagementPermissions.DirectoryDescriptor.Update);
        HasDirectoryDeletePermission = await AuthorizationService.IsGrantedAsync(FileManagementPermissions.DirectoryDescriptor.Delete);

        BreadcrumbItems.Add(new BreadcrumbItem(L["Menu:FileManagement"]));

        await InitDirectoryDescriptorTreeAsync();
        await GetDirectoryContentAsync();
    }

    //TODO: Child nodes should be lazily loaded, but there is currently no tree component that supports lazy loading
    protected virtual async Task GetDirectoryDescriptorTreeAsync(IEnumerable<DirectoryDescriptorInfoDto> descriptorInfos, List<DirectoryDescriptorTreeModel> treeModels)
    {
        foreach (var descriptorInfo in descriptorInfos)
        {
            var treeModel = new DirectoryDescriptorTreeModel(descriptorInfo.Id, descriptorInfo.Name,
                descriptorInfo.ParentId);

            if (descriptorInfo.HasChildren)
            {
                var result = await DirectoryDescriptorAppService.GetListAsync(descriptorInfo.Id);
                await GetDirectoryDescriptorTreeAsync(result.Items, treeModel.Children);
            }

            treeModels.Add(treeModel);
            DirectoryDescriptorTree.First().DirectoryDescriptorInfos.Add(treeModel);
        }
    }

    protected virtual async Task InitDirectoryDescriptorTreeAsync()
    {
        DirectoryDescriptorTree = new List<DirectoryDescriptorTreeModel>
            {
                new(null, L["AllFiles"].Value, null,false)
                {
                    Collapsed = false,
                    DirectoryDescriptorInfos = new List<DirectoryDescriptorTreeModel>()
                }
            };
        await GetDirectoryDescriptorTreeAsync((await DirectoryDescriptorAppService.GetListAsync(null)).Items, DirectoryDescriptorTree.First().Children);
        if (DirectoryContentRequestInput.Id.HasValue && DirectoryDescriptorTree.First().HasChildren)
        {
            CollapseOrExpandDirectoryNode(false, DirectoryContentRequestInput.Id);
        }
    }

    protected virtual async Task OnSelectedDirectoryNodeChangedAsync(DirectoryDescriptorTreeModel directoryNode)
    {
        DirectoryRoutes.Clear();
        CurrentPage = 1;
        DirectoryContentRequestInput.Id = directoryNode.Id;
        BuildDirectoryRoutesBySelectedDirectoryNode(directoryNode);
        await GetDirectoryContentAsync();
    }

    protected virtual void BuildDirectoryRoutesBySelectedDirectoryNode(DirectoryDescriptorTreeModel directoryNode)
    {
        if (!directoryNode.Id.HasValue)
        {
            return;
        }
        if (directoryNode.ParentId.HasValue)
        {
            BuildDirectoryRoutesBySelectedDirectoryNode(DirectoryDescriptorTree.First().DirectoryDescriptorInfos.First(x => x.Id == directoryNode.ParentId));
        }
        DirectoryRoutes.Add(new DirectoryContentDto
        {
            Name = directoryNode.Name,
            Id = directoryNode.Id.Value,
            IsDirectory = true
        });
    }

    protected virtual async Task OnDirectoryContentDataGridReadAsync(DataGridReadDataEventArgs<DirectoryContentDto> e)
    {
        DirectoryContentRequestInput.Sorting = e.Columns
            .Where(c => c.SortDirection != SortDirection.Default)
            .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
            .JoinAsString(",");
        CurrentPage = e.Page;

        await GetDirectoryContentAsync();
    }

    protected virtual async Task OnSearchDirectoryContentAsync()
    {
        CurrentPage = 1;
        await GetDirectoryContentAsync();
    }

    protected virtual async Task GetDirectoryContentAsync()
    {
        DirectoryContentRequestInput.SkipCount = (CurrentPage - 1) * PageSize;
        var result = await DirectoryDescriptorAppService.GetContentAsync(DirectoryContentRequestInput);
        DirectoryContents = result;
    }

    protected virtual async Task OnDirectoryRoutingAsync(DirectoryContentDto content)
    {
        DirectoryContentRequestInput.Id = content?.Id;
        if (content != null)
        {
            var index = DirectoryRoutes.IndexOf(content) + 1;
            CollapseOrExpandDirectoryNode(true, content.Id);
            CollapseOrExpandDirectoryNode(false, content.Id);
            DirectoryRoutes.RemoveRange(index, DirectoryRoutes.Count - index);
        }
        else
        {
            CollapseOrExpandDirectoryNode(true, null);
            DirectoryRoutes.Clear();
        }
        await GetDirectoryContentAsync();
    }

    protected virtual async Task DeleteEntityAsync(DirectoryContentDto content)
    {
        if (content.IsDirectory)
        {
            await DeleteDirectoryAsync(content.Id);
        }
        else
        {
            await DeleteFileAsync(content.Id);
        }
    }

    protected virtual async Task DeleteDirectoryAsync(Guid directoryId, bool confirm = false)
    {
        try
        {
            if (confirm)
            {
                if (!(await Message.Confirm(L["DirectoryDeleteConfirmationMessage"])))
                {
                    return;
                }
            }

            await CheckPolicyAsync(FileManagementPermissions.DirectoryDescriptor.Delete);
            await DirectoryDescriptorAppService.DeleteAsync(directoryId);

            var routedDirectory = DirectoryRoutes.FindIndex(x => x.Id == directoryId);
            if (routedDirectory >= 0)
            {
                DirectoryRoutes.RemoveRange(routedDirectory, DirectoryRoutes.Count - routedDirectory);
            }

            DirectoryContentRequestInput.Id = DirectoryRoutes.LastOrDefault()?.Id;
            await InitDirectoryDescriptorTreeAsync();
            await GetDirectoryContentAsync();
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task DeleteFileAsync(Guid fileId, bool confirm = false)
    {
        try
        {
            if (confirm)
            {
                if (!(await Message.Confirm(L["FileDeleteConfirmationMessage"])))
                {
                    return;
                }
            }

            await CheckPolicyAsync(FileManagementPermissions.FileDescriptor.Delete);
            await FileDescriptorAppService.DeleteAsync(fileId);

            await GetDirectoryContentAsync();
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual string GetDeleteConfirmationMessage(DirectoryContentDto content)
    {
        return string.Format(content.IsDirectory ? L["DirectoryDeleteConfirmationMessage"] : L["FileDeleteConfirmationMessage"]);
    }

    protected virtual async Task OpenCreateDirectoryModalAsync()
    {
        CreateDirectoryValidationsRef?.ClearAll();

        CreateDirectoryInput = new CreateDirectoryInput();

        await InvokeAsync(StateHasChanged);

        await CreateDirectoryModal.Show();
    }

    protected virtual async Task CloseCreateDirectoryModalAsync()
    {
        await CreateDirectoryModal.Hide();
    }

    protected virtual Task ClosingCreateDirectoryModal(ModalClosingEventArgs eventArgs)
    {
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;
        return Task.CompletedTask;
    }

    protected virtual async Task CreateDirectoryAsync()
    {
        try
        {
            await CheckPolicyAsync(FileManagementPermissions.DirectoryDescriptor.Create);

            var validate = true;
            if (CreateDirectoryValidationsRef != null)
            {
                validate = await CreateDirectoryValidationsRef.ValidateAll();
            }
            if (validate)
            {
                CreateDirectoryInput.ParentId = DirectoryRoutes.LastOrDefault()?.Id;
                await DirectoryDescriptorAppService.CreateAsync(CreateDirectoryInput);

                await InitDirectoryDescriptorTreeAsync();
                await GetDirectoryContentAsync();

                await CreateDirectoryModal.Hide();
            }
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task OpenRenameModalAsync(DirectoryContentDto content)
    {
        RenameContentId = content.Id;

        if (content.IsDirectory)
        {
            await OpenRenameDirectoryModalAsync(RenameContentId, content.Name, content.ConcurrencyStamp);
        }
        else
        {
            RenameFileValidationsRef?.ClearAll();

            RenameFileInput = new RenameFileInput
            {
                Name = content.Name,
                ConcurrencyStamp = content.ConcurrencyStamp
            };

            await InvokeAsync(StateHasChanged);

            await RenameFileModal.Show();
        }
    }

    protected virtual async Task OpenRenameDirectoryModalAsync(Guid directoryId, string directoryName, string concurrencyStamp)
    {
        RenameContentId = directoryId;

        RenameDirectorValidationsRef?.ClearAll();

        RenameDirectoryInput = new RenameDirectoryInput
        {
            Name = directoryName,
            ConcurrencyStamp = concurrencyStamp
        };

        await InvokeAsync(StateHasChanged);

        await RenameDirectoryModal.Show();
    }

    protected virtual async Task CloseRenameModalAsync(bool isDirectory)
    {
        if (isDirectory)
        {
            await RenameDirectoryModal.Hide();
        }
        else
        {
            await RenameFileModal.Hide();
        }
    }

    protected virtual Task ClosingRenameDirectoryModal(ModalClosingEventArgs eventArgs)
    {
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;
        return Task.CompletedTask;
    }

    protected virtual Task ClosingRenameModal(ModalClosingEventArgs eventArgs)
    {
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;
        return Task.CompletedTask;
    }

    protected virtual async Task RenameDirectoryAsync()
    {
        try
        {
            await CheckPolicyAsync(FileManagementPermissions.DirectoryDescriptor.Update);

            var validate = true;
            if (RenameDirectorValidationsRef != null)
            {
                validate = await RenameDirectorValidationsRef.ValidateAll();
            }
            if (validate)
            {
                CreateDirectoryInput.ParentId = DirectoryContentRequestInput.Id;
                await DirectoryDescriptorAppService.RenameAsync(RenameContentId, RenameDirectoryInput);

                ChangeDirectoryRouteName(RenameContentId, RenameDirectoryInput.Name);
                await InitDirectoryDescriptorTreeAsync();
                await GetDirectoryContentAsync();
                await RenameDirectoryModal.Hide();
            }
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual void ChangeDirectoryRouteName(Guid routeId, string name)
    {
        var route = DirectoryRoutes.FirstOrDefault(x => x.Id == RenameContentId);
        if (route != null)
        {
            route.Name = name;
        }
    }

    protected virtual async Task RenameFileAsync()
    {
        try
        {
            await CheckPolicyAsync(FileManagementPermissions.FileDescriptor.Update);

            var validate = true;
            if (RenameFileValidationsRef != null)
            {
                validate = await RenameFileValidationsRef.ValidateAll();
            }
            if (validate)
            {
                CreateDirectoryInput.ParentId = DirectoryContentRequestInput.Id;
                await FileDescriptorAppService.RenameAsync(RenameContentId, RenameFileInput);

                await GetDirectoryContentAsync();

                await RenameFileModal.Hide();
            }
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task OpenFolderAsync(DirectoryContentDto content)
    {
        DirectoryContentRequestInput.Id = content.Id;
        DirectoryRoutes.Add(content);
        CollapseOrExpandDirectoryNode(false, content.Id);
        await GetDirectoryContentAsync();
    }

    protected virtual void CollapseOrExpandDirectoryNode(bool collapsed, Guid? id)
    {
        if (!id.HasValue)
        {
            DirectoryDescriptorTree.First().DirectoryDescriptorInfos.ForEach(x => x.ChangeCollapsed(true));
            DirectoryDescriptorTree.First().ChangeCollapsed(false);
            return;
        }

        var directoryNode = DirectoryDescriptorTree.First().DirectoryDescriptorInfos.First(x => x.Id == id);
        if (collapsed)
        {
            directoryNode.ChangeCollapsed(true);
            foreach (var child in directoryNode.Children)
            {
                CollapseOrExpandDirectoryNode(true, child.Id);
            }
        }
        else
        {
            directoryNode.ChangeCollapsed(false);
            if (directoryNode.ParentId.HasValue)
            {
                CollapseOrExpandDirectoryNode(false, directoryNode.ParentId);
            }
        }
    }

    protected virtual async Task GoUpFolderAsync()
    {
        var parentRoute = DirectoryRoutes.ElementAtOrDefault(DirectoryRoutes.Count - 2);
        DirectoryContentRequestInput.Id = parentRoute?.Id;
        CollapseOrExpandDirectoryNode(true, DirectoryRoutes.Last().Id);
        DirectoryRoutes.RemoveAt(DirectoryRoutes.Count - 1);
        await GetDirectoryContentAsync();
    }

    protected virtual async Task OpenUploadFilesModalAsync()
    {
        FileManagementUploadFileModels.Clear();
        await UploadFilesModal.Show();
        await InvokeAsync(StateHasChanged);
    }

    protected virtual async Task CloseUploadFilesModalAsync()
    {
        await UploadFilesModal.Hide();
    }

    protected virtual Task ClosingUploadFilesModal(ModalClosingEventArgs eventArgs)
    {
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;
        return Task.CompletedTask;
    }

    protected virtual async Task UploadFilesAsync()
    {
        try
        {
            await CheckPolicyAsync(FileManagementPermissions.FileDescriptor.Create);

            var preFilesInfo = await FileDescriptorAppService.GetPreInfoAsync(FileManagementUploadFileModels
                .Select(file => new FileUploadPreInfoRequest { FileName = file.Name, Size = file.Stream.Length, DirectoryId = DirectoryContentRequestInput.Id }).ToList());

            if (preFilesInfo.Any(x => x.DoesExist))
            {
                var message = L["FilesWillBeOverrided", string.Join(',', preFilesInfo.Where(x => x.DoesExist).Select(x => x.FileName))];

                if (!await Message.Confirm(message))
                {
                    return;
                }
            }

            foreach (var fileModel in FileManagementUploadFileModels)
            {
                fileModel.Stream.Position = 0;
                await FileDescriptorAppService.CreateAsync(DirectoryRoutes.LastOrDefault()?.Id, new CreateFileInputWithStream
                {
                    File = new RemoteStreamContent(fileModel.Stream, fileModel.Name, MimeTypeMap.GetMimeType(fileModel.Name.Split('.').Last().ToLower())),
                    Name = fileModel.Name
                });
            }

            await GetDirectoryContentAsync();

            await CloseUploadFilesModalAsync();
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task SelectedFilesAsync(InputFileChangeEventArgs files)
    {
        try
        {
            var multipleFiles = files.GetMultipleFiles(int.MaxValue);

            var preFilesInfo = await FileDescriptorAppService.GetPreInfoAsync(multipleFiles
                .Select(file => new FileUploadPreInfoRequest { FileName = file.Name, Size = file.Size, DirectoryId = DirectoryContentRequestInput.Id }).ToList());

            if (preFilesInfo.Any(x => x.DoesExist))
            {
                await Message.Info(L["FilesAlreadyExist"]);
            }

            foreach (var file in multipleFiles)
            {
                if (FileManagementUploadFileModels.Any(x => x.Name == file.Name))
                {
                    continue;
                }

                var fileModel = new FileManagementUploadFileModel
                {
                    ContentType = file.ContentType,
                    Name = file.Name,
                    Stream = new MemoryStream()
                };

                await file.OpenReadStream(file.Size).CopyToAsync(fileModel.Stream);

                fileModel.Stream.Position = 0;

                if (fileModel.IsImages())
                {
                    fileModel.Url = $"data:{file.ContentType};base64,{Convert.ToBase64String(fileModel.Stream.ToArray())}";
                }

                FileManagementUploadFileModels.Add(fileModel);
            }

            await InvokeAsync(StateHasChanged);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual void RemoveSelectedFile(FileManagementUploadFileModel file)
    {
        FileManagementUploadFileModels.Remove(file);
        InvokeAsync(StateHasChanged);
    }

    protected virtual async Task DownloadFile(DirectoryContentDto content)
    {
        var baseUrl = await ServerUrlProvider.GetBaseUrlAsync(FileManagementRemoteServiceConsts.RemoteServiceName);
        var downloadToken = await FileDescriptorAppService.GetDownloadTokenAsync(content.Id);
        var culture = CultureInfo.CurrentUICulture.Name ?? CultureInfo.CurrentCulture.Name;
        if(!culture.IsNullOrEmpty())
        {
            culture = "&culture=" + culture;
        }
        NavigationManager.NavigateTo($"{baseUrl}{DownloadEndpoint}/{content.Id}?token={downloadToken.Token}{culture}", forceLoad: true);
    }

    protected virtual async Task OpenMoveContentModalAsync(Guid contentId, string concurrencyStamp, bool isDirectory = false, bool reBuildRoute = false)
    {
        MoveDirectory = isDirectory;
        MovingContentId = contentId;
        MovingContentConcurrencyStamp = concurrencyStamp;

        ReBuildRoute = reBuildRoute;

        MoveFileDirectoryRoutes.Clear();
        await GetMoveDirectoryContents(null);

        await MoveContentModal.Show();
    }

    protected virtual async Task CloseMoveContentModalAsync()
    {
        await MoveContentModal.Hide();
    }

    protected virtual Task ClosingMoveContentModal(ModalClosingEventArgs eventArgs)
    {
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;
        return Task.CompletedTask;
    }

    protected virtual async Task GetMoveDirectoryContents(DirectoryContentDto content)
    {
        if (content != null)
        {
            MoveFileDirectoryRoutes.Add(content);
        }

        NewDirectoryId = content?.Id;
        var result = await DirectoryDescriptorAppService.GetContentAsync(new DirectoryContentRequestInput { Id = content?.Id });

        var filter = result.Items.Where(x => x.IsDirectory);

        if (MoveDirectory)
        {
            filter = filter.Where(x => x.Id != MovingContentId);
        }

        MoveDirectoryContents = filter.ToList();

        await InvokeAsync(StateHasChanged);
    }

    protected virtual async Task MoveContentAsync()
    {
        try
        {
            if (MoveDirectory)
            {
                await CheckPolicyAsync(FileManagementPermissions.DirectoryDescriptor.Update);

                await DirectoryDescriptorAppService.MoveAsync(new MoveDirectoryInput
                {
                    Id = MovingContentId,
                    NewParentId = NewDirectoryId,
                    ConcurrencyStamp = MovingContentConcurrencyStamp,
                });

                await InitDirectoryDescriptorTreeAsync();

                if (ReBuildRoute)
                {
                    await OnSelectedDirectoryNodeChangedAsync(DirectoryDescriptorTree.First().DirectoryDescriptorInfos.First(x => x.Id == MovingContentId));
                }
            }
            else
            {
                await CheckPolicyAsync(FileManagementPermissions.FileDescriptor.Update);

                await FileDescriptorAppService.MoveAsync(new MoveFileInput
                {
                    Id = MovingContentId,
                    NewDirectoryId = NewDirectoryId,
                    ConcurrencyStamp = MovingContentConcurrencyStamp,
                });
            }

            await GetDirectoryContentAsync();
            MoveFileDirectoryRoutes.Clear();
            await CloseMoveContentModalAsync();
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task OnMoveContentDirectoryRoutingAsync(DirectoryContentDto content)
    {
        if (content != null)
        {
            var index = MoveFileDirectoryRoutes.IndexOf(content);
            MoveFileDirectoryRoutes.RemoveRange(index, MoveFileDirectoryRoutes.Count - index);
        }
        else
        {
            MoveFileDirectoryRoutes.Clear();
        }
        await GetMoveDirectoryContents(content);
    }

    protected virtual async Task CheckPolicyAsync(string policyName)
    {
        await AuthorizationService.CheckAsync(policyName);
    }

    protected virtual string FormatBytes(long bytes)
    {
        var unit = 1024;
        if (bytes < unit) { return $"{bytes} B"; }
        
        var exp = (int)(Math.Log(bytes) / Math.Log(unit));
        return $"{bytes / Math.Pow(unit, exp):F2} {("KMGTPE")[exp - 1]}B";
    }
}
