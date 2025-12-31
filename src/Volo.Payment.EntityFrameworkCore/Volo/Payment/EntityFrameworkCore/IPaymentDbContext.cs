using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;
using Volo.Payment.Plans;
using Volo.Payment.Requests;

namespace Volo.Payment.EntityFrameworkCore;

[IgnoreMultiTenancy]
[ConnectionStringName(PaymentDbProperties.ConnectionStringName)]
public interface IPaymentDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */

    DbSet<PaymentRequest> PaymentRequests { get; }

    DbSet<Plan> Plans { get; }

    DbSet<GatewayPlan> GatewayPlans { get; }
}
