using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Forms.Responses;
using Xunit;

namespace Volo.Forms.Forms;

public abstract class ResponseRepository_Tests<TStartupModule> : FormsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly TestData _testData;
    private readonly IResponseRepository _formResponseRepository;

    protected ResponseRepository_Tests()
    {
        _formResponseRepository = GetRequiredService<IResponseRepository>();
        _testData = GetRequiredService<TestData>();
    }

    [Fact]
    public async Task Should_Get_TestUser_Responses()
    {
        var responses = await _formResponseRepository.GetByUserId(_testData.TestUser1);
        responses.Count.ShouldBe(2);
    }

    [Fact]
    public async Task TestUser_ShouldHave_FormResponse()
    {
        var responseExists = await _formResponseRepository.UserResponseExistsAsync(_testData.TestFormId, _testData.TestUser1);

        responseExists.ShouldBeTrue();
    }

    [Fact]
    public async Task TestUser_ShouldNotHave_FormResponse()
    {
        var responseExists = await _formResponseRepository.UserResponseExistsAsync(_testData.TestFormId, Guid.NewGuid());

        responseExists.ShouldBeFalse();
    }
}
