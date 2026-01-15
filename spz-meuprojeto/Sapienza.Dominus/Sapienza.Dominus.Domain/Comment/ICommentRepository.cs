using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.Comment;

public interface ICommentRepository : IRepository<Dominus.Comment.Comment, Guid>
{
}
