using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.TaskComment;

public interface ITaskCommentRepository : IRepository<Sapienza.Dominus.TaskComment.TaskComment, Guid>
{
}
