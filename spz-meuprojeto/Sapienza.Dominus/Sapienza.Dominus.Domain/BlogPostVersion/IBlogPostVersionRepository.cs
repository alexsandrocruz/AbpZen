using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.BlogPostVersion;

public interface IBlogPostVersionRepository : IRepository<Dominus.BlogPostVersion.BlogPostVersion, Guid>
{
}
