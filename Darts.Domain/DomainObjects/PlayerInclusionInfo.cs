namespace Darts.Domain.DomainObjects
{
    public class PlayerInclusionInfo
    {
        public bool User { get; set; }
        public bool Stats { get; set; }
        public bool PremierLeagueMatches { get; set; }

        public static PlayerInclusionInfo Empty => new PlayerInclusionInfo();
    }
}
