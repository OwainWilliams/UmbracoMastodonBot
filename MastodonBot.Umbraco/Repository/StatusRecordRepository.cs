using MastodonBot.Core.Models;
using MastodonBot.Core.Repositories;
using MastodonBot.Umbraco.Models;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.Extensions;

namespace MastodonBot.Umbraco.Repository;

internal class StatusRecordRepository : IStatusRecordRepository
{
    private readonly ILogger<StatusRecordRepository> _logger;
    private readonly IScopeProvider _scopeProvider;

    public StatusRecordRepository(ILogger<StatusRecordRepository> logger, IScopeProvider scopeProvider)
    {
        _logger = logger;
        _scopeProvider = scopeProvider;
    }
    public bool AddTootStatusToDb(TootStatusDtoModel tootStatus)
    {
        return AddTootStatusToDbAsync(tootStatus).Result;
    }

    public async Task<bool> AddTootStatusToDbAsync(TootStatusDtoModel tootStatus)
    {
        try
        {
            using var scope = _scopeProvider.CreateScope();
            var dbModel = new TootStatusDbModel
            {
                StatusId = tootStatus.StatusId,
                Boosted = tootStatus.Boosted,
                Favorited = tootStatus.Favorited
            };
            await scope.Database.InsertAsync(dbModel);
            scope.Complete();

            tootStatus.Id = dbModel.Id;
        }
        catch (Exception ex)
        {

            throw;
        }

        return true;
    }

    public IEnumerable<TootStatusDtoModel> TootStatusByStatusId(string statusId)
    {
        return TootStatusByStatusIdAsync(statusId).Result;
    }

    public async Task<IEnumerable<TootStatusDtoModel>> TootStatusByStatusIdAsync(string statusId)
    {
        using var scope = _scopeProvider.CreateScope();
        var sql = scope.SqlContext.Sql().Select<TootStatusDbModel>()
            .From<TootStatusDbModel>()
            .Where<TootStatusDbModel>(m =>  m.StatusId == statusId);
        var queryResults = await scope.Database.FetchAsync<TootStatusDbModel>(sql);
        scope.Complete();


        return queryResults.Select(r => 
        new TootStatusDtoModel
        {
            Id = r.Id,
            StatusId = r.StatusId,
            Boosted = r.Boosted,
            Favorited = r.Favorited 
        }).ToList();
    }
}
