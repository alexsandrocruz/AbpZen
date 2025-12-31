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
public class FormsDbContext : AbpDbContext<FormsDbContext>, IFormsDbContext
{
    public virtual DbSet<Form> Forms { get; set; }
    public virtual DbSet<QuestionBase> Questions { get; set; }
    public virtual DbSet<ShortText> ShortTexts { get; set; }
    public virtual DbSet<Choice> Choices { get; set; }
    public virtual DbSet<Checkbox> Checkboxes { get; set; }
    public virtual DbSet<ChoiceMultiple> ChoiceMultiples { get; set; }
    public virtual DbSet<DropdownList> DropdownLists { get; set; }
    public virtual DbSet<Answer> Answers { get; set; }
    public virtual DbSet<FormResponse> FormResponses { get; set; }

    public FormsDbContext(DbContextOptions<FormsDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureForms();
    }
}
