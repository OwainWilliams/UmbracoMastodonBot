using MastodonBot.Mastodon.Services;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.Common.Controllers;

namespace MastodonBot.Mastodon
{
	public class MastodonController : UmbracoApiController
	{
		private readonly IMastodonService _mastodonService;

		public MastodonController(IMastodonService mastodonService)
		{
			_mastodonService = mastodonService;
		}

		public async Task<IActionResult> PostToot([FromBody] TootRequest request)
		{
			await _mastodonService.PostToot(request.Content);
			return Ok();
		}

		public class TootRequest
		{
			public string Content { get; set; }
		}

		public async Task<IActionResult> BoostAndFavorite([FromQuery] string hashtag)
		{
			if (string.IsNullOrEmpty(hashtag))
			{
				return BadRequest("Hashtag is required.");
			}

			await _mastodonService.BoostAndFavoritePostsWithHashtagAsync(hashtag);

			return Ok("Posts with the hashtag have been boosted and favorited.");
		}
	}
}