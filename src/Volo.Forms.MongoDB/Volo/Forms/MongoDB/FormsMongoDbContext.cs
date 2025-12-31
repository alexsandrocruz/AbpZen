using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.Forms.Choices;
using Volo.Forms.Forms;
using Volo.Forms.Questions;
using Volo.Forms.Questions.ChoosableItems;
using Volo.Forms.Responses;

namespace Volo.Forms.MongoDB;

[ConnectionStringName(FormsDbProperties.ConnectionStringName)]
public class FormsMongoDbContext : AbpMongoDbContext, IFormsMongoDbContext
{
    /* Add mongo collections here. Example:
     * public IMongoCollection<Question> Questions => Collection<Question>();
     */
    public IMongoCollection<Form> Forms => Collection<Form>();
    public IMongoCollection<QuestionBase> Questions => Collection<QuestionBase>();
    public IMongoCollection<Choice> Choices => Collection<Choice>();
    public IMongoCollection<Checkbox> Checkboxes => Collection<Checkbox>();
    public IMongoCollection<ChoiceMultiple> ChoiceMultiples => Collection<ChoiceMultiple>();
    public IMongoCollection<DropdownList> DropdownLists => Collection<DropdownList>();
    public IMongoCollection<ShortText> ShortTexts => Collection<ShortText>();
    public IMongoCollection<FormResponse> FormResponses => Collection<FormResponse>();

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        modelBuilder.ConfigureForms();
    }
}
