using System;
using System.Threading.Tasks;
using Sapienza.Dominus.BlogCategory.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.BlogCategory;

public interface IBlogCategoryAppService :
    ICrudAppService<
        BlogCategoryDto,
        Guid,
        BlogCategoryGetListInput,
        CreateUpdateBlogCategoryDto,
        CreateUpdateBlogCategoryDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetBlogCategoryLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
