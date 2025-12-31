using Volo.Abp.Data;

namespace Volo.Forms;

public static class FormsDbProperties
{
    public static string DbTablePrefix { get; set; } = "Frm";

    public static string DbSchema { get; set; } = AbpCommonDbProperties.DbSchema;

    public const string ConnectionStringName = "Forms";
}
