using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Forms.Forms;
using Volo.Forms.Questions;

namespace Volo.Forms.EntityFrameworkCore;

[DependsOn(
    typeof(FormsDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class FormsEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<FormsDbContext>(options =>
        {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
            options.AddDefaultRepositories();
            options.AddRepository<Form, EfCoreFormRepository>();
            options.AddRepository<QuestionBase, EfCoreQuestionRepository>();
        });
    }
}
