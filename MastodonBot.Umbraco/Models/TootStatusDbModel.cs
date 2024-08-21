using NPoco;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace MastodonBot.Umbraco.Models;

[TableName("MastodonToots")]
[PrimaryKey("Id", AutoIncrement = true)]
[ExplicitColumns]
public class TootStatusDbModel
{
    [PrimaryKeyColumn(AutoIncrement = true, IdentitySeed = 1)]
    [Column("Id")]
    public int Id { get; set; }

    [Column("StatusId")]
    public required string StatusId { get; set; }

    [Column("Boosted")]
    public bool Boosted { get; set; }

    [Column("Favorited")]
    public bool Favorited { get; set; }
}
