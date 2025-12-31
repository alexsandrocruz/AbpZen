using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.Gdpr;

[ConnectionStringName(GdprDbProperties.ConnectionStringName)]
public class GdprDbContext : AbpDbContext<GdprDbContext>, IGdprDbContext
{
    public DbSet<GdprRequest> Requests { get; set; }
    
    public GdprDbContext(DbContextOptions<GdprDbContext> options)
        : base(options)
    {

    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.ConfigureGdpr();
    }
}