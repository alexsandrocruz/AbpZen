using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Features;
using Volo.Abp.LanguageManagement.Dto;
using Volo.Abp.Localization;
using Volo.Abp.Localization.External;

namespace Volo.Abp.LanguageManagement;

[RequiresFeature(LanguageManagementFeatures.Enable)]
[Authorize(LanguageManagementPermissions.LanguageTexts.Default)]
public class LanguageTextAppService : LanguageAppServiceBase, ILanguageTextAppService
{
    protected ILanguageTextRepository LanguageTextRepository { get; }
    protected IStringLocalizerFactory LocalizerFactory { get; }
    protected AbpLocalizationOptions AbpLocalizationOptions { get; }
    protected IExternalLocalizationStore ExternalLocalizationStore { get; }

    public LanguageTextAppService(
        ILanguageTextRepository languageTextRepository,
        IOptions<AbpLocalizationOptions> abpLocalizationOptions,
        IStringLocalizerFactory localizerFactory,
        IExternalLocalizationStore externalLocalizationStore)
    {
        LanguageTextRepository = languageTextRepository;
        LocalizerFactory = localizerFactory;
        ExternalLocalizationStore = externalLocalizationStore;
        AbpLocalizationOptions = abpLocalizationOptions.Value;
    }

    public virtual async Task<PagedResultDto<LanguageTextDto>> GetListAsync(GetLanguagesTextsInput input)
    {
        if (!CultureHelper.IsValidCultureCode(input.BaseCultureName) ||
            !CultureHelper.IsValidCultureCode(input.TargetCultureName))
        {
            throw new AbpException("The selected culture is not valid! Make sure you enter a valid culture code.");
        }
        
        var languageTexts = new List<LanguageTextDto>();

        foreach (var resourceName in await GetResourceNamesAsync(input))
        {
            languageTexts.AddRange(await GetLocalizationsAsync(input, resourceName));
        }

        var filteredQuery = languageTexts
            .AsQueryable()
            .WhereIf(
                !input.Filter.IsNullOrWhiteSpace(),
                l =>
                    (l.Name != null && l.Name.IndexOf(input.Filter, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (l.BaseValue != null &&
                     l.BaseValue.IndexOf(input.Filter, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (!input.GetOnlyEmptyValues && l.Value != null &&
                     l.Value.IndexOf(input.Filter, StringComparison.InvariantCultureIgnoreCase) >= 0)
            );

        var languagesTextDtos = filteredQuery
            .OrderBy(input.Sorting.IsNullOrWhiteSpace() ? nameof(LanguageTextDto.Name) : input.Sorting)
            .PageBy(input)
            .ToList();

        return new PagedResultDto<LanguageTextDto>(
            filteredQuery.Count(),
            languagesTextDtos
        );
    }

    public virtual async Task<LanguageTextDto> GetAsync(
        string resourceName,
        string cultureName,
        string name,
        string baseCultureName)
    {
        if (!CultureHelper.IsValidCultureCode(cultureName) ||
            !CultureHelper.IsValidCultureCode(baseCultureName))
        {
            throw new AbpException("The selected culture is not valid! Make sure you enter a valid culture code.");
        }
        
        var localizer = await GetLocalizerAsync(resourceName);
        
        var languageTextDto = new LanguageTextDto
        {
            Name = name,
            ResourceName = resourceName, 
            CultureName = cultureName,
            BaseCultureName = baseCultureName
        };

        using (CultureHelper.Use(CultureInfo.GetCultureInfo(baseCultureName)))
        {
            languageTextDto.BaseValue = baseCultureName != null
                ? (await localizer.GetAllStringsAsync(false)).FirstOrDefault(lt => lt.Name == name)?.Value ?? ""
                : "";
        }

        using (CultureHelper.Use(CultureInfo.GetCultureInfo(cultureName)))
        {
            languageTextDto.Value = (await localizer.GetAllStringsAsync(false)).FirstOrDefault(lt => lt.Name == name)?.Value ?? "";
        }

        return languageTextDto;
    }
    
    [Authorize(LanguageManagementPermissions.LanguageTexts.Edit)]
    public virtual async Task UpdateAsync(string resourceName, string cultureName, string name, string value)
    {
        value ??= "";

        using (CultureHelper.Use(CultureInfo.GetCultureInfo(cultureName)))
        {
            var localizer = await GetLocalizerAsync(resourceName);
            var localizerValue = (await localizer.GetAllStringsAsync(false))
                .FirstOrDefault(lt => lt.Name == name)?.Value ?? "";

            if (localizerValue == value)
            {
                return;
            }
        }

        var text = (await LanguageTextRepository.GetListAsync())
            .FirstOrDefault(l =>
                l.CultureName == cultureName &&
                l.ResourceName == resourceName &&
                l.Name == name
            );

        if (text == null)
        {
            await LanguageTextRepository.InsertAsync(
                new LanguageText(
                    GuidGenerator.Create(),
                    resourceName,
                    cultureName,
                    name,
                    value,
                    CurrentTenant?.Id
                )
            );
        }
        else
        {
            text.Value = value;
            await LanguageTextRepository.UpdateAsync(text);
        }
    }

    public virtual async Task RestoreToDefaultAsync(string resourceName, string cultureName, string name)
    {
        var text = (await LanguageTextRepository.GetListAsync())
            .FirstOrDefault(l =>
                l.CultureName == cultureName &&
                l.ResourceName == resourceName &&
                l.Name == name
            );

        if (text == null)
        {
            return;
        }

        await LanguageTextRepository.DeleteAsync(text);
    }

    protected virtual async Task<List<string>> GetResourceNamesAsync(GetLanguagesTextsInput input)
    {
        var resourceNames = new List<string>();

        if (string.IsNullOrWhiteSpace(input.ResourceName))
        {
            resourceNames.AddRange(
                AbpLocalizationOptions
                    .Resources
                    .Values
                    .Select(l => l.ResourceName)
            );

            var externalResourceNames = await ExternalLocalizationStore.GetResourceNamesAsync();
            resourceNames.AddIfNotContains(externalResourceNames);
        }
        else
        {
            resourceNames.Add(input.ResourceName);
        }

        resourceNames.Sort();

        return resourceNames;
    }

    protected virtual async Task<List<LanguageTextDto>> GetLocalizationsAsync(
        GetLanguagesTextsInput input,
        string resourceName)
    {
        var localizer = await GetLocalizerOrNullAsync(resourceName);
        if (localizer == null)
        {
            return new List<LanguageTextDto>();
        }

        List<LocalizedString> baseLocalizedStrings;
        List<LocalizedString> targetLocalizedStrings;

        using (CultureHelper.Use(CultureInfo.GetCultureInfo(input.BaseCultureName)))
        {
            baseLocalizedStrings = (await localizer
                    .GetAllStringsAsync(
                        includeParentCultures: true,
                        includeBaseLocalizers: false,
                        includeDynamicContributors: true))
                .ToList();
        }

        using (CultureHelper.Use(CultureInfo.GetCultureInfo(input.TargetCultureName)))
        {
            targetLocalizedStrings = (await localizer
                    .GetAllStringsAsync(
                        includeParentCultures: false,
                        includeBaseLocalizers: false,
                        includeDynamicContributors: true))
                .ToList();
        }

        var languageTextDtos = new List<LanguageTextDto>();

        foreach (var baseLocalizedString in baseLocalizedStrings)
        {
            var target = targetLocalizedStrings.FirstOrDefault(l => l.Name == baseLocalizedString.Name);

            if (input.GetOnlyEmptyValues)
            {
                if (!string.IsNullOrEmpty(target?.Value))
                {
                    continue;
                }
            }

            languageTextDtos.Add(
                new LanguageTextDto {
                    BaseCultureName = input.BaseCultureName,
                    CultureName = input.TargetCultureName,
                    Name = baseLocalizedString.Name,
                    BaseValue = baseLocalizedString.Value,
                    ResourceName = resourceName,
                    Value = target?.Value ?? ""
                }
            );
        }

        return languageTextDtos;
    }
    
    protected virtual Task<IStringLocalizer> GetLocalizerAsync(LocalizationResourceBase resource)
    {
        return LocalizerFactory.CreateByResourceNameAsync(resource.ResourceName);
    }

    protected virtual async Task<IStringLocalizer> GetLocalizerAsync(string resourceName)
    {
        return await GetLocalizerAsync(await GetLocalizationResourceAsync(resourceName));
    }

    protected virtual async Task<IStringLocalizer?> GetLocalizerOrNullAsync(string resourceName)
    {
        var resource = await GetLocalizationResourceOrNullAsync(resourceName);
        if (resource == null)
        {
            return null;
        }

        return await GetLocalizerAsync(resource);
    }

    protected virtual async Task<LocalizationResourceBase> GetLocalizationResourceAsync(string resourceName)
    {
        var resource = await GetLocalizationResourceOrNullAsync(resourceName);
        if (resource == null)
        {
            throw new AbpException($"Resource not found: {resourceName}");
        }

        return resource;
    }

    protected virtual async Task<LocalizationResourceBase?> GetLocalizationResourceOrNullAsync(string resourceName)
    {
        return AbpLocalizationOptions.Resources.GetOrDefault(resourceName) ??
               await ExternalLocalizationStore.GetResourceOrNullAsync(resourceName);
    }
}