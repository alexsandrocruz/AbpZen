using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Client.Authentication;
using Volo.Chat.Blazor.Components;

namespace Volo.Chat.Blazor.MauiBlazor.Components;

[ExposeServices(typeof(MessagesToolbarItem))]
public class BlazorMessagesToolbarItem : MessagesToolbarItem
{
    [Inject]
    protected IAbpAccessTokenProvider AccessTokenProvider { get; set; }

    [Inject]
    protected IOptions<ChatBlazorMauiBlazorOptions> ChatBlazorMauiBlazorOptions { get; set; }
    
    [Inject]
    protected IOptions<AbpRemoteServiceOptions> AbpRemoteServiceOptions { get; set; }

    protected async override Task SetChatHubConnectionAsync()
    {
        var accessToken = await AccessTokenProvider.GetTokenAsync();

        var signalrUrl = ChatBlazorMauiBlazorOptions.Value.SignalrUrl ?? AbpRemoteServiceOptions.Value.RemoteServices.Default.BaseUrl;
        
        HubConnection = new HubConnectionBuilder()
            .WithUrl(signalrUrl.EnsureEndsWith('/') + "signalr-hubs/chat", options =>
            {
                options.AccessTokenProvider = () => Task.FromResult(accessToken);
            })
            .Build();
    }
}
