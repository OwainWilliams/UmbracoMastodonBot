using MastodonBot.Core.Config;
using MastodonBot.Core.Models;
using MastodonBot.Core.Repositories;
using Mastonet;
using Mastonet.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MastodonBot.Core.Services;

public class MastodonService : IMastodonService
{
    private readonly MastodonClient _client;
    private readonly IStatusRecordRepository _statusRecordRepository;
    private readonly MastodonBotConfig _configuration;

    public MastodonService(ILogger<IMastodonService> logger, IStatusRecordRepository statusRecordRepository, IOptions<MastodonBotConfig> configuration)
    {
        _configuration = configuration.Value;
        _statusRecordRepository = statusRecordRepository;

        var auth = new AppRegistration
        {
            Instance = _configuration.Instance,
            ClientId = _configuration.ClientId,
            ClientSecret = _configuration.ClientSecret,
        };

        _client = new MastodonClient(auth.Instance, _configuration.AccessToken);
    }

    public async Task PostToot(string content)
    {
        await _client.PublishStatus(content);
    }

    public async Task BoostAndFavoritePostsWithHashtagAsync(string hashtag)
    {
        var allStatuses = new List<Status>();

        MastodonList<Status> results = await _client.GetTagTimeline(hashtag);

        allStatuses.AddRange(results);

        // Handle pagination using max_id if needed
        while (allStatuses.Count < 20 && results.Any())
        {
            var maxId = results.Last().Id;
            var sinceId = results.First().Id;
            results = await _client.GetTagTimeline(hashtag, new ArrayOptions { Limit = 20, MaxId = maxId, SinceId = sinceId });
            if (results == null || !results.Any()) break;
            allStatuses.AddRange(results);
        }
        allStatuses = allStatuses.Take(20).ToList();

        foreach (var status in allStatuses)
        {
            // Check if the status has already been processed
            var existingRecord = await _statusRecordRepository.TootStatusByStatusIdAsync(status.Id);
            if (existingRecord.Any() == false)
            {
                // Boost (reblog) the status
                await _client.Reblog(status.Id);

                // Favorite the status
                await _client.Favourite(status.Id);

                // Toot a thank you
                var replyContent = $"@{status.Account.AccountName} Thank you for using the {hashtag} hashtag! Check out https://h5yr.com";

                await _client.PublishStatus(replyContent, replyStatusId: status.Id);

                await _statusRecordRepository.AddTootStatusToDbAsync(new TootStatusDtoModel
                {
                    StatusId = status.Id,
                    Boosted = true,
                    Favorited = true
                });
            }
        }
    }
}
