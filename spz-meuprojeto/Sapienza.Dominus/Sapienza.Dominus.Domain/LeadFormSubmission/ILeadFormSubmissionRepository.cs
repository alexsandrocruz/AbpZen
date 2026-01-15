using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.LeadFormSubmission;

public interface ILeadFormSubmissionRepository : IRepository<Sapienza.Dominus.LeadFormSubmission.LeadFormSubmission, Guid>
{
}
