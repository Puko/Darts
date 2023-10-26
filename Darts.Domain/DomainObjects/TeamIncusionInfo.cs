namespace Darts.Domain.DomainObjects
{
    public class TeamIncusionInfo
    {
        public bool Matches { get; set; }
        public bool Players { get; set; }

        public static TeamIncusionInfo Empty = new TeamIncusionInfo();
    }
}
