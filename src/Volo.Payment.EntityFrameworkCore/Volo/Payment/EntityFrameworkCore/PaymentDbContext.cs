using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;
using Volo.Payment.Plans;
using Volo.Payment.Requests;

namespace Volo.Payment.EntityFrameworkCore;

[IgnoreMultiTenancy]
[ConnectionStringName(PaymentDbProperties.ConnectionStringName)]
public class PaymentDbContext : AbpDbContext<PaymentDbContext>, IPaymentDbContext
{
    public DbSet<PaymentRequest> PaymentRequests { get; set; }

    public DbSet<Plan> Plans { get; set; }

    public DbSet<GatewayPlan> GatewayPlans { get; set; }

    public PaymentDbContext(DbContextOptions<PaymentDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigurePayment();
    }
}
