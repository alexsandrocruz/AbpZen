using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.CmsKit.Pro.Admin.Blazor.Pages.CmsKit;

public class NewslettersManagementState: IScopedDependency
{
    public Func<Task> OnDataGridChanged { get; set; }
    
    public async Task DataGridChangedAsync()
    {
        await OnDataGridChanged.Invoke();
    }
}