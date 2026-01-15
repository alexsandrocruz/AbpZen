using System;
using System.Threading.Tasks;
using Sapienza.Dominus.ChatMessage.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.ChatMessage;

public interface IChatMessageAppService :
    ICrudAppService<
        ChatMessageDto,
        Guid,
        ChatMessageGetListInput,
        CreateUpdateChatMessageDto,
        CreateUpdateChatMessageDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetChatMessageLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
