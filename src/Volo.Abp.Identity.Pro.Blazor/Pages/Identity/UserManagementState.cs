using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Identity.Pro.Blazor.Pages.Identity;

public class UserManagementState : IScopedDependency
{
    public Func<GetIdentityUsersInput> OnGetFilter { get; set; }
    public Func<Task> OnDataGridChanged { get; set; }

    public GetIdentityUsersInput GetFilter()
    {
        return OnGetFilter.Invoke();
    }
    
    public async Task DataGridChangedAsync()
    {
        await OnDataGridChanged.Invoke();
    }
}