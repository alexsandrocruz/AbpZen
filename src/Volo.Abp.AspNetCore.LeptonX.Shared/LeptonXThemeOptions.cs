using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.LeptonX.Shared;

public class LeptonXThemeOptions
{
    public Dictionary<string, LeptonXThemeStyle> Styles { get; } = new();

    /// <summary>
    /// Defines the default fallback theme. Default value is <see cref="LeptonXStyleNames.System"/>
    /// </summary>
    public string DefaultStyle { get; set; } = LeptonXStyleNames.System;

    public LeptonXThemeStyle GetDefaultStyle()
    {
        if (string.IsNullOrEmpty(DefaultStyle) || !Styles.ContainsKey(DefaultStyle))
        {
            return Styles.FirstOrDefault().Value;
        }

        return Styles[DefaultStyle];
    }
}
