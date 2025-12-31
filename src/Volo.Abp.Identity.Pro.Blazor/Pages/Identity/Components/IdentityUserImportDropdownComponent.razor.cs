using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MimeTypes;
using Volo.Abp.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.Content;

namespace Volo.Abp.Identity.Pro.Blazor.Pages.Identity.Components;

public partial class IdentityUserImportDropdownComponent : AbpComponentBase
{
    public const string ImportInvalidUsersEndpoint = "api/identity/users/download-import-invalid-users-file";
    public const string DownloadImportSampleFileEndpoint = "api/identity/users/import-users-sample-file";
    
    protected Modal ExternalUserModal;
    protected Modal UploadFileModal;
    
    protected ExternalUserViewModel ExternalUser = new();
    protected UploadFileViewModel UploadFile = new();

    protected Validations ExternalUserValidationsRef;

    protected List<ExternalLoginProviderDto> ExternalLoginProviders = new();

    [Inject]
    protected IIdentityUserAppService IdentityUserAppService { get; set; }
    
    [Inject]
    protected IServerUrlProvider ServerUrlProvider { get; set; }
    
    [Inject]
    protected NavigationManager NavigationManager { get; set; }
    
    [Inject]
    protected UserManagementState UserManagementState { get; set; }

    protected async override Task OnInitializedAsync()
    {
        ExternalLoginProviders = await IdentityUserAppService.GetExternalLoginProvidersAsync();
        ExternalUser.Provider = ExternalLoginProviders.FirstOrDefault()?.Name;
    }

    protected virtual Task OpenExternalUserModalAsync()
    {
        if (!ExternalLoginProviders.Any())
        {
            return Message.Warn(L["NoExternalLoginProviderAvailable"]);
        }
        
        return InvokeAsync(ExternalUserModal.Show);
    }
    
    protected virtual Task CloseExternalUserModal()
    {
        ExternalUserValidationsRef.ClearAll();
        ExternalUser = new ExternalUserViewModel {Provider = ExternalLoginProviders.First().Name};
        return InvokeAsync(ExternalUserModal.Hide);
    }
    
    protected virtual async Task CloseUploadFileModal()
    {
        await UserManagementState.DataGridChangedAsync();
        await InvokeAsync(UploadFileModal.Hide);
    }
    
    protected virtual Task ClosingModal(ModalClosingEventArgs eventArgs)
    {
        // cancel close if clicked outside of modal area
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;

        return Task.CompletedTask;
    }

    protected virtual async Task ImportExternalUserAsync()
    {
        try
        {
            var validate = true;
            if (ExternalUserValidationsRef != null)
            {
                validate = await ExternalUserValidationsRef.ValidateAll();
            }
            if (validate)
            {
                await IdentityUserAppService.ImportExternalUserAsync(new ImportExternalUserInput
                {
                    Provider = ExternalUser.Provider,
                    UserNameOrEmailAddress = ExternalUser.UserNameOrEmailAddress,
                    Password = ExternalUser.Password,
                });
                
                
                await CloseExternalUserModal();
                await UserManagementState.DataGridChangedAsync();
            }

        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }
    
    protected virtual Task OpenUploadFileModalAsync(ImportUsersFromFileType fileType)
    {
        UploadFile = new UploadFileViewModel { FileType = fileType , FileName = L["ChooseFile"]};
        return InvokeAsync(UploadFileModal.Show);
    }

    protected virtual async Task OnChooseFileChangedAsync(InputFileChangeEventArgs e)
    {
        try
        {
            var file = e.File;
            UploadFile.FileName = file.Name;
            UploadFile.File = file;
        }
        catch (Exception ex)
        {
            UploadFile.FileName = L["ChooseFile"];
            UploadFile.File = null;
            await HandleErrorAsync(ex);
        }
        finally
        {
            await InvokeAsync(StateHasChanged);
        }
    }
    
    protected virtual async Task ImportUsersFromFileAsync()
    {
        try
        {
            if (UploadFile.File == null)
            {
                await Message.Warn(L["PleaseSelectAFile"]);
                return;
            }
            var result = await IdentityUserAppService.ImportUsersFromFileAsync(new ImportUsersFromFileInputWithStream
            {
                FileType = UploadFile.FileType,
                File = new RemoteStreamContent(UploadFile.File.OpenReadStream(), UploadFile.FileName, MimeTypeMap.GetMimeType(UploadFile.FileName.Split('.').Last().ToLower()))
            });

            if (result.IsAllSucceeded)
            {
                await Message.Success(L["ImportSuccessMessage"]);
                await CloseUploadFileModal();
            }
            else
            {
                if (await Message.Confirm(L["ImportFailedMessage", result.SucceededCount, result.FailedCount]))
                {
                    var baseUrl = await ServerUrlProvider.GetBaseUrlAsync(IdentityProRemoteServiceConsts.RemoteServiceName);
                    NavigationManager.NavigateTo($"{baseUrl}{ImportInvalidUsersEndpoint}/?token={result.InvalidUsersDownloadToken}", forceLoad: true);
                }
            }
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }
    
    protected virtual async Task DownloadImportSampleFileAsync()
    {
        try
        {
            var downloadToken = await IdentityUserAppService.GetDownloadTokenAsync();
            var baseUrl = await ServerUrlProvider.GetBaseUrlAsync(IdentityProRemoteServiceConsts.RemoteServiceName);
            var culture = CultureInfo.CurrentUICulture.Name ?? CultureInfo.CurrentCulture.Name;
            if(!culture.IsNullOrEmpty())
            {
                culture = "&culture=" + culture;
            }
            NavigationManager.NavigateTo($"{baseUrl}{DownloadImportSampleFileEndpoint}?fileType={(int)UploadFile.FileType}&token={downloadToken.Token}{culture}", forceLoad: true);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }
}

public class ExternalUserViewModel
{
    [Required] 
    public string Provider { get; set; }

    [Required]
    public string UserNameOrEmailAddress { get; set; }
        
    public string Password { get; set; }
}

public class UploadFileViewModel
{
    public ImportUsersFromFileType FileType { get; set; }
    
    public IBrowserFile File { get; set; }

    public string FileName { get; set; }

    public string Accept
    {
        get {
            switch (FileType)
            {
                case ImportUsersFromFileType.Excel:
                    default:
                    return ".xlsx,.xls";
                case ImportUsersFromFileType.Csv:
                    return ".csv";
            }
        }
    }
}