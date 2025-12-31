using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Gdpr;

public class GdprTestData : ISingletonDependency
{
    public Guid RequestId1 { get; } = Guid.NewGuid();
    public Guid UserId1 { get; } = Guid.NewGuid();
}