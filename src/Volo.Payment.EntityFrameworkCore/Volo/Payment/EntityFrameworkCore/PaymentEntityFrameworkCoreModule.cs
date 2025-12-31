using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Payment.Requests;
using Volo.Payment.Plans;

namespace Volo.Payment.EntityFrameworkCore;

[DependsOn(
    typeof(AbpPaymentDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class AbpPaymentEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<PaymentDbContext>(options =>
        {
            options.AddRepository<PaymentRequest, EfCorePaymentRequestRepository>();
            options.AddRepository<Plan, EfCorePlanRepository>();
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }
}
