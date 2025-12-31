using System;
using System.Collections.Generic;
using Volo.Abp.Caching;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.LanguageManagement;

[Serializable]
[IgnoreMultiTenancy]
[CacheName("Volo.Abp.LanguageList")]
public class LanguageListCacheItem
{
    public List<LanguageInfo> Languages { get; set; }

    public LanguageListCacheItem()
    {

    }

    public LanguageListCacheItem(List<LanguageInfo> languages)
    {
        Languages = languages;
    }
}
