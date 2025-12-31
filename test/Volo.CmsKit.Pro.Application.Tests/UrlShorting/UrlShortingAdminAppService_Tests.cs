using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp;
using Volo.CmsKit.Admin.UrlShorting;
using Volo.CmsKit.UrlShorting;
using Xunit;

namespace Volo.CmsKit.Pro.UrlShorting
{
    public class UrlShortingAdminAppService_Tests: CmsKitProApplicationTestBase
    {
        private readonly  IUrlShortingAdminAppService _urlShortingAdminAppService;
        private readonly CmsKitProTestData _cmsKitProTestData;

        public UrlShortingAdminAppService_Tests()
        {
            _urlShortingAdminAppService = GetRequiredService<IUrlShortingAdminAppService>();
            _cmsKitProTestData = GetRequiredService<CmsKitProTestData>();
        }

        [Fact]
        public async Task CreateAsync()
        {
            var url = new CreateShortenedUrlDto()
            {
                Source = "mySourceUrl",
                Target = "myTargetUrl",
            };
            await _urlShortingAdminAppService.CreateAsync(url);

            UsingDbContext(context =>
            {
                var urls = context.Set<ShortenedUrl>().ToList();

                urls
                    .Any(c => c.Target == url.Target)
                    .ShouldBeTrue();
            });
        }

        [Fact]
        public async Task CreateAsync_Exception()
        {
            var url = new CreateShortenedUrlDto()
            {
                Source = _cmsKitProTestData.ShortenedUrlSource1,
                Target = "myTargetUrl",
            };

            await Should.ThrowAsync<ShortenedUrlAlreadyExistsException>(async () =>
                    await _urlShortingAdminAppService.CreateAsync(url)
                );
        }

        [Fact]
        public async Task UpdateAsync()
        {
            var url = await _urlShortingAdminAppService.GetAsync(_cmsKitProTestData.ShortenedUrlId1);

            await _urlShortingAdminAppService.UpdateAsync(url.Id, new UpdateShortenedUrlDto {
                Target = "MyTarget"
            });

            UsingDbContext(context =>
            {
                var updatedUrl = context.Set<ShortenedUrl>().FirstOrDefault(u=> u.Id == url.Id);

                updatedUrl.ShouldNotBeNull();
                updatedUrl.Target.ShouldBe("MyTarget");
            });
        }

        [Fact]
        public async Task DeleteAsync()
        {
            await _urlShortingAdminAppService.DeleteAsync(_cmsKitProTestData.ShortenedUrlId1);

            UsingDbContext(context =>
            {
                var deletedUrl = context.Set<ShortenedUrl>().FirstOrDefault(u=> u.Id == _cmsKitProTestData.ShortenedUrlId1);

                deletedUrl.ShouldBeNull();
            });
        }

        [Fact]
        public async Task GetListAsync()
        {
            var result = await _urlShortingAdminAppService.GetListAsync(new GetShortenedUrlListInput {
                ShortenedUrlFilter = null
            });

            result.ShouldNotBeNull();
            result.TotalCount.ShouldBe(2);
            result.Items.Count.ShouldBe(2);
        }

        [Fact]
        public async Task GetAsync()
        {
            var url = await _urlShortingAdminAppService.GetAsync(_cmsKitProTestData.ShortenedUrlId1);

            url.ShouldNotBeNull();
            url.Id.ShouldBe(_cmsKitProTestData.ShortenedUrlId1);
            url.Source.ShouldBe(_cmsKitProTestData.ShortenedUrlSource1);
            url.Target.ShouldBe(_cmsKitProTestData.ShortenedUrlTarget1);
        }

    }
}
