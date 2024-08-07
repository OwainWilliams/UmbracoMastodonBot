using MastodonBot.Mastodon.Models;
using Umbraco.Cms.Core.Scoping;

public class StatusRecordRepository
{
	private readonly IScopeProvider _scopeProvider;

	public StatusRecordRepository(IScopeProvider scopeProvider)
	{
		_scopeProvider = scopeProvider;
	}

	public void AddTootStatusToDb(TootStatusModel tootStatus)
	{
		using var scope = _scopeProvider.CreateScope();
		scope.Database.Insert(tootStatus);
		scope.Complete();
	}

	public IEnumerable<TootStatusModel> TootStatusByStatusId(string statusId)
	{
		using var scope = _scopeProvider.CreateScope();
		var queryResults = scope.Database.Fetch<TootStatusModel>("SELECT * FROM MastodonToots WHERE StatusId = @0", statusId);
		scope.Complete();

		return queryResults;
	}
}