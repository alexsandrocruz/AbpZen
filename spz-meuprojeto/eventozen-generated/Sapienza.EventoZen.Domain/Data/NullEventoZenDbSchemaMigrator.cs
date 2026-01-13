using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Sapienza.EventoZen.Data
{
    /* This is used if database provider does't define
     * IEventoZenDbSchemaMigrator implementation.
     */
    public class NullEventoZenDbSchemaMigrator : IEventoZenDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}