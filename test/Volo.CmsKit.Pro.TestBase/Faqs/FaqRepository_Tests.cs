using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.CmsKit.Faqs;
using Xunit;

namespace Volo.CmsKit.Pro.Faqs;

public abstract class FaqRepository_Tests<TStartupModule> : CmsKitProTestBase<TStartupModule> where TStartupModule : IAbpModule
{
    private readonly IFaqSectionRepository _faqSectionRepository;

    protected FaqRepository_Tests()
    {
        _faqSectionRepository = GetRequiredService<IFaqSectionRepository>();
    }
    
    [Fact]
    public async Task GetListAsync()
    {
        var faqs = await _faqSectionRepository.GetListAsync();

        faqs.ShouldNotBeNull();
        faqs.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task GetCountAsync()
    {
        var count = await _faqSectionRepository.GetCountAsync(null);

        count.ShouldBeGreaterThan(0);
    }
}
