using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;
using Volo.Forms.Choices;
using Volo.Forms.Forms;
using Volo.Forms.Questions;
using Volo.Forms.Responses;

namespace Volo.Forms.MongoDB;

[DependsOn(
    typeof(FormsDomainModule),
    typeof(AbpMongoDbModule)
    )]
public class FormsMongoDbModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMongoDbContext<FormsMongoDbContext>(options =>
        {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, MongoQuestionRepository>();
                 */
            options.AddDefaultRepositories();
            options.AddRepository<Form, MongoFormRepository>();
            options.AddRepository<QuestionBase, MongoQuestionRepository>();
            options.AddRepository<Choice, MongoChoiceRepository>();
            options.AddRepository<FormResponse, MongoFormResponseRepository>();
        });
    }
}
