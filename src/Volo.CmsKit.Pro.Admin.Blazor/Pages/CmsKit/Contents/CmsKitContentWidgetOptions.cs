using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Volo.CmsKit.Pro.Admin.Blazor.Pages.CmsKit.Contents;

public class CmsKitContentWidgetOptions
{
    public Dictionary<string, ContentWidgetConfig> WidgetConfigs { get; }

    public CmsKitContentWidgetOptions()
    {
        WidgetConfigs = new();
    }

    public void AddWidget<T>(string widgetType, string widgetName) where T : ComponentBase
    {
        AddWidget(typeof(T), widgetType, widgetName);
    }
    
    public void AddWidget<T, TParameterWidgetType>(string widgetType, string widgetName, string parameterWidgetName = null) where T : ComponentBase
    {
        AddWidget(typeof(T), widgetType, widgetName, parameterWidgetName, typeof(TParameterWidgetType));
    }

    public void AddWidget(Type type, string widgetType, string widgetName,  string parameterWidgetName = null, Type parameterWidgetType = null)
    {
        var config = new ContentWidgetConfig(type, widgetName, parameterWidgetName, parameterWidgetType);
        WidgetConfigs.Add(widgetType, config);
    }
    
    public Type FindComponentType(string widgetType)
    {
        return WidgetConfigs[widgetType].Type;
    }
    
    public Type FindParameterComponentType(string editorComponentName)
    {
        foreach (var config in WidgetConfigs.Values)
        {
            if (config.EditorComponentName == editorComponentName)
            {
                return config.ParameterComponentType;
            }
        }

        return null;
    }
}