namespace Volo.Saas;

public static class TenantConsts
{
    public static int MaxNameLength { get; set; } = 64;
    public const string TenantIdParameterName = "TenantId";
    public const int MaxPasswordLength = 128;
    public const string Username = "admin";
}
