using Volo.Abp;
using Volo.Abp.MongoDB;
using Volo.FileManagement.Directories;
using Volo.FileManagement.Files;

namespace Volo.FileManagement.MongoDB;

public static class FileManagementMongoDbContextExtensions
{
    public static void ConfigureFileManagement(
        this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<DirectoryDescriptor>(b =>
        {
            b.CollectionName = FileManagementDbProperties.DbTablePrefix + "DirectoryDescriptors";
        });

        builder.Entity<FileDescriptor>(b =>
        {
            b.CollectionName = FileManagementDbProperties.DbTablePrefix + "FileDescriptors";
        });
    }
}
