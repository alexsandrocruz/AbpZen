using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace Volo.Forms;

[DependsOn(
    typeof(FormsHttpApiClientModule),
    typeof(AbpHttpClientIdentityModelModule)
    )]
public class FormsConsoleApiClientModule : AbpModule
{

}
