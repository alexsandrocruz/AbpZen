using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.Abp.MultiTenancy;
using Volo.Payment.Plans;
using Volo.Payment.Requests;

namespace Volo.Payment.MongoDB;

[IgnoreMultiTenancy]
[ConnectionStringName(PaymentDbProperties.ConnectionStringName)]
public class PaymentMongoDbContext : AbpMongoDbContext, IPaymentMongoDbContext
{
    public IMongoCollection<PaymentRequest> PaymentRequests => Collection<PaymentRequest>();

    public IMongoCollection<Plan> Plans => Collection<Plan>();

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        modelBuilder.ConfigurePayment();
    }
}
