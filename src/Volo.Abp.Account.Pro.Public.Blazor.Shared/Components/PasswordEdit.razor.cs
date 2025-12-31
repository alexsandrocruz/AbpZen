using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Volo.Abp.Account.Localization;

namespace Volo.Abp.Account.Pro.Public.Blazor.Shared.Components;

public partial class PasswordEdit : ComponentBase
{
    
    [Inject]
    protected IStringLocalizer<AccountResource> L { get; set; }
    
    [Parameter]
    public string Password { get; set; }
    
    [Parameter]
    public EventCallback<string> PasswordChanged { get; set; }
    
    [Parameter]
    public bool Visible { get; set; } = true;
    
    [Parameter]
    public bool ShowPasswordIcon { get; set; }
    
    [Parameter]
    public bool ShowPasswordStrength { get; set; }

    [Parameter]
    public string[] Colors { get; set; }
    
    [Parameter]
    public string[] Texts { get; set; }
    
    protected TextRole TextRole = TextRole.Password;

    protected string Progress { get; set; } = "0%";
    
    protected string ProgressColor { get; set; }
    
    protected string ProgressText { get; set; }
    
    protected bool ShowPassword { get; set; }
    
    protected override void OnInitialized()
    {
        Colors = new[] {
            "#B0284B",
            "#F2A34F",
            "#5588A4",
            "#3E5CF6",
            "#6EBD70"
        };
       
        Texts = new [] {
            L["Weak"].Value,
            L["Fair"].Value,
            L["Normal"].Value,
            L["Good"].Value,
            L["Strong"].Value
        };
    }

    protected virtual void ChangePasswordTextRole()
    {
        ShowPassword = !ShowPassword;
        TextRole = ShowPassword ? TextRole.Text : TextRole.Password;
    }
    
    protected virtual Task OnTextChanged(string value )
    {
        Password = value;
        PasswordChanged.InvokeAsync(value);

        if (ShowPasswordStrength)
        {
            CheckPasswordStrength(value);
        }
        
        return Task.CompletedTask;
    }
    
    protected virtual void CheckPasswordStrength(string value)
    {
        if (value.IsNullOrWhiteSpace())
        {
            return;
        }

        var result = Zxcvbn.Zxcvbn.MatchPassword(value);
        var progress = result.Score;
        
        Progress = $"{(progress + 1) * 20}%";
        ProgressColor = Colors[progress];
        ProgressText = Texts[progress];
    }
}