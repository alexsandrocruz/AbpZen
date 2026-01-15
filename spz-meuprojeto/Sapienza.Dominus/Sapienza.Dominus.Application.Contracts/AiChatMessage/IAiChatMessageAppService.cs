using System;
using System.Threading.Tasks;
using Sapienza.Dominus.AiChatMessage.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.AiChatMessage;

public interface IAiChatMessageAppService :
    ICrudAppService<
        AiChatMessageDto,
        Guid,
        AiChatMessageGetListInput,
        CreateUpdateAiChatMessageDto,
        CreateUpdateAiChatMessageDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetAiChatMessageLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
