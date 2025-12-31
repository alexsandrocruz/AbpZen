using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.Gdpr;

[ConnectionStringName(GdprDbProperties.ConnectionStringName)]
public interface IGdprDbContext : IEfCoreDbContext
{
    DbSet<GdprRequest> Requests { get; }
}