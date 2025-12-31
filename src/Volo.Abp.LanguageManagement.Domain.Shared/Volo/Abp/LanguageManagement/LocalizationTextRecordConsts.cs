namespace Volo.Abp.LanguageManagement;

public static class LocalizationTextRecordConsts
{
    /// <summary>
    /// Default value: 128
    /// </summary>
    public static int MaxResourceNameLength { get; set; } = 128;

    /// <summary>
    /// Default value: 1024 * 1024
    /// </summary>
    public static int MaxValueLength { get; set; } = 1024 * 1024;

    /// <summary>
    /// Default value: 10
    /// </summary>
    public static int MaxCultureNameLength = LanguageConsts.MaxCultureNameLength;
}