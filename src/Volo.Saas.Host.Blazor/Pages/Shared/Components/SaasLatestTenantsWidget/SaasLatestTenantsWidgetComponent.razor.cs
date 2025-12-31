using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Volo.Abp.Application.Dtos;
using Volo.Saas.Host.Dtos;

namespace Volo.Saas.Host.Blazor.Pages.Shared.Components.SaasLatestTenantsWidget;

public partial class SaasLatestTenantsWidgetComponent
{
    [Inject]
    public ITenantAppService TenantAppService { get; set; }

    protected PagedResultDto<SaasTenantDto> SaasTenants { get; set; } = new();

    protected async override Task OnInitializedAsync()
    {
        SaasTenants = await TenantAppService.GetListAsync(new GetTenantsInput
        {
            GetEditionNames = true,
            MaxResultCount = 6,
            SkipCount = 0,
            Sorting = "CreationTime desc"
        });
    }
}