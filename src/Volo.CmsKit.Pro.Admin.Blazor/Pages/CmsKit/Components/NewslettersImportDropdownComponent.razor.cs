using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MimeTypes;
using Volo.Abp.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.Content;
using Volo.CmsKit.Admin;
using Volo.CmsKit.Admin.Newsletters;

namespace Volo.CmsKit.Pro.Admin.Blazor.Pages.CmsKit.Components;

public partial class NewslettersImportDropdownComponent : AbpComponentBase
{
    public const string ImportInvalidUsersEndpoint = "api/cms-kit-admin/newsletter/download-import-invalid-newsletters-file";
    public const string DownloadImportSampleFileEndpoint = "api/cms-kit-admin/newsletter/import-newsletters-sample-file";
    
    [Inject]
    protected INewsletterRecordAdminAppService NewsletterRecordAdminAppService { get; set; }
    
    [Inject]
    protected NavigationManager NavigationManager { get; set; }
    
    [Inject]
    protected IServerUrlProvider ServerUrlProvider { get; set; }
    
    [Inject]
    protected NewslettersManagementState NewslettersManagementState { get; set; }
    
    protected Modal ImportModal;
    
    protected UploadFileViewModel UploadFile = new();
    
    protected virtual async Task DownloadImportSampleFileAsync()
    {
        try
        {
            var downloadToken = await NewsletterRecordAdminAppService.GetDownloadTokenAsync();
            var baseUrl = await ServerUrlProvider.GetBaseUrlAsync(CmsKitAdminRemoteServiceConsts.RemoteServiceName);
            var culture = CultureInfo.CurrentUICulture.Name ?? CultureInfo.CurrentCulture.Name;
            if (!culture.IsNullOrEmpty())
            {
                culture = "&culture=" + culture;
            }

            NavigationManager.NavigateTo($"{baseUrl}{DownloadImportSampleFileEndpoint}?token={downloadToken.Token}{culture}", forceLoad: true);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
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
    
    protected virtual async Task ImportNewslettersFromFileAsync()
    {
        try
        {
            if (UploadFile.File == null)
            {
                await Message.Warn(L["PleaseSelectAFile"]);
                return;
            }
            var result = await NewsletterRecordAdminAppService.ImportNewslettersFromFileAsync(new ImportNewslettersFromFileInputWithStream
            {
                File = new RemoteStreamContent(UploadFile.File.OpenReadStream(), UploadFile.FileName, MimeTypeMap.GetMimeType(UploadFile.FileName.Split('.').Last().ToLower()))
            });

            await NewslettersManagementState.DataGridChangedAsync();
            if (result.IsAllSucceeded)
            {
                await Message.Success(L["NewsletterImportSuccessMessage"]);
                await CloseImportModalAsync();
            }
            else
            {
                if (await Message.Confirm(L["NewsletterImportFailedMessage", result.SucceededCount, result.FailedCount]))
                {
                    var baseUrl = await ServerUrlProvider.GetBaseUrlAsync(CmsKitAdminRemoteServiceConsts.RemoteServiceName);
                    NavigationManager.NavigateTo($"{baseUrl}{ImportInvalidUsersEndpoint}/?token={result.InvalidNewslettersDownloadToken}", forceLoad: true);
                }
            }
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }
    
    protected virtual Task OpenImportModalAsync()
    {
        return InvokeAsync(ImportModal.Show);
    }
    
    protected virtual Task ClosingModal(ModalClosingEventArgs eventArgs)
    {
        // cancel close if clicked outside of modal area
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;

        return Task.CompletedTask;
    }
    
    protected virtual Task CloseImportModalAsync()
    {
        return InvokeAsync(ImportModal.Hide);
    }
}

public class UploadFileViewModel
{
    
    public IBrowserFile File { get; set; }

    public string FileName { get; set; }

    public readonly string Accept = ".csv";
}