using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Sapienza.Sapienza.Dominus.Data
{
    /* This is used if database provider does't define
     * ISapienza.DominusDbSchemaMigrator implementation.
     */
    public class NullSapienza.DominusDbSchemaMigrator : ISapienza.DominusDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}