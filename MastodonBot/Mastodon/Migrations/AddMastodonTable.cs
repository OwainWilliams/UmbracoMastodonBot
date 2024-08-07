using NPoco;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

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
				Create.Table<MastodonTootsSchema>().Do();
			}
			else
			{
				Logger.LogDebug("The database table {DbTable} already exists, skipping", "BlogComments");
			}
		}

		[TableName("MastodonToots")]
		[PrimaryKey("Id", AutoIncrement = true)]
		[ExplicitColumns]
		public class MastodonTootsSchema
		{
			[PrimaryKeyColumn(AutoIncrement = true, IdentitySeed = 1)]
			[Column("Id")]
			public int Id { get; set; }

			[Column("StatusId")]
			public string StatusId { get; set; }

			[Column("Boosted")]
			public bool Boosted { get; set; }

			[Column("Favorited")]
			public bool Favorited { get; set; }
		}
	}
}