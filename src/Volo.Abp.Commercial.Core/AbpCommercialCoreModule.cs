using Volo.Abp.Modularity;

namespace Volo.Abp.Commercial;

public class AbpCommercialCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // Esta implementação é um stub vazio para fins educacionais.
        // Ela permite que os módulos Pro carreguem sem depender da validação
        // de licença externa contida na DLL comercial original.
    }
}
