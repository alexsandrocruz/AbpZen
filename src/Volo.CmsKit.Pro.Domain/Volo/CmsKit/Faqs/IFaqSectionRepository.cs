using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Domain.Repositories;

namespace Volo.CmsKit.Faqs;

public interface IFaqSectionRepository : IBasicRepository<FaqSection, Guid>
{
    Task<List<FaqSectionWithQuestionCount>> GetListAsync(
        [CanBeNull] string filter = null,
        [CanBeNull] string sorting = null,
        int skipCount = 0,
        int maxResultCount = int.MaxValue,
        CancellationToken cancellationToken = default);

    Task<long> GetCountAsync(
        [CanBeNull] string filter = null,
        CancellationToken cancellationToken = default);

    Task<List<FaqSectionWithQuestions>> GetListSectionWithQuestionAsync(
        [CanBeNull] string groupName = null,
        [CanBeNull] string sectionName = null,
        CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(
        Guid id,
        CancellationToken cancellationToken = default);
    
    Task<bool> AnyAsync(
        [NotNull] string groupName,
        [NotNull] string name,
        CancellationToken cancellationToken = default);
}