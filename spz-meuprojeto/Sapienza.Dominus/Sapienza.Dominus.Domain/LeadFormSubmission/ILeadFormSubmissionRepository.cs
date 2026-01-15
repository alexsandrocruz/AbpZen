using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.LeadFormSubmission;

public interface ILeadFormSubmissionRepository : IRepository<Dominus.LeadFormSubmission.LeadFormSubmission, Guid>
{
}
