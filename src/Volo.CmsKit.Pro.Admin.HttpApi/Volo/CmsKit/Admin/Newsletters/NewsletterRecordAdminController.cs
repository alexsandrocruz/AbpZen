using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Permissions;
using Volo.CmsKit.Public.Newsletters;

namespace Volo.CmsKit.Admin.Newsletters;

[RequiresFeature(CmsKitProFeatures.NewsletterEnable)]
[RequiresGlobalFeature(typeof(NewslettersFeature))]
[RemoteService(Name = CmsKitAdminRemoteServiceConsts.RemoteServiceName)]
[Area(CmsKitProAdminRemoteServiceConsts.ModuleName)]
[Route("api/cms-kit-admin/newsletter")]
[Authorize(CmsKitProAdminPermissions.Newsletters.Default)]
public class NewsletterRecordAdminController : CmsKitProAdminController, INewsletterRecordAdminAppService
{
    protected INewsletterRecordAdminAppService NewsletterRecordAdminAppService { get; }

    public NewsletterRecordAdminController(INewsletterRecordAdminAppService newsletterRecordAdminAppService)
    {
        NewsletterRecordAdminAppService = newsletterRecordAdminAppService;
    }

    [HttpGet]
    public Task<PagedResultDto<NewsletterRecordDto>> GetListAsync(GetNewsletterRecordsRequestInput input)
    {
        return NewsletterRecordAdminAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("{id}")]
    public Task<NewsletterRecordWithDetailsDto> GetAsync(Guid id)
    {
        return NewsletterRecordAdminAppService.GetAsync(id);
    }

    [HttpGet]
    [Route("csv-detail")]
    public Task<List<NewsletterRecordCsvDto>> GetNewsletterRecordsCsvDetailAsync(GetNewsletterRecordsCsvRequestInput input)
    {
        return NewsletterRecordAdminAppService.GetNewsletterRecordsCsvDetailAsync(input);
    }

    [HttpGet]
    [Route("preferences")]
    public Task<List<string>> GetNewsletterPreferencesAsync()
    {
        return NewsletterRecordAdminAppService.GetNewsletterPreferencesAsync();
    }

    [HttpGet]
    [Route("preferences/{emailAddress}")]
    public Task<List<NewsletterPreferenceDetailsDto>> GetNewsletterPreferencesAsync(string emailAddress)
    {
        return NewsletterRecordAdminAppService.GetNewsletterPreferencesAsync(emailAddress);
    }

    [HttpGet]
    [Route("export-csv")]
    [AllowAnonymous]
    public virtual async Task<IRemoteStreamContent> GetCsvResponsesAsync(GetNewsletterRecordsCsvRequestInput input)
    {
        return await NewsletterRecordAdminAppService.GetCsvResponsesAsync(input);
    }

    [HttpPut]
    [Route("preferences")]
    public Task UpdatePreferencesAsync(UpdatePreferenceInput input)
    {
        return NewsletterRecordAdminAppService.UpdatePreferencesAsync(input);
    }

    [HttpGet]
    [Route("download-token")]
    public Task<DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        return NewsletterRecordAdminAppService.GetDownloadTokenAsync();
    }

    [HttpGet]
    [Route("import-newsletters-sample-file")]
    [AllowAnonymous]
    public Task<IRemoteStreamContent> GetImportNewslettersSampleFileAsync(GetImportNewslettersSampleFileInput input)
    {
        return NewsletterRecordAdminAppService.GetImportNewslettersSampleFileAsync(input);
    }

    [HttpPost]
    [Route("import-newsletters-from-file")]
    public Task<ImportNewslettersFromFileOutput> ImportNewslettersFromFileAsync([FromForm]ImportNewslettersFromFileInputWithStream input)
    {
        return NewsletterRecordAdminAppService.ImportNewslettersFromFileAsync(input);
    }

    [HttpGet]
    [Route("download-import-invalid-newsletters-file")]
    [AllowAnonymous]
    public Task<IRemoteStreamContent> GetImportInvalidNewslettersFileAsync(GetImportInvalidNewslettersFileInput input)
    {
        return NewsletterRecordAdminAppService.GetImportInvalidNewslettersFileAsync(input);
    }
}
