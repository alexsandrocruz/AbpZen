using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Volo.Chat.Messages;

namespace Volo.Chat.Blazor.Components;

public interface IChatHubConnectionService
{
    Guid LastNotificationMessageId { get; set; }
    
    Task ReceivedMessageAsync(ChatMessageRdto message);

    Task OnReceiveMessageAsync(Func<ChatMessageRdto, Task> func);

    Task DeletedMessageAsync(Guid messageId);
    
    Task OnDeletedMessageAsync(Func<Guid, Task> func);
    
    Task DeletedConversationAsync(Guid userId);
    
    Task OnDeletedConversationAsync(Func<Guid, Task> func);
}