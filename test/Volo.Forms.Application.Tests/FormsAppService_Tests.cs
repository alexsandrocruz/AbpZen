using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Forms.Choices;
using Volo.Forms.Forms;
using Volo.Forms.Questions;
using Xunit;

namespace Volo.Forms;

public class FormsAppService_Tests : FormsApplicationTestBase
{
    private readonly IFormAppService _formAppService;
    private readonly IFormRepository _formRepository;
    private readonly TestData _testData;

    public FormsAppService_Tests()
    {
        _formAppService = GetRequiredService<IFormAppService>();
        _formRepository = GetService<IFormRepository>();
        _testData = GetRequiredService<TestData>();
    }

    [Fact]
    public async Task Should_Create_Form()
    {
        CreateFormDto dto = new CreateFormDto()
        {
            Title = "My Form",
            Description = "My Desc",
        };

        var insertedForm = await _formAppService.CreateAsync(dto);

        insertedForm.ShouldNotBeNull();
        insertedForm.Description.ShouldContain("My Desc");
        insertedForm.Title.ShouldContain("My Form");
    }

    [Fact]
    public async Task Should_Create_Checkbox_Item_And_Return_It()
    {
        var testForm = await _formRepository.GetAsync(_testData.TestFormId);

        var checkboxDto = GetCreateItemDtoOfCheckbox();
        var result = await _formAppService.CreateQuestionAsync(testForm.Id, checkboxDto);
        result.QuestionType.ShouldBe(QuestionTypes.Checkbox);
    }

    [Fact]
    public async Task Should_Create_MultiChoice_Item()
    {
        var result = await _formAppService.CreateQuestionAsync(_testData.TestFormId, new CreateQuestionDto()
        {
            QuestionType = QuestionTypes.ChoiceMultiple,
            Title = "MultiChoice Title",
            Description = "MultiChoice Desc",
            Index = 1,
            Choices = CreateDummyChoices()
        });

        result.ShouldNotBeNull();
        result.Title.ShouldContain("MultiChoice");
        result.Index.ShouldBe(1);
        result.QuestionType.ShouldBe(QuestionTypes.ChoiceMultiple);

        result.Choices.Count.ShouldNotBe(0);
        var correctAnswer = result.Choices.First(q => q.IsCorrect == true);
        correctAnswer.ShouldNotBeNull();
        correctAnswer.Value.ShouldContain("GoldenKey");
    }

    [Fact]
    public async Task Should_Create_Non_Choosable_Item()
    {
        var result = await _formAppService.CreateQuestionAsync(_testData.TestFormId, new CreateQuestionDto()
        {
            QuestionType = QuestionTypes.ShortText,
            Title = "Text Title",
            Description = "Text Desc",
            Index = 2
        });

        result.ShouldNotBeNull();
        result.Title.ShouldContain("Text");
        result.Index.ShouldBe(2);
        result.QuestionType.ShouldBe(QuestionTypes.ShortText);

        result.Choices.Count.ShouldBe(0);
    }

    [Fact]
    public async Task Should_Get_Form_With_Items()
    {
        var form = await _formAppService.GetAsync(_testData.TestFormId);
        form.Questions.ShouldNotBeNull();
        form.Questions.Count.ShouldBe(4);
    }

