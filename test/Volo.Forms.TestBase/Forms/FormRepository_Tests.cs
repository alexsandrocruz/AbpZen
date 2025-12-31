using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Guids;
using Volo.Abp.Modularity;
using Volo.Forms.Questions.ChoosableItems;
using Xunit;

namespace Volo.Forms.Forms;

public abstract class FormRepository_Tests<TStartupModule> : FormsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly IFormRepository _formRepository;
    private readonly IGuidGenerator _guidGenerator;
    private readonly TestData _testData;

    public FormRepository_Tests()
    {
        _formRepository = GetRequiredService<IFormRepository>();
        _guidGenerator = GetRequiredService<IGuidGenerator>();
        _testData = GetRequiredService<TestData>();
    }

    [Fact]
    public async Task Should_Get_TestForm()
    {
        var formList = await _formRepository.GetListAsync();
        formList.Count.ShouldNotBe(0);
        var testForm = formList.First();
        testForm.ShouldNotBeNull();
        testForm.Id.ShouldBe(_testData.TestFormId);
    }

    [Fact]
    public async Task TestForm_Should_Have_Items_With_Different_Types()
    {
        var testForm = await _formRepository.GetWithQuestionsAsync(_testData.TestFormId);
        testForm.Questions.Count.ShouldNotBe(0);

        var checkboxes = testForm.Questions.Where(q => q.GetQuestionType() == QuestionTypes.Checkbox);
        var ch = checkboxes.First().As<Checkbox>();
        ch.ShouldNotBeNull();
    }

    // [Fact]
    // public async Task TestForm_Should_Have_Items_With_Answers()
    // {
    //     var testForm = await _formRepository.GetQuestionsWithAnswersAsync(_testData.TestFormId);
    //     testForm.Answers.Count.ShouldNotBe(0);
    // }
}
