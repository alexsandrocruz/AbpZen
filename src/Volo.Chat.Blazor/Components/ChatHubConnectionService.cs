using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Volo.Abp.DependencyInjection;
using Volo.Chat.Messages;

namespace Volo.Chat.Blazor.Components;

public class ChatHubConnectionService : IChatHubConnectionService, IScopedDependency
{
     private readonly List<Func<ChatMessageRdto, Task>> _messageReceived;
     private readonly List<Func<Guid, Task>> _messageDeleted;
     private readonly List<Func<Guid, Task>> _conversationDeleted;
     
     public ChatHubConnectionService()
     {
          _messageReceived =  new List<Func<ChatMessageRdto, Task>>();
          _messageDeleted = new List<Func<Guid, Task>>();
          _conversationDeleted = new List<Func<Guid, Task>>();
          
     }
     
     public Guid LastNotificationMessageId { get; set; }

     public async Task ReceivedMessageAsync(ChatMessageRdto message)
     {
          foreach (var func in _messageReceived)
          {
               await func(message);
          }
     }

     public Task OnReceiveMessageAsync(Func<ChatMessageRdto, Task> func)
     {
          _messageReceived.Add(func);
          return Task.CompletedTask;
     }

     public async Task DeletedMessageAsync(Guid messageId)
     {
          foreach (var func in _messageDeleted)
          {
               await func(messageId);
          }
     }

     public Task OnDeletedMessageAsync(Func<Guid, Task> func)
     {
          _messageDeleted.Add(func);
          return Task.CompletedTask;
     }

     public async Task DeletedConversationAsync(Guid userId)
     {
          foreach (var func in _conversationDeleted)
          {
               await func(userId);
          }
     }

     public Task OnDeletedConversationAsync(Func<Guid, Task> func)
     {
          _conversationDeleted.Add(func);
          return Task.CompletedTask;
     }
}