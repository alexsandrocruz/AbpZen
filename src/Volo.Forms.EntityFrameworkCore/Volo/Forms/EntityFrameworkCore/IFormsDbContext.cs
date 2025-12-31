using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Forms.Answers;
using Volo.Forms.Choices;
using Volo.Forms.Forms;
using Volo.Forms.Questions;
using Volo.Forms.Questions.ChoosableItems;
using Volo.Forms.Responses;

namespace Volo.Forms.EntityFrameworkCore;

[ConnectionStringName(FormsDbProperties.ConnectionStringName)]
public interface IFormsDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
    public DbSet<Form> Forms { get; }
    public DbSet<QuestionBase> Questions { get; }
    public DbSet<ShortText> ShortTexts { get; }
    public DbSet<Choice> Choices { get; }
    public DbSet<Checkbox> Checkboxes { get; }
    public DbSet<ChoiceMultiple> ChoiceMultiples { get; }
    public DbSet<DropdownList> DropdownLists { get; }
    public DbSet<Answer> Answers { get; }
    public DbSet<FormResponse> FormResponses { get; }
}
