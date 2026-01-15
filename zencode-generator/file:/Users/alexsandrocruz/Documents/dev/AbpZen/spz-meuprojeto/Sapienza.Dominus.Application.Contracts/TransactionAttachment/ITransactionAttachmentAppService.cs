using System;
using System.Threading.Tasks;
using Sapienza.Dominus.TransactionAttachment.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.TransactionAttachment;

public interface ITransactionAttachmentAppService :
    ICrudAppService<
        TransactionAttachmentDto,
        Guid,
        TransactionAttachmentGetListInput,
        CreateUpdateTransactionAttachmentDto,
        CreateUpdateTransactionAttachmentDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetTransactionAttachmentLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
