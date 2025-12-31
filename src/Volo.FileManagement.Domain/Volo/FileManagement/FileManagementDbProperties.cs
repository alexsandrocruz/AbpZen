using Volo.Abp.Data;

namespace Volo.FileManagement;

public static class FileManagementDbProperties
{
    public static string DbTablePrefix { get; set; } = "Fm";

    public static string DbSchema { get; set; } = AbpCommonDbProperties.DbSchema;

    public const string ConnectionStringName = "FileManagement";
}
