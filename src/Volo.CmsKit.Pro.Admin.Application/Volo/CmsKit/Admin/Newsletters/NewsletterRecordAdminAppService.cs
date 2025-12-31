using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Volo.Abp.Content;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Uow;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Newsletters;
using Volo.CmsKit.Newsletters.Helpers;
using Volo.CmsKit.Permissions;
using Volo.CmsKit.Public.Newsletters;
using DistributedCacheEntryOptions = Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions;

namespace Volo.CmsKit.Admin.Newsletters;

[RequiresFeature(CmsKitProFeatures.NewsletterEnable)]
[RequiresGlobalFeature(NewslettersFeature.Name)]
[Authorize(CmsKitProAdminPermissions.Newsletters.Default)]
public class NewsletterRecordAdminAppService : CmsKitProAdminAppService, INewsletterRecordAdminAppService
{
    protected INewsletterRecordRepository NewsletterRecordsRepository { get; }

    protected NewsletterRecordManager NewsletterRecordManager { get; }

    protected SecurityCodeProvider SecurityCodeProvider { get; }
    
    protected IDistributedCache<NewsletterDownloadTokenCacheItem, string> DownloadTokenCache { get; }
    protected IDistributedCache<ImportInvalidNewslettersCacheItem, string> ImportInvalidNewslettersCache { get; }

    public NewsletterRecordAdminAppService(
        INewsletterRecordRepository newsletterRecordsRepository,
        NewsletterRecordManager newsletterRecordManager,
        SecurityCodeProvider securityCodeProvider, 
        IDistributedCache<NewsletterDownloadTokenCacheItem, string> downloadTokenCache, 
        IDistributedCache<ImportInvalidNewslettersCacheItem, string> importInvalidNewslettersCache)
    {
        NewsletterRecordsRepository = newsletterRecordsRepository;
        NewsletterRecordManager = newsletterRecordManager;
        SecurityCodeProvider = securityCodeProvider;
        DownloadTokenCache = downloadTokenCache;
        ImportInvalidNewslettersCache = importInvalidNewslettersCache;
    }

    public virtual async Task<PagedResultDto<NewsletterRecordDto>> GetListAsync(GetNewsletterRecordsRequestInput input)
    {
        var count = await NewsletterRecordsRepository.GetCountByFilterAsync(input.Preference, input.Source, input.EmailAddress);

        var newsletterSummaries = await NewsletterRecordsRepository.GetListAsync(
            input.Preference,
            input.Source,
            input.EmailAddress,
            input.SkipCount,
            input.MaxResultCount
        );

        return new PagedResultDto<NewsletterRecordDto>(
            count,
            ObjectMapper.Map<List<NewsletterSummaryQueryResultItem>, List<NewsletterRecordDto>>(newsletterSummaries)
        );
    }

    public virtual async Task<NewsletterRecordWithDetailsDto> GetAsync(Guid id)
    {
        var newsletterRecord = await NewsletterRecordsRepository.GetAsync(id);

        return ObjectMapper.Map<NewsletterRecord, NewsletterRecordWithDetailsDto>(newsletterRecord);
    }

    public virtual async Task<List<NewsletterRecordCsvDto>> GetNewsletterRecordsCsvDetailAsync(GetNewsletterRecordsCsvRequestInput input)
    {
        var newsletters = await NewsletterRecordsRepository.GetListAsync(input.Preference, input.Source, input.EmailAddress,0, int.MaxValue);

        var newsletterRecordsCsvDto = new List<NewsletterRecordCsvDto>();

        foreach (var newsletter in newsletters)
        {
            var emailAddress = newsletter.EmailAddress;
            var securityCode = SecurityCodeProvider.GetSecurityCode(emailAddress);
            foreach (var preference in newsletter.Preferences)
            {
                newsletterRecordsCsvDto.Add(new NewsletterRecordCsvDto() {
                    EmailAddress = emailAddress,
                    SecurityCode = securityCode,
                    Preference = preference
                });
            }
        }

        return newsletterRecordsCsvDto;
    }

    public virtual async Task<List<string>> GetNewsletterPreferencesAsync()
    {
        var newsletterPreferences = await NewsletterRecordManager.GetNewsletterPreferencesAsync();

        return newsletterPreferences.Select(newsletterPreference => newsletterPreference.Preference).ToList();
    }

