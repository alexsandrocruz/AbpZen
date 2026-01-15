using System;
using System.Threading.Tasks;
using Sapienza.Dominus.MessageAttachment.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.MessageAttachment;

public interface IMessageAttachmentAppService :
    ICrudAppService<
        MessageAttachmentDto,
        Guid,
        MessageAttachmentGetListInput,
        CreateUpdateMessageAttachmentDto,
        CreateUpdateMessageAttachmentDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetMessageAttachmentLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
