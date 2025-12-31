using Volo.Abp.Data;

namespace Volo.Payment;

public static class PaymentDbProperties
{
    public static string DbTablePrefix { get; set; } = "Pay";

    public static string DbSchema { get; set; } = AbpCommonDbProperties.DbSchema;

    public const string ConnectionStringName = "Payment";
}
