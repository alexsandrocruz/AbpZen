using System.Threading.Tasks;

namespace Sapienza.EventoZen.Data
{
    public interface IEventoZenDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}