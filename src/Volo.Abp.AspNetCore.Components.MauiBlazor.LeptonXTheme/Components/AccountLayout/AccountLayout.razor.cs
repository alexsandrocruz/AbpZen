using System;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AspNetCore.Components.MauiBlazor.LeptonXTheme.Components.AccountLayout;

public partial class AccountLayout
{
    protected Modal SwitchTenantModal;
    
    protected string TenantName { get; set; }
    
    [Inject]
    public ITenantStore TenantStore { get; set; }
    
    [Inject]
    public IUiMessageService UiMessageService { get; set; }
    
    [Inject]
    public ICurrentTenantAccessor CurrentTenantAccessor { get; set; }

    protected override void OnInitialized()
    {
        TenantName = CurrentTenant.Name;
    }

    protected virtual async Task SwitchTenantAsync()
    {
        TenantConfiguration tenant = null;
        if (!TenantName.IsNullOrWhiteSpace())
        {
            tenant = await TenantStore.FindAsync(TenantName);
            if (tenant == null)
            {
                await UiMessageService.Error(L["GivenTenantIsNotExist", TenantName]);
                return;
            }
        
            if (!tenant.IsActive)
            {
                await UiMessageService.Error(L["GivenTenantIsNotAvailable", TenantName]);
                return;
            }
        }
        
        CurrentTenantAccessor.Current = new BasicTenantInfo(
            tenant?.Id,
            tenant?.Name);
        
        await CloseSwitchTenantModalAsync();
        await InvokeAsync(StateHasChanged);
    }
    
    protected virtual async Task OpenSwitchTenantModalAsync()
    {
        await SwitchTenantModal.Show();
    }

    protected virtual async Task CloseSwitchTenantModalAsync()
    {
        await SwitchTenantModal.Hide();
    }
}