using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.CmsKit.Faqs;
using Volo.CmsKit.Public.Faqs;
using Xunit;

namespace Volo.CmsKit.Pro.Faqs;

public class FaqPublicAppService_Tests : CmsKitProApplicationTestBase
{
    private readonly IFaqSectionPublicAppService _faqSectionPublicAppService;
    private readonly FaqOptions _faqOptions;
    
    public FaqPublicAppService_Tests()
    {
        _faqSectionPublicAppService = GetRequiredService<IFaqSectionPublicAppService>();
        _faqOptions = GetRequiredService<IOptions<FaqOptions>>().Value;
    }

    [Fact]
    public async Task GetListAsync()
    {
        var faq = await _faqSectionPublicAppService.GetListSectionWithQuestionsAsync(
            new FaqSectionListFilterInput() {
               GroupName = _faqOptions.Groups.FirstOrDefault().Key,
               SectionName = "" 
            });

        faq.ShouldNotBeNull();
    }
}