    public virtual async Task<List<NewsletterPreferenceDetailsDto>> GetNewsletterPreferencesAsync(string emailAddress)
    {
        var newsletterRecord = await NewsletterRecordsRepository.FindByEmailAddressAsync(emailAddress);

        if (newsletterRecord is null)
        {
            return new List<NewsletterPreferenceDetailsDto>();
        }

        var newsletterPreferences = await NewsletterRecordManager.GetNewsletterPreferencesAsync();

        var newsletterPreferencesDto = new List<NewsletterPreferenceDetailsDto>();

        foreach (var preference in newsletterPreferences)
        {
            newsletterPreferencesDto.Add(new NewsletterPreferenceDetailsDto
            {
                Preference = preference.Preference,
                DisplayPreference = preference.DisplayPreference.Localize(StringLocalizerFactory),
                IsSelectedByEmailAddress = newsletterRecord.Preferences.Any(x => x.Preference == preference.Preference),
                Definition = preference.Definition?.Localize(StringLocalizerFactory)
            });
        }

        return newsletterPreferencesDto;
    }
    
    [AllowAnonymous]
    public virtual async Task<IRemoteStreamContent> GetCsvResponsesAsync(GetNewsletterRecordsCsvRequestInput input)
    {
        await CheckDownloadTokenAsync(input.Token);
        
        var newsletters = await GetNewsletterRecordsCsvDetailAsync(input);

        var csvConfiguration = new CsvConfiguration(new CultureInfo(CultureInfo.CurrentUICulture.Name));
        using (var memoryStream = new MemoryStream())
        {
            using (var streamWriter = new StreamWriter(stream: memoryStream, encoding: new UTF8Encoding(true)))
            {
                using (var csvWriter = new CsvWriter(streamWriter, csvConfiguration))
                {
                    await csvWriter.WriteRecordsAsync(newsletters);

                    await streamWriter.FlushAsync();
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    var ms = new MemoryStream();
                    await memoryStream.CopyToAsync(ms);
                    ms.Seek(0, SeekOrigin.Begin);

                    return new RemoteStreamContent(
                        ms,
                        fileName: "NewsletterEmails.csv",
                        contentType: "text/csv");
                }
            }
        }
    }

    [Authorize(CmsKitProAdminPermissions.Newsletters.EditPreferences)]
    public virtual async Task UpdatePreferencesAsync(UpdatePreferenceInput input)
    {
        var newsletterRecord = await NewsletterRecordsRepository.FindByEmailAddressAsync(input.EmailAddress);
        if (newsletterRecord is null)
        {
            return;
        }

        var isExistingEnabledPreference = input.PreferenceDetails.Any(x => x.IsEnabled);
        if (!isExistingEnabledPreference)
        {
            await NewsletterRecordsRepository.DeleteAsync(newsletterRecord.Id);
            return;
        }

        foreach (var preferenceDetail in input.PreferenceDetails)
        {
            var newsletterPreference = newsletterRecord.Preferences.FirstOrDefault(x => x.Preference == preferenceDetail.Preference);
            if (newsletterPreference is null && preferenceDetail.IsEnabled)
            {
                newsletterRecord.AddPreferences(
                    new NewsletterPreference(GuidGenerator.Create(),
                        newsletterRecord.Id,
                        preferenceDetail.Preference,
                        input.Source,
                        input.SourceUrl,
                        CurrentTenant.Id)
                );
            }
            else if (newsletterPreference != null && !preferenceDetail.IsEnabled)
            {
                newsletterRecord.RemovePreference(newsletterPreference.Id);
            }
        }
        await NewsletterRecordsRepository.UpdateAsync(newsletterRecord);
    }

    public async Task<DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        var token = Guid.NewGuid().ToString("N");

