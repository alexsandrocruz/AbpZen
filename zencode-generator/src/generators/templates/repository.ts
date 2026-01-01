/**
 * Repository interface template for ABP Domain layer
 */
export function getRepositoryInterfaceTemplate(): string {
    return `using System;
using Volo.Abp.Domain.Repositories;

namespace {{ project.namespace }}.{{ entity.name }};

public interface I{{ entity.name }}Repository : IRepository<{{ entity.name }}, {{ entity.primaryKey }}>
{
}
`;
}

/**
 * Repository implementation template for EF Core
 */
export function getRepositoryImplementationTemplate(): string {
    return `using System;
using {{ project.namespace }}.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace {{ project.namespace }}.{{ entity.name }};

public class Ef{{ entity.name }}Repository 
    : EfCoreRepository<{{ project.name }}DbContext, {{ entity.name }}, {{ entity.primaryKey }}>, 
      I{{ entity.name }}Repository
{
    public Ef{{ entity.name }}Repository(IDbContextProvider<{{ project.name }}DbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
`;
}
