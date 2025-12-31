using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Forms.Forms;
using Volo.Forms.Responses;
using Volo.Forms.Questions;
using Volo.Forms.Questions.ChoosableItems;

namespace Volo.Forms;

public class FormsDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IGuidGenerator _guidGenerator;
    private readonly IFormRepository _formRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IResponseRepository _formResponseRepository;
    private readonly TestData _testData;

    public FormsDataSeedContributor(
        IGuidGenerator guidGenerator,
        IQuestionRepository questionRepository,
        IFormRepository formRepository,
        TestData testData,
        IResponseRepository formResponseRepository)
    {
        _guidGenerator = guidGenerator;
        _questionRepository = questionRepository;
        _formRepository = formRepository;
        _testData = testData;
        _formResponseRepository = formResponseRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        /* Instead of returning the Task.CompletedTask, you can insert your test data
         * at this point!
         */
        await SeedFormDataAsync();
        await SeedShortTextDataAsync();
        await SeedCheckboxDataAsync();
        await SeedMultiChoiceDataAsync();
        await SeedDropdownListDataAsync();
        await SeedFormResponsesAsync();
        // await SeedShortTextAnswerAsync();
        // await SeedMultiChoiceAnswerAsync();
    }

    private async Task SeedFormResponsesAsync()
    {
        FormResponse formResponse1 =
            new FormResponse(_testData.FormResponse1, _testData.TestFormId, _testData.TestUser1);
        FormResponse formResponse2 =
            new FormResponse(_testData.FormResponse2, _testData.TestFormId, _testData.TestUser2);
        formResponse2.AddOrUpdateAnswer(_testData.TestMultiChoiceId, _guidGenerator.Create(), _testData.TestChoiceJava, "");
        FormResponse formResponse3 =
            new FormResponse(_testData.FormResponse3, _testData.TestFormId, _testData.TestUser1);
        await _formResponseRepository.InsertAsync(formResponse1);
        await _formResponseRepository.InsertAsync(formResponse2);
        await _formResponseRepository.InsertAsync(formResponse3);
    }

    // private async Task SeedMultiChoiceAnswerAsync()
    // {
    //     Answer choiceMultipleAnswer =
    //         new Answer(_testData.MultiChoiceAnswerId, _testData.TestChoiceCSharp,
    //             choiceId:_testData.TestMultiChoiceId,value: "Instanbul");
    //     await _answerRepository.InsertAsync(choiceMultipleAnswer);
    // }
    //
    // private async Task SeedShortTextAnswerAsync()
    // {
    //     Answer shortTextAnswer =
    //         new Answer(_testData.ShortTextAnswerId, _testData.TestShortTextId, _testData.TestChoiceCSharp,
    //             choiceId: null, value: "John Doe");
    //     await _answerRepository.InsertAsync(shortTextAnswer);
    // }

    private async Task SeedDropdownListDataAsync()
    {
        DropdownList dd = new DropdownList(_guidGenerator.Create());
        dd.SetTitle("Which one do you love the most?")
            .SetDescription("Select all applies")
            .SetIndex(4)
            .SetFormId(_testData.TestFormId)
            .As<DropdownList>();
        dd.AddChoice(_guidGenerator.Create(), "Father", false);
        dd.AddChoice(_guidGenerator.Create(), "Mother", false);
        dd.AddChoice(_guidGenerator.Create(), "Brother", false);
        dd.AddChoice(_guidGenerator.Create(), "Sister", false);

        var item = await _questionRepository.InsertAsync(dd);
    }

    private async Task SeedFormDataAsync()
    {
        Form form = new Form(_testData.TestFormId, "Test Form", "Test Description");

        await _formRepository.InsertAsync(form);
    }

    private async Task SeedShortTextDataAsync()
    {
        ShortText st = new ShortText(_testData.TestShortTextId);
        st.SetTitle("What is your name?")
            .SetDescription("Please specify your full name")
            .SetIndex(1)
            .SetFormId(_testData.TestFormId);

        await _questionRepository.InsertAsync(st);
    }

    private async Task SeedCheckboxDataAsync()
    {
        List<(Guid id, string value, bool isCorrect)> choiceList =
            new List<(Guid id, string value, bool isCorrect)>()
            {
                    (_testData.TestChoiceCSharp, "C#", false),
                    (_testData.TestChoiceJava, "Java", false),
                    (_testData.TestChoiceJavascript, "Javascript", false),
                    (_guidGenerator.Create(), "Python", false),
                    (_guidGenerator.Create(), "Go", false),
            };

        Checkbox cb = new Checkbox(_testData.TestCheckboxId);
        cb.SetTitle("Which technologies are you interested?")
            .SetDescription("Please select all options apply")
            .SetIndex(2)
            .SetFormId(_testData.TestFormId)
            .As<Checkbox>()
            .AddChoices(choiceList);

        await _questionRepository.InsertAsync(cb);
    }

    private async Task SeedMultiChoiceDataAsync()
    {
        List<(Guid id, string value, bool isCorrect)> choiceList =
            new List<(Guid id, string value, bool isCorrect)>()
            {
                    (_guidGenerator.Create(), "London", false),
                    (_guidGenerator.Create(), "New York", false),
                    (_guidGenerator.Create(), "Instanbul", false),
                    (_guidGenerator.Create(), "Paris", false),
            };

        ChoiceMultiple cm = new ChoiceMultiple(_testData.TestMultiChoiceId);
        cm.SetTitle("Where are you located?")
            .SetDescription("Please specify your location")
            .SetIndex(3)
            .SetFormId(_testData.TestFormId)
            .As<ChoiceMultiple>()
            .AddChoices(choiceList);

        await _questionRepository.InsertAsync(cm);
    }
}