        await DownloadTokenCache.SetAsync(
            token,
            new NewsletterDownloadTokenCacheItem { Token = token , TenantId = CurrentTenant.Id},
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            });

        return new DownloadTokenResultDto
        {
            Token = token
        };
    }

    [AllowAnonymous]
    public virtual async Task<IRemoteStreamContent> GetImportNewslettersSampleFileAsync(GetImportNewslettersSampleFileInput input)
    {
        await CheckDownloadTokenAsync(input.Token);
        
        var sampleNewsletters = new List<ImportNewslettersFromFileDto>
        {
            new() {
                EmailAddress = "john.doe@acme.com",
                Preference = "News",
                Source = "external",
                SourceUrl = "https://acme.com"
            },
            new() {
                EmailAddress = "jane.doe@acme.com",
                Preference = "News",
                Source = "external",
                SourceUrl = "https://acme.com"
            }
        };

        return await GetImportNewslettersFileAsync(sampleNewsletters, "ImportNewslettersSampleFile");
    }
    
    [Authorize(CmsKitProAdminPermissions.Newsletters.Import)]
    public async Task<ImportNewslettersFromFileOutput> ImportNewslettersFromFileAsync(ImportNewslettersFromFileInputWithStream input)
    {
        var stream = new MemoryStream();
        await input.File.GetStream().CopyToAsync(stream);

        var invalidNewsletters = new List<InvalidImportNewslettersFromFileDto>();
        List<ImportNewslettersFromFileDto> waitingImportNewsletters;
        try
        {
            stream.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));
            waitingImportNewsletters = csv.GetRecords<ImportNewslettersFromFileDto>().ToList();
        }
        catch (Exception)
        {
            throw new UserFriendlyException(L["InvalidFileFormat"]);
        }
        
        if (!waitingImportNewsletters.Any())
        {
            throw new UserFriendlyException(L["NoNewsletterFoundInTheFile"]);
        }

        var resultDto = new ImportNewslettersFromFileOutput
        {
            AllCount = waitingImportNewsletters.Count
        };
        
        foreach (var waitingImportNewsletter in waitingImportNewsletters.GroupBy(x => x.EmailAddress))
        {
            if(string.IsNullOrWhiteSpace(waitingImportNewsletter.Key))
            {
                invalidNewsletters.AddRange(waitingImportNewsletter.Select(x => new InvalidImportNewslettersFromFileDto
                {
                    EmailAddress = x.EmailAddress,
                    Preference = x.Preference
                }));
                continue;
            }
            
            foreach (var dto in waitingImportNewsletter)
            {
                if (string.IsNullOrWhiteSpace(dto.Preference))
                {
                    invalidNewsletters.Add(new InvalidImportNewslettersFromFileDto
                    {
                        EmailAddress = dto.EmailAddress,
                        Preference = dto.Preference
                    });
                }
            }
            
            var preferences = waitingImportNewsletter.DistinctBy(x => x.Preference).Where(x => !string.IsNullOrWhiteSpace(x.Preference)).ToList();

            foreach (var preference in preferences)
            {
                try
                {
                    await NewsletterRecordManager.CreateOrUpdateAsync(waitingImportNewsletter.Key, preference.Preference, preference.Source, preference.SourceUrl, null);
                }
                catch (Exception e)
                {
                    invalidNewsletters.Add(new InvalidImportNewslettersFromFileDto() {
                        EmailAddress = preference.EmailAddress,
                        Preference = preference.Preference,
                        Source = preference.Source,
                        SourceUrl = preference.SourceUrl,
                        ErrorReason = e.Message
                    });
                }
            }
        }

        if (invalidNewsletters.Any())
        {
            var token = Guid.NewGuid().ToString("N");

            await ImportInvalidNewslettersCache.SetAsync(
                token,
                new ImportInvalidNewslettersCacheItem 
                {
                    Token = token,
                    InvalidNewsletters = invalidNewsletters
                },
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
                });

            resultDto.InvalidNewslettersDownloadToken = token;
        }
        
        resultDto.SucceededCount = resultDto.AllCount - invalidNewsletters.Count;
        resultDto.FailedCount = invalidNewsletters.Count;

        return resultDto;
    }

    [AllowAnonymous]
    public async Task<IRemoteStreamContent> GetImportInvalidNewslettersFileAsync(GetImportInvalidNewslettersFileInput input)
    {
        await CheckDownloadTokenAsync(input.Token, isInvalidNewslettersToken: true);
        
        var invalidNewslettersCacheItem = await ImportInvalidNewslettersCache.GetAsync(input.Token);
        return await GetImportNewslettersFileAsync(invalidNewslettersCacheItem.InvalidNewsletters, "InvalidNewsletters");
    }
    
    protected virtual async Task<IRemoteStreamContent> GetImportNewslettersFileAsync<T>(
        List<T> newsletters,
        string fileName) where T: ImportNewslettersFromFileDto
    {
        var csvConfiguration = new CsvConfiguration(new CultureInfo(CultureInfo.CurrentUICulture.Name));
        using (var memoryStream = new MemoryStream())
        {
            using (var streamWriter = new StreamWriter(stream: memoryStream, encoding: new UTF8Encoding(true)))
            {
                using (var csvWriter = new CsvWriter(streamWriter, csvConfiguration))
                {
                    await csvWriter.WriteRecordsAsync(newsletters);

                    await streamWriter.FlushAsync();
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    var ms = new MemoryStream();
                    await memoryStream.CopyToAsync(ms);
                    ms.Seek(0, SeekOrigin.Begin);

                    return new RemoteStreamContent(
                        ms,
                        fileName: $"{fileName}.csv",
                        contentType: "text/csv");
                }
            }
        }
    }
    
    protected virtual async Task CheckDownloadTokenAsync(string token, bool isInvalidNewslettersToken = false)
    {
        IDownloadCacheItem downloadToken;
        if (isInvalidNewslettersToken)
        {
            downloadToken = await ImportInvalidNewslettersCache.GetAsync(token);
        }
        else
        {
            downloadToken = await DownloadTokenCache.GetAsync(token);
        }
        
        if (downloadToken == null || token != downloadToken.Token)
        {
            throw new AbpAuthorizationException("Invalid download token: " + token);
        }
    }
}
