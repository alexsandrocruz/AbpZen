using System;
using System.Threading.Tasks;
using Sapienza.Lexus.Case.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.Case;

public interface ICaseAppService :
    ICrudAppService<
        CaseDto,
        Guid,
        CaseGetListInput,
        CreateUpdateCaseDto,
        CreateUpdateCaseDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetCaseLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
