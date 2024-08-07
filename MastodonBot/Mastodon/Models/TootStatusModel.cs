using NPoco;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace MastodonBot.Mastodon.Models
{
	[TableName("MastodonToots")]
	[PrimaryKey("Id", AutoIncrement = true)]
	[ExplicitColumns]
	public class TootStatusModel
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