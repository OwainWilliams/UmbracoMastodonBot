namespace MastodonBot.Core.Config;

public class MastodonBotConfig
{
    public const string Key = "Mastodon:Api";

    public string Instance { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
}

