using System;
using System.ComponentModel.DataAnnotations;

namespace Darts.Domain.DomainObjects
{
    public class EditPlayerRecord
    {
        [Key]
        public int Id { get; set; }
        public long PlayerId { get; set; }
        public Player Player { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public string Diff { get; set; }
        public DateTimeOffset Date { get; set; } = DateTimeOffset.UtcNow;
    }
}
