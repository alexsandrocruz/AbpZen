using Volo.Abp.Gdpr.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Volo.Abp.Gdpr;

[DependsOn(typeof(GdprEntityFrameworkCoreTestModule))]
public class GdprDomainTestModule : AbpModule
{
    
}