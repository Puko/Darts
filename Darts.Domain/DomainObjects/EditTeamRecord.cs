using System;
using System.ComponentModel.DataAnnotations;

namespace Darts.Domain.DomainObjects
{
    public class EditTeamRecord
    {
        [Key]
        public int Id { get; set; }
        public long TeamId { get; set; }
        public Team Team { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public string Diff { get; set; }
        public DateTimeOffset Date { get; set; } = DateTimeOffset.UtcNow;
    }
}
