using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.Abp.MultiTenancy;
using Volo.Payment.Plans;
using Volo.Payment.Requests;

namespace Volo.Payment.MongoDB;

[IgnoreMultiTenancy]
[ConnectionStringName(PaymentDbProperties.ConnectionStringName)]
public interface IPaymentMongoDbContext : IAbpMongoDbContext
{
    /* Define mongo collections here. Example:
     * IMongoCollection<Question> Questions { get; }
     */

    IMongoCollection<PaymentRequest> PaymentRequests { get; }
    IMongoCollection<Plan> Plans { get; }
}
