using System;
using System.Collections.Generic;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Localization;

namespace Volo.Abp.LanguageManagement.External;

public class LocalizationResourceRecord : BasicAggregateRoot<Guid>, IHasCreationTime, IHasModificationTime
{
    public virtual string Name { get; private set; }

    public virtual string? DefaultCulture { get; set; }

    public virtual string? BaseResources { get; private set; }
    
    public virtual string? SupportedCultures { get; private set; }

    public virtual DateTime CreationTime { get; protected set; }
    
    public DateTime? LastModificationTime { get; set; }

    protected LocalizationResourceRecord()
    {
    }

    public LocalizationResourceRecord(
        LocalizationResourceBase resource,
        IEnumerable<string> supportedCultures)
    {
        Name = Check.NotNullOrWhiteSpace(resource.ResourceName, nameof(resource.ResourceName));
        DefaultCulture = resource.DefaultCultureName;
        SetBaseResources(resource.BaseResourceNames);
        SetSupportedCultures(supportedCultures);
    }

    public string[] GetBaseResources()
    {
        return GetArrayFromString(BaseResources);
    }

    private LocalizationResourceRecord SetBaseResources(IEnumerable<string> baseResources)
    {
        BaseResources = JoinAsString(baseResources);
        return this;
    }
    
    public string[] GetSupportedCultures()
    {
        return GetArrayFromString(SupportedCultures);
    }

    private LocalizationResourceRecord SetSupportedCultures(IEnumerable<string> supportedCultures)
    {
        SupportedCultures = JoinAsString(supportedCultures);
        return this;
    }

    public bool TryUpdate(
        LocalizationResourceBase resource,
        IEnumerable<string> supportedCultures)
    {
        var updatedDefaultCultureName = TryUpdateDefaultCultureName(resource);
        var updatedBaseResources = TryUpdateBaseResources(resource);
        var updatedSupportedCultures = TryUpdateSupportedCultures(supportedCultures);

        return updatedDefaultCultureName ||
               updatedBaseResources ||
               updatedSupportedCultures;
    }

    private bool TryUpdateDefaultCultureName(LocalizationResourceBase resource)
    {
        if (DefaultCulture == resource.DefaultCultureName)
        {
            return false;
        }

        DefaultCulture = resource.DefaultCultureName;
        return true;
    }

    private bool TryUpdateBaseResources(LocalizationResourceBase resource)
    {
        var serializedBaseResources = JoinAsString(resource.BaseResourceNames);
        if (BaseResources == serializedBaseResources)
        {
            return false;
        }

        BaseResources = serializedBaseResources;
        return true;
    }
    
    private bool TryUpdateSupportedCultures(IEnumerable<string> supportedCultures)
    {
        var serializedSupportedCultures = JoinAsString(supportedCultures);
        if (SupportedCultures == serializedSupportedCultures)
        {
            return false;
        }

        SupportedCultures = serializedSupportedCultures;
        return true;
    }

    private static string? JoinAsString(IEnumerable<string> list)
    {
        var serialized = string.Join(",", list);
        if (serialized.IsNullOrWhiteSpace())
        {
            return null;
        }

        return serialized;
    }
    
    private string[] GetArrayFromString(string? str)
    {
        return str?.Split(",", StringSplitOptions.RemoveEmptyEntries) ??
               Array.Empty<string>();
    }
}