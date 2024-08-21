using MastodonBot.Core.Services;
using MastodonBot.Umbraco.Models;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Extensions;

namespace MastodonBot.Umbraco.Controllers;

public class MastodonController : UmbracoAuthorizedApiController
{
    private readonly IMastodonService _mastodonService;

    public MastodonController(IMastodonService mastodonService)
    {
        _mastodonService = mastodonService;
    }

    [Route("umbraco/backoffice/posttoot/")]
    public async Task<IActionResult> PostToot([FromBody] TootRequest request)
    {
        if (!request.Content.IsNullOrWhiteSpace())
        {
            await _mastodonService.PostToot(request.Content);
            return Ok();
        }

        return BadRequest("Nothing to post");
    }

    

    [Route("umbraco/backoffice/boost/hashtag/{hashtag}")]
    public async Task<IActionResult> BoostAndFavorite(string hashtag)
    {
        if (string.IsNullOrEmpty(hashtag))
        {
            return BadRequest("Hashtag is required.");
        }

        await _mastodonService.BoostAndFavoritePostsWithHashtagAsync(hashtag);

        return Ok("Posts with the hashtag have been boosted and favorited.");
    }
}
