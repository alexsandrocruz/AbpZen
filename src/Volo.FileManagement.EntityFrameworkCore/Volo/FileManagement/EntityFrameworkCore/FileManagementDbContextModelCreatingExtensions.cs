using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.FileManagement.Directories;
using Volo.FileManagement.Files;

namespace Volo.FileManagement.EntityFrameworkCore;

public static class FileManagementDbContextModelCreatingExtensions
{
    public static void ConfigureFileManagement(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<DirectoryDescriptor>(b =>
        {
            b.ToTable(FileManagementDbProperties.DbTablePrefix + "DirectoryDescriptors", FileManagementDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.Property(p => p.Name).IsRequired().HasMaxLength(DirectoryDescriptorConsts.MaxNameLength);

            b.HasMany<DirectoryDescriptor>().WithOne().HasForeignKey(p => p.ParentId);
            b.HasOne<DirectoryDescriptor>().WithMany().HasForeignKey(p => p.ParentId);

            b.HasMany<FileDescriptor>().WithOne().HasForeignKey(p => p.DirectoryId);

            b.HasIndex(x => new { x.TenantId, x.ParentId, x.Name });

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<FileDescriptor>(b =>
        {
            b.ToTable(FileManagementDbProperties.DbTablePrefix + "FileDescriptors", FileManagementDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.Property(p => p.Name).IsRequired().HasMaxLength(FileDescriptorConsts.MaxNameLength);
            b.Property(p => p.MimeType).IsRequired().HasMaxLength(FileDescriptorConsts.MaxMimeTypeLength);
            b.Property(p => p.Size).IsRequired();

            b.HasOne<DirectoryDescriptor>().WithMany().HasForeignKey(p => p.DirectoryId);

            b.HasIndex(x => new { x.TenantId, x.DirectoryId, x.Name });

            b.ApplyObjectExtensionMappings();
        });

        builder.TryConfigureObjectExtensions<FileManagementDbContext>();
    }
}
