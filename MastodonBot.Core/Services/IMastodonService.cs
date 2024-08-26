namespace MastodonBot.Core.Services;

public interface IMastodonService
{
    Task PostToot(string content);

    Task BoostAndFavoritePostsWithHashtagAsync(string hashtag);
}
