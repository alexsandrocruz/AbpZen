using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR.Client;
using Volo.Abp.DependencyInjection;
using Volo.Chat.Blazor.Components;

namespace Volo.Chat.Blazor.Server.Components;

[ExposeServices(typeof(MessagesToolbarItem))]
public class BlazorServerMessagesToolbarItem : MessagesToolbarItem
{
    [Inject]
    protected IHttpContextAccessor HttpContextAccessor { get; set; }

    protected override Task SetChatHubConnectionAsync()
    {
        var cookies = new CookieContainer();

        if (HttpContextAccessor.HttpContext != null)
        {
            foreach (var cookie in HttpContextAccessor.HttpContext.Request.Cookies)
            {
                if (!cookie.Value.IsNullOrEmpty())
                {
                    cookies.Add(new Cookie(cookie.Key, WebUtility.UrlEncode(cookie.Value), null, HttpContextAccessor.HttpContext.Request.Host.Host));    
                }
            }
        }

        HubConnection = new HubConnectionBuilder()
            .WithUrl(Navigation.ToAbsoluteUri("/signalr-hubs/chat"), options =>
            {
                options.Cookies = cookies;
            })
            .Build();

        return Task.CompletedTask;
    }
}
