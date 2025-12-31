using System.Threading.Tasks;
using Shouldly;
using Volo.Abp;
using Volo.Saas.Editions;
using Xunit;

namespace Volo.Saas;

public class EditionManager_Tests : SaasDomainTestBase
{
    private readonly EditionManager _editionManager;
    private readonly IEditionRepository _editionRepository;

    public EditionManager_Tests()
    {
        _editionManager = GetRequiredService<EditionManager>();
        _editionRepository = GetRequiredService<IEditionRepository>();
    }

    [Fact]
    public async Task CreateAsync()
    {
        var edition = await _editionManager.CreateAsync("Test");
        edition.DisplayName.ShouldBe("Test");
    }

    [Fact]
    public async Task Create_Edition_Name_Can_Not_Duplicate()
    {
        await Assert.ThrowsAsync<BusinessException>(async () => await _editionManager.CreateAsync("FirstEdition"));
    }

    [Fact]
    public async Task ChangeDisplayNameAsync()
    {
        var edition = await _editionRepository.FindByDisplayNameAsync("FirstEdition");
        edition.ShouldNotBeNull();

        await _editionManager.ChangeDisplayNameAsync(edition, "newFirstEdition");

        edition.DisplayName.ShouldBe("newFirstEdition");
    }

    [Fact]
    public async Task ChangeDisplayName_Edition_DisplayName_Can_Not_Duplicate()
    {
        var edition = await _editionRepository.FindByDisplayNameAsync("FirstEdition");
        edition.ShouldNotBeNull();

        await Assert.ThrowsAsync<BusinessException>(async () => await _editionManager.ChangeDisplayNameAsync(edition, "SecondEdition"));
    }
}
