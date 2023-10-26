using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Darts.Domain.DomainObjects
{
   public class User
   {
      public long Id { get; set; }
      [Required]
      public DateTime Created { get; set; }
      [Required]
      public string FirstName { get; set; }
      [Required]
      public string LastName { get; set; }
      [Required]
      public string Email { get; set; }
      public string Mobile { get; set; }
      [Required]
      public string Password { get; set; }
      public bool IsActive { get; set; }
      public UserRole UserRole { get; set; }
      public virtual ICollection<League> League { get; set; } = new List<League>();
      public virtual ICollection<EditTeamRecord> EditTeamRecords { get; set; } = new List<EditTeamRecord>();
      public virtual ICollection<EditPlayerRecord> EditPlayerRecords { get; set; } = new List<EditPlayerRecord>();

      public override bool Equals(object obj)
      {
         return obj is User user &&
                Id == user.Id;
      }

      public override int GetHashCode()
      {
         return HashCode.Combine(Id);
      }
   }
}
