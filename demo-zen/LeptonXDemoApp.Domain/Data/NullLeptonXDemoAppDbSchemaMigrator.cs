using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LeptonXDemoApp.Data
{
    /* This is used if database provider does't define
     * ILeptonXDemoAppDbSchemaMigrator implementation.
     */
    public class NullLeptonXDemoAppDbSchemaMigrator : ILeptonXDemoAppDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}