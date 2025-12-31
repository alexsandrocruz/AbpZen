using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using Volo.CmsKit.Public.Newsletters;

namespace Volo.CmsKit.Admin.Newsletters;

public interface INewsletterRecordAdminAppService : IApplicationService
{
    Task<PagedResultDto<NewsletterRecordDto>> GetListAsync(GetNewsletterRecordsRequestInput input);

    Task<NewsletterRecordWithDetailsDto> GetAsync(Guid id);

    Task<List<NewsletterRecordCsvDto>> GetNewsletterRecordsCsvDetailAsync(GetNewsletterRecordsCsvRequestInput input);

    Task<List<string>> GetNewsletterPreferencesAsync();

    Task<List<NewsletterPreferenceDetailsDto>> GetNewsletterPreferencesAsync(string emailAddress);

    Task<IRemoteStreamContent> GetCsvResponsesAsync(GetNewsletterRecordsCsvRequestInput input);

    Task UpdatePreferencesAsync(UpdatePreferenceInput input);

    Task<DownloadTokenResultDto> GetDownloadTokenAsync();

    Task<IRemoteStreamContent> GetImportNewslettersSampleFileAsync(GetImportNewslettersSampleFileInput input);

    Task<ImportNewslettersFromFileOutput> ImportNewslettersFromFileAsync(ImportNewslettersFromFileInputWithStream input);

    Task<IRemoteStreamContent> GetImportInvalidNewslettersFileAsync(GetImportInvalidNewslettersFileInput input);
}