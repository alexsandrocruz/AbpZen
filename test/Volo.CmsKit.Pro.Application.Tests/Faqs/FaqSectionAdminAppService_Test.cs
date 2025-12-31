using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.CmsKit.Admin.Faqs;
using Volo.CmsKit.Faqs;
using Xunit;

namespace Volo.CmsKit.Pro.Faqs;

public class FaqSectionAdminAppService_Test : CmsKitProApplicationTestBase
{
    private readonly IFaqSectionAdminAppService _faqSectionAdminAppService;
    private readonly FaqOptions _faqOptions;
    
    public FaqSectionAdminAppService_Test()
    {
        _faqSectionAdminAppService = GetRequiredService<IFaqSectionAdminAppService>();
        _faqOptions = GetRequiredService<IOptions<FaqOptions>>().Value;
    }

    [Fact]
    public async Task GetListAsync()
    {
        var faq = await _faqSectionAdminAppService.GetListAsync(new FaqSectionListFilterDto());

        faq.ShouldNotBeNull();
        faq.TotalCount.ShouldBeGreaterThan(0);
    }
}
