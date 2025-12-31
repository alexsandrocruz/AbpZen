using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.Features;
using Volo.Abp.LanguageManagement.Dto;
using Volo.Abp.Localization;
using Volo.Abp.Localization.External;
using Volo.Abp.ObjectExtending;
using Volo.Abp.SettingManagement;

namespace Volo.Abp.LanguageManagement;

[RequiresFeature(LanguageManagementFeatures.Enable)]
[Authorize(LanguageManagementPermissions.Languages.Default)]
public class LanguageAppService : LanguageAppServiceBase, ILanguageAppService
{
    protected ISettingManager SettingManager { get; }

    protected ILanguageRepository LanguageRepository { get; }

    protected LanguageManager LanguageManager { get; }

    protected AbpLocalizationOptions AbpLocalizationOptions { get; }

    protected IExternalLocalizationStore ExternalLocalizationStore { get; }

    public LanguageAppService(
        ISettingManager settingManager,
        ILanguageRepository languageRepository,
        LanguageManager languageManager,
        IOptions<AbpLocalizationOptions> abpLocalizationOptions,
        IExternalLocalizationStore externalLocalizationStore)
    {
        SettingManager = settingManager;
        LanguageRepository = languageRepository;
        LanguageManager = languageManager;
        ExternalLocalizationStore = externalLocalizationStore;
        AbpLocalizationOptions = abpLocalizationOptions.Value;
    }

    public virtual async Task<ListResultDto<LanguageDto>> GetAllListAsync()
    {
        var languages = await LanguageRepository.GetListAsync();
        var defaultLanguage = await FindDefaultLanguage(languages);
        var languageDtos = ObjectMapper.Map<List<Language>, List<LanguageDto>>(languages);

        if (defaultLanguage != null)
        {
            var defaultLanguageDto = languageDtos.Find(
                l => l.CultureName == defaultLanguage.CultureName &&
                     l.UiCultureName == defaultLanguage.UiCultureName
            );

            if (defaultLanguageDto != null)
            {
                defaultLanguageDto.IsDefaultLanguage = true;
            }
        }

        return new ListResultDto<LanguageDto>(languageDtos);
    }

    public virtual async Task<PagedResultDto<LanguageDto>> GetListAsync(GetLanguagesTextsInput input)
    {
        var languages = await LanguageRepository.GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount, input.Filter);
        var totalCount = await LanguageRepository.GetCountAsync(input.Filter);
        var defaultLanguage = await FindDefaultLanguage(languages);

        var languageDtos = ObjectMapper.Map<List<Language>, List<LanguageDto>>(languages);

        if (defaultLanguage != null)
        {
            var defaultLanguageDto = languageDtos.Find(
                l => l.CultureName == defaultLanguage.CultureName &&
                     l.UiCultureName == defaultLanguage.UiCultureName
            );

            if (defaultLanguageDto != null)
            {
                defaultLanguageDto.IsDefaultLanguage = true;
            }
        }

        return new PagedResultDto<LanguageDto>(totalCount, languageDtos);
    }

    [Authorize(LanguageManagementPermissions.Languages.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await LanguageRepository.DeleteAsync(id);
    }

    [Authorize(LanguageManagementPermissions.Languages.ChangeDefault)]
    public virtual async Task SetAsDefaultAsync(Guid id)
    {
        var language = await LanguageRepository.GetAsync(id);
        await SettingManager.SetForCurrentTenantAsync(
            LocalizationSettingNames.DefaultLanguage,
            $"{language.CultureName};{language.UiCultureName}"
        );
    }

    [Authorize(LanguageManagementPermissions.Languages.Create)]
    public virtual async Task<LanguageDto> CreateAsync(CreateLanguageDto input)
    {
        var language = await LanguageManager.CreateAsync(
            input.CultureName,
            input.UiCultureName,
            input.DisplayName,
            input.IsEnabled);

        input.MapExtraPropertiesTo(language);

        language = await LanguageRepository.InsertAsync(language);

        return ObjectMapper.Map<Language, LanguageDto>(language);
    }

    public virtual async Task<LanguageDto> GetAsync(Guid id)
    {
        var language = await LanguageRepository.GetAsync(id);

        var languageDto = ObjectMapper.Map<Language, LanguageDto>(language);

        languageDto.IsDefaultLanguage = await IsDefaultLanguage(language);

        return languageDto;
    }

    [Authorize(LanguageManagementPermissions.Languages.Edit)]
    public virtual async Task<LanguageDto> UpdateAsync(Guid id, UpdateLanguageDto input)
    {
        var language = await LanguageRepository.GetAsync(id);

        language.SetDisplayName(input.DisplayName);
        language.IsEnabled = input.IsEnabled;
        language.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);

        input.MapExtraPropertiesTo(language);

        await LanguageRepository.UpdateAsync(language);

        return ObjectMapper.Map<Language, LanguageDto>(language);
    }

    public virtual async Task<List<LanguageResourceDto>> GetResourcesAsync()
    {
        var list = AbpLocalizationOptions.Resources
            .Values
            .Select(r => new LanguageResourceDto { Name = r.ResourceName })
            .ToList();

        var resourceNames = await ExternalLocalizationStore.GetResourceNamesAsync();
        foreach (var resourceName in resourceNames)
        {
            if (list.All(x => x.Name != resourceName))
            {
                list.Add(new LanguageResourceDto { Name = resourceName });
            }
        }

        return list.OrderBy(x => x.Name).ToList();
    }

    public virtual async Task<List<CultureInfoDto>> GetCulturelistAsync()
    {
        //return CultureInfo.GetCultures(CultureTypes.AllCultures).ToList(); // See https://github.com/dotnet/corefx/issues/38579

        return await Task.FromResult(
            CultureInfo
                .GetCultures(CultureTypes.AllCultures)
                .Where(c => c.DisplayName != CultureInfo.InvariantCulture.DisplayName)
                .Select(c => new CultureInfoDto {
                    DisplayName = c.EnglishName,
                    Name = c.Name
                })
                .OrderBy(x => x.DisplayName)
                .ToList());
    }

    protected virtual async Task<Language?> FindDefaultLanguage(List<Language> languages)
    {
        var settingValue = await SettingManager.GetOrNullForCurrentTenantAsync(LocalizationSettingNames.DefaultLanguage);
        if (settingValue.IsNullOrEmpty())
        {
            return null;
        }

        var (cultureName, uiCultureName) = LocalizationSettingHelper.ParseLanguageSetting(settingValue);
        return languages.FindByCulture(cultureName, uiCultureName);
    }

    protected virtual async Task<bool> IsDefaultLanguage(Language language)
    {
        var settingValue = await SettingManager.GetOrNullForCurrentTenantAsync(LocalizationSettingNames.DefaultLanguage);
        if (settingValue.IsNullOrEmpty())
        {
            return false;
        }

        var (cultureName, uiCultureName) = LocalizationSettingHelper.ParseLanguageSetting(settingValue);

        return language.CultureName == cultureName && language.UiCultureName == uiCultureName;
    }
}
