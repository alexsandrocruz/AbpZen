using System;
using System.Threading.Tasks;
using Sapienza.Dominus.Transaction.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.Transaction;

public interface ITransactionAppService :
    ICrudAppService<
        TransactionDto,
        Guid,
        TransactionGetListInput,
        CreateUpdateTransactionDto,
        CreateUpdateTransactionDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetTransactionLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
