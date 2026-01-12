using System;
using System.Threading.Tasks;
using LeptonXDemoApp.Category.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LeptonXDemoApp.Category;

public interface ICategoryAppService :
    ICrudAppService<
        CategoryDto,
        Guid,
        CategoryGetListInput,
        CreateUpdateCategoryDto,
        CreateUpdateCategoryDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetCategoryLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
