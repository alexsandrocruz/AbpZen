using System;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Localization;

namespace Volo.Abp.LanguageManagement;

public class Language : FullAuditedAggregateRoot<Guid>, ILanguageInfo
{
    [NotNull]
    public virtual string CultureName { get; protected set; }

    [NotNull]
    public virtual string UiCultureName { get; protected set; }

    [NotNull]
    public virtual string DisplayName { get; protected set; }

    public virtual bool IsEnabled { get; set; }

    protected Language()
    {

    }

    internal Language(
        Guid id,
        string cultureName,
        string uiCultureName = null,
        string displayName = null,
        bool isEnabled = true
    ) : base(id: id)
    {
        ChangeCultureInternal(cultureName, uiCultureName, displayName);
        IsEnabled = isEnabled;
    }

    public virtual void ChangeCulture(string cultureName, string uiCultureName = null, string displayName = null)
    {
        ChangeCultureInternal(cultureName, uiCultureName, displayName);
    }

    protected virtual void ChangeCultureInternal(string cultureName, string uiCultureName, string displayName)
    {
        CultureName = Check.NotNullOrWhiteSpace(cultureName, nameof(cultureName));

        UiCultureName = !uiCultureName.IsNullOrWhiteSpace()
            ? uiCultureName
            : cultureName;

        DisplayName = !displayName.IsNullOrWhiteSpace()
            ? displayName
            : cultureName;
    }

    public virtual void SetDisplayName(string displayName)
    {
        DisplayName = !displayName.IsNullOrWhiteSpace()
            ? displayName
            : CultureName;
    }
}
