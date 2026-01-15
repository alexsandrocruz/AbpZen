using System;
using System.Threading.Tasks;
using Sapienza.Dominus.Budget.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.Budget;

public interface IBudgetAppService :
    ICrudAppService<
        BudgetDto,
        Guid,
        BudgetGetListInput,
        CreateUpdateBudgetDto,
        CreateUpdateBudgetDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetBudgetLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
