using System;
using System.Threading.Tasks;
using Sapienza.Dominus.Contract.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.Contract;

public interface IContractAppService :
    ICrudAppService<
        ContractDto,
        Guid,
        ContractGetListInput,
        CreateUpdateContractDto,
        CreateUpdateContractDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetContractLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
