using System;
using System.Threading.Tasks;
using Sapienza.Dominus.FinancialCategory.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.FinancialCategory;

public interface IFinancialCategoryAppService :
    ICrudAppService<
        FinancialCategoryDto,
        Guid,
        FinancialCategoryGetListInput,
        CreateUpdateFinancialCategoryDto,
        CreateUpdateFinancialCategoryDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetFinancialCategoryLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
