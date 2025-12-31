using Volo.Abp.Data;

namespace Volo.Abp.Gdpr;

public static class GdprDbProperties
{
    public static string DbTablePrefix { get; set; } = "Gdpr";

    public static string DbSchema { get; set; } = AbpCommonDbProperties.DbSchema;

    public const string ConnectionStringName = "AbpGdpr";
}