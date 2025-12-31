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
public interface IFormsMongoDbContext : IAbpMongoDbContext
{
    /* Define mongo collections here. Example:
     * IMongoCollection<Question> Questions { get; }
     */
    IMongoCollection<Form> Forms { get; }
    IMongoCollection<QuestionBase> Questions { get; }
    IMongoCollection<Choice> Choices { get; }
    IMongoCollection<Checkbox> Checkboxes { get; }
    IMongoCollection<ChoiceMultiple> ChoiceMultiples { get; }
    IMongoCollection<DropdownList> DropdownLists { get; }
    IMongoCollection<ShortText> ShortTexts { get; }
    IMongoCollection<FormResponse> FormResponses { get; }
}
