using System;
using System.Text.Json.Serialization;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.TextTemplateManagement.TextTemplates;

public class TextTemplateDefinitionRecord : BasicAggregateRoot<Guid>, IHasExtraProperties
{
    public string Name { get; set; }

    public string DisplayName { get; set; }

    public bool IsLayout { get; set; }

    public string Layout { get; set; }

    public string LocalizationResourceName  { get; set; }

    public bool IsInlineLocalized { get; set; }

    public string DefaultCultureName { get; set; }

    public string RenderEngine { get; set; }

    public ExtraPropertyDictionary ExtraProperties { get; protected set; }

    public TextTemplateDefinitionRecord()
    {
        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    public TextTemplateDefinitionRecord(
        Guid id,
        string name,
        string displayName,
        bool isLayout,
        string layout,
        string localizationResourceName,
        bool isInlineLocalized,
        string defaultCultureName,
        string renderEngine)
        : base(id)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), TemplateDefinitionRecordConsts.MaxNameLength);
        DisplayName =  Check.NotNullOrWhiteSpace(displayName, nameof(displayName), TemplateDefinitionRecordConsts.MaxDisplayNameLength);
        IsLayout = isLayout;
        Layout = Check.Length(layout, nameof(layout), TemplateDefinitionRecordConsts.MaxLayoutLength);
        LocalizationResourceName = Check.Length(localizationResourceName, nameof(localizationResourceName), TemplateDefinitionRecordConsts.MaxLocalizationResourceNameLength);
        IsInlineLocalized = isInlineLocalized;
        DefaultCultureName = Check.Length(defaultCultureName, nameof(defaultCultureName), TemplateDefinitionRecordConsts.MaxDefaultCultureNameLength);
        RenderEngine = Check.Length(renderEngine, nameof(renderEngine), TemplateDefinitionRecordConsts.MaxRenderEngineLength);

        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    public bool HasSameData(TextTemplateDefinitionRecord otherRecord)
    {
        if (Name != otherRecord.Name)
        {
            return false;
        }

        if (DisplayName != otherRecord.DisplayName)
        {
            return false;
        }

        if (IsLayout != otherRecord.IsLayout)
        {
            return false;
        }

        if (Layout != otherRecord.Layout)
        {
            return false;
        }

        if (LocalizationResourceName != otherRecord.LocalizationResourceName)
        {
            return false;
        }

        if (IsInlineLocalized != otherRecord.IsInlineLocalized)
        {
            return false;
        }

        if (DefaultCultureName != otherRecord.DefaultCultureName)
        {
            return false;
        }

        if (RenderEngine != otherRecord.RenderEngine)
        {
            return false;
        }

        if (!this.HasSameExtraProperties(otherRecord))
        {
            return false;
        }

        return true;
    }

    public void Patch(TextTemplateDefinitionRecord otherRecord)
    {
        if (Name != otherRecord.Name)
        {
            Name = otherRecord.Name;
        }

        if (DisplayName != otherRecord.DisplayName)
        {
            DisplayName = otherRecord.DisplayName;
        }

        if (IsLayout != otherRecord.IsLayout)
        {
            IsLayout = otherRecord.IsLayout;
        }

        if (Layout != otherRecord.Layout)
        {
            Layout = otherRecord.Layout;
        }

        if (LocalizationResourceName != otherRecord.LocalizationResourceName)
        {
            LocalizationResourceName = otherRecord.LocalizationResourceName;
        }

        if (IsInlineLocalized != otherRecord.IsInlineLocalized)
        {
            IsInlineLocalized = otherRecord.IsInlineLocalized;
        }

        if (DefaultCultureName != otherRecord.DefaultCultureName)
        {
            DefaultCultureName = otherRecord.DefaultCultureName;
        }

        if (RenderEngine != otherRecord.RenderEngine)
        {
            RenderEngine = otherRecord.RenderEngine;
        }

        if (!this.HasSameExtraProperties(otherRecord))
        {
            this.ExtraProperties.Clear();

            foreach (var property in otherRecord.ExtraProperties)
            {
                this.ExtraProperties.Add(property.Key, property.Value);
            }
        }
    }
}
