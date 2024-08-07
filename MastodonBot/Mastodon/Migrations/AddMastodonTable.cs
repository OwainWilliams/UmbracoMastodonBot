using MastodonBot.Mastodon.Models;
using Umbraco.Cms.Infrastructure.Migrations;

namespace MastodonBot.Mastodon.Migrations
{
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
				Create.Table<TootStatusModel>().Do();
			}
			else
			{
				Logger.LogDebug("The database table {DbTable} already exists, skipping", "BlogComments");
			}
		}
	}
}