    [Fact]
    public async Task Should_Get_Items_With_Choices_Of_Test_Form()
    {
        var itemList = await _formAppService.GetQuestionsAsync(_testData.TestFormId, new GetQuestionListDto());
        itemList.Count.ShouldNotBe(0);
        var st = itemList.First(q => q.QuestionType == QuestionTypes.ShortText);
        st.ShouldNotBeNull();

        var cm = itemList.First(q => q.QuestionType == QuestionTypes.ChoiceMultiple);
        cm.ShouldNotBeNull();
        var cmChoices = cm.Choices;
        cmChoices.ShouldNotBeNull();
        cmChoices.First().ShouldNotBeNull();

        var cb = itemList.First(q => q.QuestionType == QuestionTypes.Checkbox);
        cb.ShouldNotBeNull();
        var cbChoices = cb.Choices;
        cbChoices.ShouldNotBeNull();
        cbChoices.First().ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Update_Form_Options()
    {
        var form = await _formRepository.GetAsync(_testData.TestFormId);

        form.IsQuiz.ShouldBe(false);

        form.SetSettings(true, true, true, true, true);
        var updatedForm = await _formRepository.UpdateAsync(form);
        updatedForm.IsQuiz.ShouldBe(true);
    }

    [Fact]
    public async Task Should_Get_Seeded_Form_With_Items_Without_Choices()
    {
        var form = await _formAppService.GetAsync(_testData.TestFormId);
        var formItems = form.Questions;
        formItems.ShouldNotBeNull();
        formItems.Count.ShouldNotBe(0);
        var st = form.Questions.FirstOrDefault(q => q.Index == 1);
        st.ShouldNotBeNull();
        st.Title.ShouldContain("What is your name");
        var cm = form.Questions.FirstOrDefault(q => q.Index == 2);
        cm.ShouldNotBeNull();
        cm.Title.ShouldContain("Which technologies");
        var cb = form.Questions.FirstOrDefault(q => q.Index == 3);
        cb.ShouldNotBeNull();
        cb.Title.ShouldContain("Where are you located");
    }

    [Fact]
    public async Task Should_Get_Form_List()
    {
        var result = await _formAppService.GetListAsync(new GetFormListInputDto());

        result.TotalCount.ShouldNotBe(0);
    }

    [Fact]
    public async Task Should_Update_Test_Form()
    {
        var result = await _formAppService.UpdateAsync(_testData.TestFormId, new UpdateFormDto()
        {
            Description = "Updated Description",
            Title = "Updated Test Form Title"
        });
        result.Title.ShouldContain("Updated");
        result.Description.ShouldContain("updated");
    }

    [Fact]
    public async Task Should_Set_Options_Form_Test_Form()
    {
        await _formAppService.SetSettingsAsync(_testData.TestFormId, new UpdateFormSettingInputDto
        {
            IsQuiz = true,
            IsCollectingEmail = true,
            CanEditResponse = true,
            IsAcceptingResponses = true,
            HasLimitOneResponsePerUser = true
        });

        var updatedForm = await _formAppService.GetAsync(_testData.TestFormId);
        updatedForm.IsQuiz.ShouldBe(true);
        updatedForm.IsCollectingEmail.ShouldBe(true);
        updatedForm.CanEditResponse.ShouldBe(true);
        updatedForm.IsAcceptingResponses.ShouldBe(true);
        //updatedForm.HasLimitOneResponsePerUser.ShouldBe(true);
    }

    private List<ChoiceDto> CreateDummyChoices()
    {
        return new List<ChoiceDto>()
            {
                new ChoiceDto()
                {
                    IsCorrect = true,
                    Value = "GoldenKey"
                },
                new ChoiceDto()
                {
                    IsCorrect = false,
                    Value = "BronzeKey"
                },
                new ChoiceDto()
                {
                    IsCorrect = false,
                    Value = "CopperKey"
                },
                new ChoiceDto()
                {
                    IsCorrect = false,
                    Value = "StoneKey"
                }
            };
    }

    private CreateQuestionDto GetCreateItemDtoOfCheckbox()
    {
        return new CreateQuestionDto()
        {
            Title = "Checkbox Title",
            Description = "Checkbox Description",
            QuestionType = QuestionTypes.Checkbox,
            Choices = new List<ChoiceDto>()
                {
                    new ChoiceDto()
                    {
                        Value = "Checkbox Choice 1"
                    },
                    new ChoiceDto()
                    {
                        Value = "Checkbox Choice 2",
                        IsCorrect = true
                    },
                    new ChoiceDto()
                    {
                        Value = "Checkbox Choice 3"
                    },
                }
        };
    }
}
