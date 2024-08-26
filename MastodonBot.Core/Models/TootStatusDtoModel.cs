namespace MastodonBot.Core.Models
{
    public class TootStatusDtoModel
    {
        public int Id { get; set; }
        public required string StatusId { get; set; }
        public bool Boosted { get; set; }
        public bool Favorited { get; set; }
    }
}
