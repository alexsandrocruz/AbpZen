using System;
using Volo.Abp.Domain.Repositories;

namespace LeptonXDemoApp.MessageTemplate;

public interface IMessageTemplateRepository : IRepository<LeptonXDemoApp.MessageTemplate.MessageTemplate, Guid>
{
}
