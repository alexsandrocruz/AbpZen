using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.AiChatSession;

public interface IAiChatSessionRepository : IRepository<Sapienza.Dominus.AiChatSession.AiChatSession, Guid>
{
}
