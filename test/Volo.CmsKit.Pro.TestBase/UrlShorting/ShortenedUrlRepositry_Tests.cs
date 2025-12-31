using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.CmsKit.UrlShorting;
using Xunit;

namespace Volo.CmsKit.Pro.UrlShorting
{
    public abstract class ShortenedUrlRepository_Tests<TStartupModule> : CmsKitProTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly CmsKitProTestData _cmsKitProTestData;
        private readonly IShortenedUrlRepository _shortenedUrlRepository;

        public ShortenedUrlRepository_Tests()
        {
            _cmsKitProTestData = GetRequiredService<CmsKitProTestData>();
            _shortenedUrlRepository = GetRequiredService<IShortenedUrlRepository>();
        }
        [Fact]
        public async Task GetListAsync()
        {
            var data = await _shortenedUrlRepository.GetListAsync();

            data.ShouldNotBeNull();
            data.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task FindAsync()
        {
            var data = await _shortenedUrlRepository.FindBySourceUrlAsync(_cmsKitProTestData.ShortenedUrlSource1);

            data.ShouldNotBeNull();
            data.Id.ShouldBe(_cmsKitProTestData.ShortenedUrlId1);
            data.Source.ShouldBe(_cmsKitProTestData.ShortenedUrlSource1);
            data.Target.ShouldBe(_cmsKitProTestData.ShortenedUrlTarget1);
        }

        [Fact]
        public async Task GetCountAsync()
        {
            var count = await _shortenedUrlRepository.GetCountAsync(
                null);

            count.ShouldBeGreaterThan(0);
        }
    }
}
