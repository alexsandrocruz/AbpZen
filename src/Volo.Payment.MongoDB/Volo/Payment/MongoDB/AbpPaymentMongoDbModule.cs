using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;
using Volo.Payment.Requests;
using Volo.Payment.Plans;
using Volo.Payment.MongoDB.Plans;

namespace Volo.Payment.MongoDB;

[DependsOn(
    typeof(AbpPaymentDomainModule),
    typeof(AbpMongoDbModule)
    )]
public class AbpPaymentMongoDbModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMongoDbContext<PaymentMongoDbContext>(options =>
        {
            options.AddDefaultRepositories<IPaymentMongoDbContext>();

            options.AddRepository<PaymentRequest, MongoPaymentRequestRepository>();
            options.AddRepository<Plan, MongoPlanRepository>();
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }
}
