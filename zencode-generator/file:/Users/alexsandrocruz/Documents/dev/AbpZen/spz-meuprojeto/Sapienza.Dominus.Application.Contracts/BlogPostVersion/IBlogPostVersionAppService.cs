using System;
using System.Threading.Tasks;
using Sapienza.Dominus.BlogPostVersion.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.BlogPostVersion;

public interface IBlogPostVersionAppService :
    ICrudAppService<
        BlogPostVersionDto,
        Guid,
        BlogPostVersionGetListInput,
        CreateUpdateBlogPostVersionDto,
        CreateUpdateBlogPostVersionDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetBlogPostVersionLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
