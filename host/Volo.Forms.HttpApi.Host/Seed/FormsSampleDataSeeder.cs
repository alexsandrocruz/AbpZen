using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Forms.Forms;
using Volo.Forms.Questions;
using Volo.Forms.Questions.ChoosableItems;

namespace Volo.Forms.Seed;

/* You can use this file to seed some sample data
 * to test your module easier.
 *
 * This class is shared among these projects:
 * - Volo.Forms.IdentityServer
 * - Volo.Forms.Common.Web.Unified (used as linked file)
 */
public class FormsSampleDataSeeder : ITransientDependency
{
    private readonly IGuidGenerator _guidGenerator;
    private readonly IFormRepository _formRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly ICurrentTenant _currentTenant;

    public FormsSampleDataSeeder(IGuidGenerator guidGenerator, IFormRepository formRepository,
        IQuestionRepository questionRepository, ICurrentTenant currentTenant)
    {
        _guidGenerator = guidGenerator;
        _formRepository = formRepository;
        _questionRepository = questionRepository;
        _currentTenant = currentTenant;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        using (_currentTenant.Change(null))
        {
            var count = await _formRepository.GetCountAsync(); //Seed Sample Data
            if (count == 0)
            {
                var testFormId = Guid.Parse("36A67366-DFCF-CE94-C812-39F6F4A67110");
                await CreateTestFormAsync(testFormId, context);
                await CreateTestQuestionAsync(testFormId, context);
                await CreateVacationForm(_guidGenerator.Create(), context);
            }
        }
    }

    private async Task CreateVacationForm(Guid id, DataSeedContext context)
    {
        var form = new Form(id, "Vacation Form", "In Company Vacation Form", tenantId: context.TenantId);
        form.SetSettings(false, false, false, true, false, true);

        await _formRepository.InsertAsync(form);

        var st = new ShortText(_guidGenerator.Create());
        st.SetTitle("What is your excuse?")
            .SetDescription("Please specify your full excuse")
            .SetIndex(1)
            .SetFormId(id);
        st.SetRequired(true);
        await _questionRepository.InsertAsync(st);

        var choiceList =
            new List<(Guid id, string value, bool isCorrect)>()
            {
                    (_guidGenerator.Create(), "Annual", false),
                    (_guidGenerator.Create(), "Yearly paid vacation", false),
                    (_guidGenerator.Create(), "Yearly unpaid vacation", false),
                    (_guidGenerator.Create(), "Excuse vacation", false),
            };

        var cm = new ChoiceMultiple(_guidGenerator.Create());
        cm.SetTitle("What kind of vacation?")
            .SetDescription("Please select the one below which applies to you")
            .SetIndex(2)
            .SetFormId(id);

        cm.AddChoices(choiceList);

        cm.AddChoiceOther(_guidGenerator.Create(), choiceList.Count + 1);
        cm.SetRequired(true);
        await _questionRepository.InsertAsync(cm);
    }

    private async Task CreateTestFormAsync(Guid testFormId, DataSeedContext context)
    {
        var form = new Form(testFormId, "Test Form", "Test Description", tenantId: context.TenantId);

        await _formRepository.InsertAsync(form);
    }


    private async Task CreateTestQuestionAsync(Guid testFormId, DataSeedContext context)
    {
        await SeedShortTextDataAsync(testFormId);
        await SeedCheckboxDataAsync(testFormId);
        await SeedMultiChoiceDataAsync(testFormId);
        await SeedDropdownDataAsync(testFormId);
    }
    private async Task SeedShortTextDataAsync(Guid testFormId)
    {
        ShortText st = new ShortText(_guidGenerator.Create());
        st.SetTitle("What is your name?")
            .SetDescription("Please specify your full name")
            .SetIndex(1)
            .SetFormId(testFormId);
        await _questionRepository.InsertAsync(st);
    }

    private async Task SeedCheckboxDataAsync(Guid testFormId)
    {
        List<(Guid id, string value, bool isCorrect)> choiceList =
            new List<(Guid id, string value, bool isCorrect)>()
            {
                    (_guidGenerator.Create(), "C#", false),
                    (_guidGenerator.Create(), "Java", false),
                    (_guidGenerator.Create(), "Javascript", false),
                    (_guidGenerator.Create(), "Python", false),
                    (_guidGenerator.Create(), "Go", false),
            };
        Checkbox cb = new Checkbox(_guidGenerator.Create());
        cb.SetTitle("Which technologies are you interested?")
            .SetDescription("Please select all options apply")
            .SetIndex(2)
            .SetFormId(testFormId);

        cb.AddChoices(choiceList);
        await _questionRepository.InsertAsync(cb);
    }

    private async Task SeedMultiChoiceDataAsync(Guid testFormId)
    {
        List<(Guid id, string value, bool isCorrect)> choiceList =
            new List<(Guid id, string value, bool isCorrect)>()
            {
                    (_guidGenerator.Create(), "London", false),
                    (_guidGenerator.Create(), "New York", false),
                    (_guidGenerator.Create(), "Instanbul", false),
                    (_guidGenerator.Create(), "Paris", false),
            };
        ChoiceMultiple cm = new ChoiceMultiple(_guidGenerator.Create());
        cm.SetTitle("Where are you located?")
            .SetDescription("Please specify your location")
            .SetIndex(3)
            .SetFormId(testFormId);

        cm.AddChoices(choiceList);
        await _questionRepository.InsertAsync(cm);
    }
    private async Task SeedDropdownDataAsync(Guid testFormId)
    {
        List<(Guid id, string value, bool isCorrect)> choiceList =
            new List<(Guid id, string value, bool isCorrect)>()
            {
                    (_guidGenerator.Create(), "Angular", false),
                    (_guidGenerator.Create(), "React", false),
                    (_guidGenerator.Create(), "Vue", false),
                    (_guidGenerator.Create(), "Jquery", false),
                    (_guidGenerator.Create(), "Javascript (Pure)", false),
            };
        DropdownList dropdown = new DropdownList(_guidGenerator.Create());
        dropdown.SetTitle("Which one is your front-end tech?")
            .SetDescription("Please select your favorite framework or library for front-end development")
            .SetIndex(4)
            .SetFormId(testFormId);

        dropdown.AddChoices(choiceList);
        await _questionRepository.InsertAsync(dropdown);
    }
}
