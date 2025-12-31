using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Emailing;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Forms;

[DependsOn(
    typeof(FormsDomainModule),
    typeof(FormsApplicationContractsModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpEmailingModule)
    )]
public class FormsApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<FormsApplicationModule>();
        });

        context.Services.AddAutoMapperObjectMapper<FormsApplicationModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<FormsApplicationModule>(validate: true);
        });
    }
}
