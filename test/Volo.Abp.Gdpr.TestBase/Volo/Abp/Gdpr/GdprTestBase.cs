using Volo.Abp.Modularity;
using Volo.Abp.Testing;

namespace Volo.Abp.Gdpr;

public abstract class GdprTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}