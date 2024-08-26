using Umbraco.Cms.Infrastructure.Migrations;
using Microsoft.Extensions.Logging;
using MastodonBot.Umbraco.Migrations.Models;

namespace MastodonBot.Umbraco.Migrations;

public class AddMastodonTable : MigrationBase
{
    public AddMastodonTable(IMigrationContext context) : base(context)
    {
    }

    protected override void Migrate()
    {
        Logger.LogDebug("Running migration {MigrationStep}", "AddMastodonTable");
        if (TableExists("MastodonToots") == false)
        {
            Create.Table<TootStatusDbMigrationModel>().Do();
        }
        else
        {
            Logger.LogDebug("The database table {DbTable} already exists, skipping", "BlogComments");
        }
    }
}