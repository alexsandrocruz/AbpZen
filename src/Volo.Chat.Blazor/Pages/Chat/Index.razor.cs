using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Volo.Chat.Authorization;
using Volo.Chat.Blazor.Components;
using Volo.Chat.Conversations;
using Volo.Chat.Localization;
using Volo.Chat.Messages;
using Volo.Chat.Settings;
using Volo.Chat.Users;


namespace Volo.Chat.Blazor.Pages.Chat;

public partial class Index
{
    [Inject]
    public IContactAppService ContactAppService { get; set; }

    [Inject]
    public IConversationAppService ConversationAppService { get; set; }

    [Inject]
    public IJSRuntime JsRuntime { get; set; }

    public ISettingsAppService SettingsAppService { get; set; }
    
    [Inject]
    public IAuthorizationService AuthorizationService { get; set; }

    public List<ChatContactDto> ChatContactDtos { get; set; } = new List<ChatContactDto>();

    public Dictionary<ChatContactDto, string> ChatContactsActive { get; set; } = new Dictionary<ChatContactDto, string>();

    public Dictionary<ChatContactDto, ElementReference> CanvasElementReferences { get; set; } = new Dictionary<ChatContactDto, ElementReference>();

    public string SearchValue { get; set; }

    public ChatContactDto CurrentChatContact { get; set; }

    public ElementReference CurrentChatContactCanvas { get; set; }

    public ChatConversationDto ChatConversationDto { get; set; }

    public string Message { get; set; }

    public ElementReference MessageTextArea { get; set; }

    public bool SendOnEnter { get; set; }

    [Inject]
    protected NavigationManager Navigation { get; set; }

    [Inject]
    protected IChatHubConnectionService ChatHubConnectionService { get; set; }
    
    protected ChatSettingsDto ChatSettings { get; set; }

    public Index()
    {
        LocalizationResource = typeof(ChatResource);
    }

    public string ChatMessagesContainerStyle { get; set; }
    public bool HasSearchingPermission { get; set; }

    protected async override Task OnInitializedAsync()
    {
        await GetChatSettingsAsync();

        HasSearchingPermission = await AuthorizationService.IsGrantedAsync(ChatPermissions.Searching);

        await ChatHubConnectionService.OnReceiveMessageAsync(async message =>
        {
            if (CurrentChatContact != null && CurrentChatContact.UserId == message.SenderUserId)
            {
                ChatConversationDto = await ConversationAppService.GetConversationAsync(
                    new GetConversationInput { TargetUserId = CurrentChatContact.UserId, MaxResultCount = 100 });

                if (CurrentChatContact.UnreadMessageCount > 0)
                {
                    await ConversationAppService.MarkConversationAsReadAsync(
                        new MarkConversationAsReadInput { TargetUserId = CurrentChatContact.UserId });
                }

                ChatConversationDto.Messages.Reverse();
                var lastMessage = ChatConversationDto.Messages.LastOrDefault();
                CurrentChatContact.LastMessage = lastMessage?.Message;
                CurrentChatContact.LastMessageDate = lastMessage?.MessageDate;
            }

            await InvokeAsync(StateHasChanged);
        });

        await ChatHubConnectionService.OnDeletedMessageAsync(async messageId =>
        {
            ChatConversationDto.Messages.RemoveAll(message => message.Id == messageId);
            var lastMessage = ChatConversationDto.Messages.LastOrDefault();
            CurrentChatContact.LastMessage = lastMessage?.Message;
            CurrentChatContact.LastMessageDate = lastMessage?.MessageDate;
            await InvokeAsync(StateHasChanged);
        });
        
        await ChatHubConnectionService.OnDeletedConversationAsync(async userId =>
        {
            if (userId == CurrentChatContact.UserId)
            {
                CanvasElementReferences.Clear();
                ChatConversationDto = null;
                ChatContactDtos.RemoveAll(contact => contact.UserId == userId);
            }
            await InvokeAsync(StateHasChanged);
        });
    }

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if(firstRender)
        {
            await GetContactsAsync();
        }

        foreach (var contactDto in ChatContactDtos)
        {
            if (CanvasElementReferences.ContainsKey(contactDto))
            {
                await JsRuntime.InvokeVoidAsync("VoloChatAvatarManager.createCanvasForUser", CanvasElementReferences[contactDto], contactDto.Username, GetName(contactDto));
            }
        }

