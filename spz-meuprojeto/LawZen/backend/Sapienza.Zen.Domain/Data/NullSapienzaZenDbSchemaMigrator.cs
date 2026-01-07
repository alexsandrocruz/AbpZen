using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Sapienza.Zen.Data
{
    /* This is used if database provider does't define
     * ISapienzaZenDbSchemaMigrator implementation.
     */
    public class NullSapienzaZenDbSchemaMigrator : ISapienzaZenDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}