using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.CmsKit.PageFeedbacks;

public interface IPageFeedbackRepository : IBasicRepository<PageFeedback, Guid>
{
    Task<List<PageFeedback>> GetListAsync(
        string entityType = null,
        string entityId = null,
        bool? isUseful = null,
        string url = null,
        bool? isHandled = null,
        string sorting = null,
        int skipCount = 0,
        int maxResultCount = int.MaxValue,
        bool? hasUserNote = null,
        bool? hasAdminNote = null,
        CancellationToken cancellationToken = default);
    
    Task<long> GetCountAsync(
        string entityType = null,
        string entityId = null,
        bool? isUseful = null,
        string url = null,
        bool? isHandled = null,
        bool? hasUserNote = null,
        bool? hasAdminNote = null,
        CancellationToken cancellationToken = default);
}