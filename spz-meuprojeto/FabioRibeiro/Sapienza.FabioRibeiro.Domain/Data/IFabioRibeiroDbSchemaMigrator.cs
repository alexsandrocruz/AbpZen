using System.Threading.Tasks;

namespace Sapienza.FabioRibeiro.Data
{
    public interface IFabioRibeiroDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}