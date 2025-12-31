using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Authorization;
using Volo.Abp.Data;
using Volo.Abp.Features;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;
using Volo.Abp.Users;
using Volo.Chat.Authorization;
using Volo.Chat.Messages;
using Volo.Chat.Users;

namespace Volo.Chat.Conversations;

[RequiresFeature(ChatFeatures.Enable)]
[Authorize(ChatPermissions.Messaging)]
public class ConversationAppService : ChatAppService, IConversationAppService
{
    private readonly MessagingManager _messagingManager;
    private readonly IChatUserLookupService _chatUserLookupService;
    private readonly IConversationRepository _conversationRepository;
    private readonly IRealTimeChatMessageSender _realTimeChatMessageSender;
    private readonly IAuthorizationService _authorizationService;
    private readonly IPermissionFinder _permissionFinder;

    public ConversationAppService(
        MessagingManager messagingManager,
        IChatUserLookupService chatUserLookupService,
        IConversationRepository conversationRepository,
        IRealTimeChatMessageSender realTimeChatMessageSender,
        IAuthorizationService authorizationService,
        IPermissionFinder permissionFinder)
    {
        _messagingManager = messagingManager;
        _chatUserLookupService = chatUserLookupService;
        _conversationRepository = conversationRepository;
        _realTimeChatMessageSender = realTimeChatMessageSender;
        _authorizationService = authorizationService;
        _permissionFinder = permissionFinder;
    }

    public virtual async Task<ChatMessageDto> SendMessageAsync(SendMessageInput input)
    {
        var targetUser = await _chatUserLookupService.FindByIdAsync(input.TargetUserId);
        if (targetUser == null)
        {
            throw new BusinessException("Volo.Chat:010002");
        }

        if (!await _permissionFinder.IsGrantedAsync(targetUser.Id, ChatPermissions.Messaging))
        {
            throw new BusinessException("Volo.Chat:010004");
        }

        var hasGrantToStartConversation = await _authorizationService.IsGrantedAsync(ChatPermissions.Searching);
        if (!hasGrantToStartConversation)
        {
            var hasConversation = await _messagingManager.HasConversationAsync(targetUser.Id);
            if (!hasConversation)
            {
                throw new AbpAuthorizationException(code: AbpAuthorizationErrorCodes.GivenRequirementHasNotGrantedForGivenResource);
            }
        }

        Message message;
        using (var uow = UnitOfWorkManager.Begin(requiresNew: true))
        {
            message = await _messagingManager.CreateNewMessage(
                CurrentUser.GetId(),
                targetUser.Id,
                input.Message
            );

            await uow.CompleteAsync();
        }

        var senderUser = await _chatUserLookupService.FindByIdAsync(CurrentUser.GetId());
        await _realTimeChatMessageSender.SendAsync(
            targetUser.Id,
            new ChatMessageRdto
            {
                Id = message.Id ,
                SenderName = senderUser.Name,
                SenderSurname = senderUser.Surname,
                SenderUserId = senderUser.Id,
                SenderUsername = senderUser.UserName,
                Text = input.Message
            }
        );
        
        return new ChatMessageDto
        {
            Id = message.Id,
            Message = message.Text,
            MessageDate = message.CreationTime,
            ReadDate = message.ReadTime ?? DateTime.MaxValue,
            IsRead = message.IsAllRead,
            Side = ChatMessageSide.Sender
        };
    }
 
    public virtual async Task DeleteMessageAsync(DeleteMessageInput input)
    {
        await _messagingManager.DeleteMessage(input.MessageId, CurrentUser.GetId(), input.TargetUserId);
        
        await _realTimeChatMessageSender.DeleteMessageAsync(
            input.TargetUserId,
            input.MessageId
        );
    }

    public virtual async Task<ChatConversationDto> GetConversationAsync(GetConversationInput input)
    {
        var targetUser = await _chatUserLookupService.FindByIdAsync(input.TargetUserId);
        if (targetUser == null)
        {
            throw new BusinessException("Volo.Chat:010003");
        }

        var chatConversation = new ChatConversationDto
        {
            TargetUserInfo = new ChatTargetUserInfo
            {
                UserId = targetUser.Id,
                Name = targetUser.Name,
                Surname = targetUser.Surname,
                Username = targetUser.UserName,
            },
            Messages = new List<ChatMessageDto>()
        };

        var messages = await _messagingManager.ReadMessagesAsync(targetUser.Id, input.SkipCount, input.MaxResultCount);

        chatConversation.Messages.AddRange(
            messages.Select(x => new ChatMessageDto
            {
                Id = x.Message.Id,
                Message = x.Message.Text,
                MessageDate = x.Message.CreationTime,
                ReadDate = x.Message.ReadTime ?? DateTime.MaxValue,
                IsRead = x.Message.IsAllRead,
                Side = x.UserMessage.Side
            })
            );


        return chatConversation;
    }

    public virtual async Task MarkConversationAsReadAsync(MarkConversationAsReadInput input)
    {
        try
        {
            using (var uow = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: UnitOfWorkManager.Current?.Options.IsTransactional ?? false))
            {
                var conversationPair = await _conversationRepository.FindPairAsync(CurrentUser.GetId(), input.TargetUserId);

                if (conversationPair.SenderConversation.LastMessageSide == ChatMessageSide.Receiver)
                {
                    conversationPair.SenderConversation.ResetUnreadMessageCount();
                    await _conversationRepository.UpdateAsync(conversationPair.SenderConversation);
                }

                await uow.CompleteAsync();
            }
        }
        catch (AbpDbConcurrencyException e)
        {
            // The conversation is change by another request. So, we can ignore this exception.
        }
    }
    
    public async Task DeleteConversationAsync(DeleteConversationInput input)
    {
        await _messagingManager.DeleteConversationAsync(CurrentUser.GetId(), input.TargetUserId);
        
        await _realTimeChatMessageSender.DeleteConversationAsync(
            input.TargetUserId,
            CurrentUser.GetId()
        );
    }
}