        await JsRuntime.InvokeVoidAsync("eval", "document.getElementById('chat-messages-container').scrollTop = document.getElementById('chat-messages-container').scrollHeight");
    }

    public static string GetName(ChatContactDto contact)
    {
        var name = "";

        if (!string.IsNullOrEmpty(contact.Name))
        {
            name += contact.Name + ' ';
        }

        if (!string.IsNullOrEmpty(contact.Surname))
        {
            name += contact.Surname;
        }

        if (name == string.Empty)
        {
            name = contact.Username;
        }

        return name;
    }

    public async Task GetContactsAsync(bool includeOtherContacts = false)
    {
        CanvasElementReferences.Clear();
        ChatContactsActive.Clear();

        ChatContactDtos = await ContactAppService.GetContactsAsync(new GetContactsInput
        {
            Filter = SearchValue,
            IncludeOtherContacts = includeOtherContacts
        });

        foreach (var contactDto in ChatContactDtos)
        {
            ChatContactsActive[contactDto] = "";
        }

        CurrentChatContact = ChatContactDtos.FirstOrDefault();
        if (CurrentChatContact != null)
        {
            await SetActiveAsync(CurrentChatContact);
        }
        else
        {
            await JsRuntime.InvokeVoidAsync("VoloChatAvatarManager.createCanvasForUser", CurrentChatContactCanvas, null, null);
            ChatConversationDto = null;
        }

        await InvokeAsync(StateHasChanged);
    }
    
    protected virtual bool IsDeletingMessageEnabled(ChatMessageDto message)
    {
        if (ChatSettings.DeletingMessages == ChatDeletingMessages.Disabled)
        {
            return false;
        }

        if (ChatSettings.DeletingMessages == ChatDeletingMessages.EnabledWithDeletionPeriod)
        {
            if(message.MessageDate.AddSeconds(ChatSettings.MessageDeletionPeriod) < Clock.Now)
            {
                return false;
            }
        }

        return true;
    }
    
    protected virtual bool IsDeletingConversationEnabled()
    {
        if (ChatSettings.DeletingMessages != ChatDeletingMessages.Enabled)
        {
            return false;
        }
        
        if (ChatSettings.DeletingConversations == ChatDeletingConversations.Disabled)
        {
            return false;
        }
        
        return true;
    }
    
    protected virtual async Task DeleteMessageAsync(ChatMessageDto message)
    {
        try
        {
            await ConversationAppService.DeleteMessageAsync(new DeleteMessageInput
            {
                MessageId = message.Id,
                TargetUserId = CurrentChatContact.UserId
            });
            
            ChatConversationDto.Messages.Remove(message);
            var lastMessage = ChatConversationDto.Messages.LastOrDefault();
            CurrentChatContact.LastMessage = lastMessage?.Message;
            CurrentChatContact.LastMessageDate = lastMessage?.MessageDate;
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }
    
    protected virtual async Task DeleteConversationAsync()
    {
        try
        {
            await ConversationAppService.DeleteConversationAsync(new DeleteConversationInput
            {
                TargetUserId = CurrentChatContact.UserId
            });

            ChatConversationDto = null;
            await GetContactsAsync();
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    private async Task SetActiveAsync(ChatContactDto contactDto)
    {
        await GetChatSettingsAsync();
        CurrentChatContact = contactDto;
        ChatMessagesContainerStyle = !CurrentChatContact.HasChatPermission ? "pointer-events: none; opacity: 0.5;" : "";
        ChatContactsActive[contactDto] = "active";
        foreach (var dto in ChatContactsActive.Where(x => x.Key != contactDto))
        {
            ChatContactsActive[dto.Key] = "";
        }

        await JsRuntime.InvokeVoidAsync("VoloChatAvatarManager.createCanvasForUser", CurrentChatContactCanvas, CurrentChatContact.Username, GetName(CurrentChatContact));

        ChatConversationDto = await ConversationAppService.GetConversationAsync(new GetConversationInput
        {
            TargetUserId = CurrentChatContact.UserId,
            MaxResultCount = 100
        });

        if (contactDto.UnreadMessageCount > 0)
        {
            await ConversationAppService.MarkConversationAsReadAsync(new MarkConversationAsReadInput
            {
                TargetUserId = contactDto.UserId
            });
        }

        ChatConversationDto.Messages.Reverse();
        var lastMessage = ChatConversationDto.Messages.LastOrDefault();
        CurrentChatContact.LastMessage = lastMessage?.Message;
        CurrentChatContact.LastMessageDate = lastMessage?.MessageDate;

        await InvokeAsync(StateHasChanged);
    }

    private async Task OnSearchChangeAsync(string value)
    {
        SearchValue = value;
        await GetContactsAsync(!SearchValue.IsNullOrEmpty());
    }

    private async Task OnSearchKeyupAsync(KeyboardEventArgs e)
    {
        await InvokeAsync(StateHasChanged);
        await GetContactsAsync(!SearchValue.IsNullOrEmpty());
    }

    private async Task SendMessageAsync()
    {
        await MessageTextArea.FocusAsync();

        if (Message.IsNullOrWhiteSpace() || CurrentChatContact == null)
        {
            return;
        }

        await ConversationAppService.SendMessageAsync(new SendMessageInput
        {
            Message = Message,
            TargetUserId = CurrentChatContact.UserId
        });

        Message = "";

        await SetActiveAsync(CurrentChatContact);
        await InvokeAsync(StateHasChanged);
    }

    private async Task StartConversationAsync()
    {
        await OnSearchChangeAsync(" ");
    }

    private async Task OnMessageEntryAsync(KeyboardEventArgs e)
    {
        if (e.Code == "Enter" && SendOnEnter)
        {
            await SendMessageAsync();
        }
    }
    
    private async Task GetChatSettingsAsync()
    {
        ChatSettings = await SettingsAppService.GetAsync();
    }
}
