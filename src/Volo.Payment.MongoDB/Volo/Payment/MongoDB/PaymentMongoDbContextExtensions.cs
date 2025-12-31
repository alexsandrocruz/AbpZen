using Volo.Abp;
using Volo.Abp.MongoDB;
using Volo.Payment.Requests;

namespace Volo.Payment.MongoDB;

public static class PaymentMongoDbContextExtensions
{
    public static void ConfigurePayment(
        this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<PaymentRequest>(b =>
        {
            b.CollectionName = PaymentDbProperties.DbTablePrefix + "PaymentRequests";
        });
    }
}
