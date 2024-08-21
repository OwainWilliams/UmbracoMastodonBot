using MastodonBot.Core.Models;

namespace MastodonBot.Core.Repositories;

public interface IStatusRecordRepository
{
    bool AddTootStatusToDb(TootStatusDtoModel tootStatus);
    Task<bool> AddTootStatusToDbAsync(TootStatusDtoModel tootStatus);

    IEnumerable<TootStatusDtoModel> TootStatusByStatusId(string statusId);

    Task<IEnumerable<TootStatusDtoModel>> TootStatusByStatusIdAsync(string statusId);
}
