using System;

namespace Volo.CmsKit.Pro.Admin.Blazor.Pages.CmsKit.Contents;

public class ContentWidgetConfig
{
    public string Name { get; }
    public string EditorComponentName { get; }
    
    public Type Type { get; set; }
    
    public Type ParameterComponentType { get; set; }

    public ContentWidgetConfig(Type type, string widgetName, string editorComponentName, Type parameterComponentType)
    {
        Type = type;
        Name = widgetName;
        EditorComponentName = editorComponentName;
        ParameterComponentType = parameterComponentType;
    }

}