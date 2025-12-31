# Chat

This module implements real time messaging between users for an application.

## Enabling the Module

By default, chat feature is disabled. To enable the Chatting in the application, go to `feature management` tab in the `settings` page in your application, and enable the `Chat` feature.

## Permissions

After the module installation, the following permissions are added to the system:

- **Chat**
  - **Chat.Messaging**
  - **Chat.Searching**
  - **Chat.SettingManagement**

You may need to give these permissions to the roles that you want to allow to access the Chat UI.

## Configuration for Blazor WebAssembly and Angular 

If you are using Blazor WebAssembly or Angular, you need to add the following configuration to the module class of the `.Host` project`:

```csharp
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
    
        //....
        //....
    
        // the configuration to add
        app.Use(async (httpContext, next) =>
        {
            var accessToken = httpContext.Request.Query["access_token"];

            var path = httpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) &&
                (path.StartsWithSegments("/signalr-hubs/chat")))
            {
                httpContext.Request.Headers["Authorization"] = "Bearer " + accessToken;
            }

            await next();
        });
        
        //....
        //....
    }
```

This is required to pass the access token to the SignalR hub.

## Documentation

For more information, see the [module documentation](https://abp.io/docs/latest/modules/chat).
