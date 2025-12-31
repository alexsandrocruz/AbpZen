using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Chat.Messages;

namespace Volo.Chat.Conversations;

[RemoteService(Name = ChatRemoteServiceConsts.RemoteServiceName)]
[Area(ChatRemoteServiceConsts.ModuleName)]
[Route("api/chat/conversation")]
public class ConversationController : ChatController, IConversationAppService
{
    private readonly IConversationAppService _conversationAppService;

    public ConversationController(IConversationAppService conversationAppService)
    {
        _conversationAppService = conversationAppService;
    }

    [HttpPost]
    [Route("send-message")]
    public Task<ChatMessageDto> SendMessageAsync(SendMessageInput input)
    {
        return _conversationAppService.SendMessageAsync(input);
    }

    [HttpDelete]
    [Route("delete-message")]
    public Task DeleteMessageAsync(DeleteMessageInput input)
    {
        return _conversationAppService.DeleteMessageAsync(input);
    }

    [HttpGet]
    [Route("conversation")]
    public Task<ChatConversationDto> GetConversationAsync(GetConversationInput input)
    {
        return _conversationAppService.GetConversationAsync(input);
    }

    [HttpPost]
    [Route("mark-conversation-as-read")]
    public Task MarkConversationAsReadAsync(MarkConversationAsReadInput input)
    {
        return _conversationAppService.MarkConversationAsReadAsync(input);
    }

    [HttpDelete]
    [Route("delete-conversation")]
    public Task DeleteConversationAsync(DeleteConversationInput input)
    {
        return _conversationAppService.DeleteConversationAsync(input);
    }
}
