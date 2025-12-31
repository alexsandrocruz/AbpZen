using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Guids;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;
using Volo.Forms.Questions;
using Volo.Forms.Questions.ChoosableItems;
using Xunit;

namespace Volo.Forms.Forms;

public abstract class ItemRepository_Tests<TStartupModule> : FormsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IGuidGenerator _guidGenerator;
    private readonly QuestionManager _questionManager;
    private readonly TestData _testData;
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public ItemRepository_Tests()
    {
        _unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
        _questionRepository = GetRequiredService<IQuestionRepository>();
        _questionManager = GetRequiredService<QuestionManager>();
        _guidGenerator = GetRequiredService<IGuidGenerator>();
        _testData = GetRequiredService<TestData>();
    }

    [Fact]
    public async Task Should_Get_Test_Items()
    {
        var items = await _questionRepository.GetListAsync();
        items.Count.ShouldBe(4);
    }

    [Fact]
    public async Task Should_Create_Multichoice_And_Get_Type()
    {
        List<(Guid id, string value, bool isCorrect)> choiceList =
            new List<(Guid id, string value, bool isCorrect)>()
            {
                    (_guidGenerator.Create(), "Under 18", false),
                    (_guidGenerator.Create(), "18-24", false),
                    (_guidGenerator.Create(), "24-30", false),
                    (_guidGenerator.Create(), "30-40", false),
                    (_guidGenerator.Create(), "Over 40", false)
            };

        var cmId = _guidGenerator.Create();
        ChoiceMultiple cm = new ChoiceMultiple(cmId);
        cm.SetTitle("What is your age?")
            .SetDescription("Please specify which option your age fits")
            .SetIndex(1)
            .SetFormId(_testData.TestFormId)
            .As<ChoiceMultiple>()
            .AddChoices(choiceList);
        var insertedItem = await _questionRepository.InsertAsync(cm);
        insertedItem.ShouldNotBeNull();
        insertedItem.GetQuestionType().ShouldBe(QuestionTypes.ChoiceMultiple);
    }

    [Fact]
    public async Task Should_Create_ShortText_And_Get_Type()
    {
        ShortText st = new ShortText(_guidGenerator.Create());
        st.SetTitle("Your Name?")
            .SetDescription("Please specify your full name")
            .SetIndex(1)
            .SetFormId(_testData.TestFormId);
        var insertedItem = await _questionRepository.InsertAsync(st);
        insertedItem.ShouldNotBeNull();
        insertedItem.GetQuestionType().ShouldBe(QuestionTypes.ShortText);
    }

    [Fact]
    public async Task Should_Create_Checkbox_And_Get_Type()
    {
        List<(Guid id, string value, bool isCorrect)> choiceList =
            new List<(Guid id, string value, bool isCorrect)>()
            {
                    (_guidGenerator.Create(), "08:00-12:00", false),
                    (_guidGenerator.Create(), "12:00-18:00", false),
                    (_guidGenerator.Create(), "18:00-00:00", false),
                    (_guidGenerator.Create(), "10:00-08:00", false)
            };

        var cbId = _guidGenerator.Create();
        Checkbox cb = new Checkbox(cbId);
        cb.SetTitle("Which hours are you available to work?")
            .SetDescription("Please select all options apply")
            .SetIndex(1)
            .SetFormId(_testData.TestFormId)
            .As<Checkbox>()
            .AddChoices(choiceList);
        var insertedItem = await _questionRepository.InsertAsync(cb);
        insertedItem.ShouldNotBeNull();
        insertedItem.GetQuestionType().ShouldBe(QuestionTypes.Checkbox);
    }

    [Fact]
    public async Task Should_Get_Items_By_FormId()
    {
        var itemList = await _questionRepository.GetListByFormIdAsync(_testData.TestFormId);
        itemList.Count.ShouldBe(4);

        var st = itemList.First(q => q.GetQuestionType() == QuestionTypes.ShortText);
        st.ShouldNotBeNull();

        var cm = itemList.First(q => q.GetQuestionType() == QuestionTypes.ChoiceMultiple);
        cm.ShouldNotBeNull();
        var cmChoices = cm.As<ChoiceMultiple>()
            .GetChoices();
        cmChoices.ShouldNotBeNull();
        cmChoices.First().ShouldNotBeNull();

        var cb = itemList.First(q => q.GetQuestionType() == QuestionTypes.Checkbox);
        cb.ShouldNotBeNull();
        var cbChoices = cb.As<Checkbox>()
            .GetChoices();
        cbChoices.ShouldNotBeNull();
        cbChoices.First().ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Delete_Multiple_Choices()
    {
        var item = await _questionRepository.GetWithChoices(_testData.TestCheckboxId);
        item.Choices.Count.ShouldBe(5);
        using (var uow = _unitOfWorkManager.Begin())
        {
            await _questionRepository.ClearQuestionChoicesAsync(item.Question.Id);
            await uow.CompleteAsync();
        }

        var itemUpdated = await _questionRepository.GetWithChoices(_testData.TestCheckboxId);
        itemUpdated.Choices.Count.ShouldBe(0);
    }

    [Fact]
    public async Task Should_Update_ChoiceMultiple_With_Required_And_HasOtherOption()
    {
        var item = await _questionRepository.GetAsync(_testData.TestMultiChoiceId);
        item.GetQuestionType().ShouldBe(QuestionTypes.ChoiceMultiple);
        item.As<ChoiceMultiple>().IsRequired.ShouldBe(false);
        item.As<ChoiceMultiple>().HasOtherOption.ShouldBe(false);
        var choices = item.As<ChoiceMultiple>().GetChoices();
        List<(Guid Id, string value, bool isCorrect)> choiceList =
            new List<(Guid Id, string value, bool isCorrect)>();
        var updatedItem = await _questionManager.UpdateAsync(item.Id, item.Title, item.Index, isRequired: true,
            item.Description,
            item.GetQuestionType(), hasOtherOption: true, choiceList);

        updatedItem.As<ChoiceMultiple>().IsRequired.ShouldBe(true);
        updatedItem.As<ChoiceMultiple>().HasOtherOption.ShouldBe(true);
    }

    [Fact]
    public async Task Should_Get_Test_Form_Responses()
    {
        var result = await _questionRepository.GetListWithAnswersByFormId(_testData.TestFormId);
        result.Count().ShouldNotBe(0);
        result[2].Answers.Count.ShouldNotBe(0);
    }
}
