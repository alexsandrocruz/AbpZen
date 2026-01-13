using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Sapienza.FabioRibeiro.Data
{
    /* This is used if database provider does't define
     * IFabioRibeiroDbSchemaMigrator implementation.
     */
    public class NullFabioRibeiroDbSchemaMigrator : IFabioRibeiroDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}