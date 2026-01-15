using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Sapienza.Dominus.Data
{
    /* This is used if database provider does't define
     * IDominusDbSchemaMigrator implementation.
     */
    public class NullDominusDbSchemaMigrator : IDominusDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}