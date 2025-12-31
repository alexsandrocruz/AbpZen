using Volo.Abp;
using Volo.Abp.MongoDB;

namespace Volo.Chat.MongoDB;

public static class ChatMongoDbContextExtensions
{
    public static void ConfigureChat(
        this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));
    }
}
