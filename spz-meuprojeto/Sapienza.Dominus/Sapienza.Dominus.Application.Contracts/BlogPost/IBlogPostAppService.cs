using System;
using System.Threading.Tasks;
using Sapienza.Dominus.BlogPost.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.BlogPost;

public interface IBlogPostAppService :
    ICrudAppService<
        BlogPostDto,
        Guid,
        BlogPostGetListInput,
        CreateUpdateBlogPostDto,
        CreateUpdateBlogPostDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetBlogPostLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
