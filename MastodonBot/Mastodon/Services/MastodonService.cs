using MastodonBot.Mastodon.Models;
using Mastonet;
using Mastonet.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MastodonBot.Mastodon.Services
{
	public interface IMastodonService
	{
		Task PostToot(string content);

		Task BoostAndFavoritePostsWithHashtagAsync(string hashtag);
	}

	public class MastodonService : IMastodonService
	{
		private readonly MastodonClient _client;
		private readonly StatusRecordRepository _statusRecordRepository;
		private readonly IConfiguration _configuration;

		public MastodonService(StatusRecordRepository statusRecordRepository, IConfiguration configuration)
		{
			_configuration = configuration;
			_statusRecordRepository = statusRecordRepository;

			var auth = new AppRegistration
			{
				Instance = _configuration["Mastodon:Api:Instance"],
				ClientId = _configuration["Mastodon:Api:ClientId"],
				ClientSecret = _configuration["Mastodon:Api:ClientSecret"],
			};

			_client = new MastodonClient(auth.Instance, _configuration["Mastodon:Api:AccessToken"]);
		}

		public async Task PostToot(string content)
		{
			await _client.PublishStatus(content);
		}

		[HttpGet]
		public async Task BoostAndFavoritePostsWithHashtagAsync(string hashtag = "h5yr")
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
				var existingRecord = _statusRecordRepository.TootStatusByStatusId(status.Id);
				if (existingRecord.Any() == false)
				{
					// Boost (reblog) the status
					await _client.Reblog(status.Id);

					// Favorite the status
					await _client.Favourite(status.Id);

					_statusRecordRepository.AddTootStatusToDb(new TootStatusModel
					{
						StatusId = status.Id,
						Boosted = true,
						Favorited = true
					});
				}
			}
		}
	}
}