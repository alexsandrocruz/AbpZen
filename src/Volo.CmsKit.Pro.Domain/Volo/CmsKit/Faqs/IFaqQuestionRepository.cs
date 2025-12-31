using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using JetBrains.Annotations;
using Volo.Abp.Domain.Repositories;

namespace Volo.CmsKit.Faqs;

public interface IFaqQuestionRepository : IBasicRepository<FaqQuestion, Guid>
{
    Task<List<FaqQuestion>> GetListAsync(
        Guid sectionId,
        [CanBeNull] string filter = null,
        [CanBeNull] string sorting = null,
        int skipCount = 0,
        int maxResultCount = int.MaxValue,
        CancellationToken cancellationToken = default);

    Task<long> GetCountAsync(
        Guid sectionId,
        [CanBeNull] string filter = null,
        CancellationToken cancellationToken = default);
    
    Task<bool> AnyAsync(
        Guid sectionId,
        [NotNull] string title,
        CancellationToken cancellationToken = default);
}
