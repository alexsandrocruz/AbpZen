namespace Volo.Abp.LanguageManagement;

public static class LocalizationResourceRecordConsts
{
    /// <summary>
    /// Default value: 128
    /// </summary>
    public static int MaxNameLength { get; set; } = 128;
    
    /// <summary>
    /// Default value: 1280 (10 x <see cref="MaxNameLength"/>)
    /// </summary>
    public static int MaxBaseResourcesLength { get; set; } = MaxNameLength * 10;
    
    /// <summary>
    /// Default value: 10 (<see cref="LanguageConsts.MaxCultureNameLength"/>)
    /// </summary>
    public static int MaxDefaultCultureLength { get; set; } = LanguageConsts.MaxCultureNameLength;

    public static int MaxSupportedCulturesLength { get; set; } = LanguageConsts.MaxCultureNameLength * 64;
}