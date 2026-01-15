using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.BlogPostVersion;

public interface IBlogPostVersionRepository : IRepository<Sapienza.Dominus.BlogPostVersion.BlogPostVersion, Guid>
{
}
