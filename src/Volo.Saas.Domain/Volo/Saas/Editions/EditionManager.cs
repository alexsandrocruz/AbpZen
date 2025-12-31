using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Saas.Tenants;

namespace Volo.Saas.Editions;

public class EditionManager : DomainService
{
    protected IEditionRepository EditionRepository { get; }
    protected ITenantRepository TenantRepository { get; }

    public EditionManager(IEditionRepository editionRepository, ITenantRepository tenantRepository)
    {
        EditionRepository = editionRepository;
        TenantRepository = tenantRepository;
    }

    public virtual async Task<Edition> CreateAsync(string displayName)
    {
        Check.NotNull(displayName, nameof(displayName));

        await ValidateDisplayNameAsync(displayName);
        return new Edition(GuidGenerator.Create(), displayName);
    }

    public virtual async Task ChangeDisplayNameAsync(Edition edition, string displayName)
    {
        Check.NotNull(edition, nameof(edition));
        Check.NotNull(displayName, nameof(displayName));

        await ValidateDisplayNameAsync(displayName, edition.Id);
        edition.SetDisplayName(displayName);
    }

    protected virtual async Task ValidateDisplayNameAsync(string displayName, Guid? expectedId = null)
    {
        var edition = await EditionRepository.FindByDisplayNameAsync(displayName);
        if (edition != null && edition.Id != expectedId)
        {
            throw new BusinessException("Volo.Saas:DuplicateEditionDisplayName").WithData("Name", displayName);
        }
    }

    public virtual async Task<Edition> GetEditionForSubscriptionAsync(Guid id)
    {
        var edition = await EditionRepository.GetAsync(id);

        await CheckEditionForSubscriptionAsync(edition);

        return edition;
    }

    public virtual Task CheckEditionForSubscriptionAsync(Edition edition)
    {
        if (!edition.PlanId.HasValue)
        {
            throw new EditionDoesntHavePlanException(edition.Id);
        }
        return Task.CompletedTask;
    }

    public virtual async Task DeleteAsync(Edition edition, Guid? assignToEditionId = null)
    {
        await TenantRepository.UpdateEditionsAsync(edition.Id, assignToEditionId);
        await EditionRepository.DeleteAsync(edition);
    }

    public virtual async Task MoveAllTenantsAsync(Guid id, Guid? targetEditionId = null)
    {
        await TenantRepository.UpdateEditionsAsync(id, targetEditionId);
    }
}
