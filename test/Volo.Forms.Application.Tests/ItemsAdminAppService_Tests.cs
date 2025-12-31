using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Guids;
using Volo.Forms.Choices;
using Volo.Forms.Questions;
using Xunit;

namespace Volo.Forms;

public class ItemsAdminAppService_Tests : FormsApplicationTestBase
{
    private readonly IQuestionAppService _questionAppService;
    private readonly IChoiceRepository _choiceRepository;
    private readonly TestData _testData;
    private readonly IGuidGenerator _guidGenerator;

    public ItemsAdminAppService_Tests()
    {
        _guidGenerator = GetRequiredService<IGuidGenerator>();
        _testData = GetRequiredService<TestData>();
        _choiceRepository = GetService<IChoiceRepository>();
        _questionAppService = GetRequiredService<IQuestionAppService>();
    }

    [Fact]
    public async Task Should_Get_Test_CheckboxItem_With_Choices()
    {
        var item = await _questionAppService.GetAsync(_testData.TestCheckboxId);
        item.Choices.ShouldNotBeNull();
        //item.Choices.Count.ShouldNotBe(0); //EfCore bug:https://github.com/dotnet/efcore/issues/22016
        //item.Choices.First(q => q.Value.Contains("C#")).ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Delete_TestCheckbox_Item_Soft_Delete_Choices()
    {
        var choiceList = await _choiceRepository.GetListAsync();
        await _questionAppService.DeleteAsync(_testData.TestCheckboxId);
        var itemList = await _questionAppService.GetListAsync(new GetQuestionListDto());
        itemList.Count.ShouldBeLessThan(4);
        var updated = await _choiceRepository.GetListAsync();
        updated.Count.ShouldBe(choiceList.Count); //Soft-Deleted
    }

    [Fact]
    public async Task Should_Update_ShortText_Item_To_MultiChoice_Item()
    {
        var result = await _questionAppService.UpdateAsync(_testData.TestShortTextId, new UpdateQuestionDto()
        {
            Index = 2,
            Title = "Updated",
            QuestionType = QuestionTypes.ChoiceMultiple,
            Choices = CreateDummyChoices()
        });

        result.ShouldNotBeNull();
        result.Id.ShouldBe(_testData.TestShortTextId);
        result.QuestionType.ShouldBe(QuestionTypes.ChoiceMultiple);
        result.Choices.Count.ShouldBe(4);
    }

    [Fact]
    public async Task Should_Update_Checkbox_Item_To_MultiChoice_Item()
    {
        var choices = await _choiceRepository.GetListAsync();
        choices.Count.ShouldBe(13);
        var result = await _questionAppService.UpdateAsync(_testData.TestCheckboxId, new UpdateQuestionDto()
        {
            Index = 2,
            Title = "Updated",
            QuestionType = QuestionTypes.ChoiceMultiple,
            Choices = new List<ChoiceDto>()
                {
                    new ChoiceDto()
                    {
                        Id = _testData.TestChoiceCSharp,
                        Value = "C#"
                    },
                    new ChoiceDto()
                    {
                        Id = _testData.TestChoiceJava,
                        Value = "Java"
                    },
                    new ChoiceDto()
                    {
                        Id = _testData.TestChoiceJavascript,
                        Value = "JavaScript"
                    }
                }
        });
        var updatedChoices = await _choiceRepository.GetListAsync();
        updatedChoices.Count.ShouldBeLessThan(13);
        result.ShouldNotBeNull();
        result.Id.ShouldBe(_testData.TestCheckboxId);
        result.QuestionType.ShouldBe(QuestionTypes.ChoiceMultiple);
        result.Choices.Count.ShouldBe(3);
        result.Choices.First().Id.ShouldBe(_testData.TestChoiceCSharp);
    }

    [Fact]
    public async Task Should_Update_Checkbox_Item_To_ShorText_Item()
    {
        var choices = await _choiceRepository.GetListAsync();
        choices.Count.ShouldBe(13);
        var result = await _questionAppService.UpdateAsync(_testData.TestCheckboxId, new UpdateQuestionDto()
        {
            Index = 2,
            Title = "Updated ShortText",
            QuestionType = QuestionTypes.ShortText
        });

        result.ShouldNotBeNull();
        result.QuestionType.ShouldBe(QuestionTypes.ShortText);
        result.Id.ShouldBe(_testData.TestCheckboxId);
        result.Title.ShouldContain("Updated");
        var updatedChoices = await _choiceRepository.GetListAsync();
        updatedChoices.Count.ShouldBeLessThan(13);
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
}
