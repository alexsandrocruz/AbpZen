using System;
using Volo.CmsKit.EntityFrameworkCore;

namespace Volo.CmsKit.Pro;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class CmsKitProApplicationTestBase : CmsKitProTestBase<CmsKitProApplicationTestModule>
{
    protected virtual void UsingDbContext(Action<ICmsKitProDbContext> action)
    {
        using (var dbContext = GetRequiredService<ICmsKitProDbContext>())
        {
            action.Invoke(dbContext);
        }
    }
    protected virtual T UsingDbContext<T>(Func<ICmsKitProDbContext, T> action)
    {
        using (var dbContext = GetRequiredService<ICmsKitProDbContext>())
        {
            return action.Invoke(dbContext);
        }
    }
}
