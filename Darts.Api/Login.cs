using System.ComponentModel.DataAnnotations;

namespace Darts.Api
{
   public class Login
   {
      [Required]
      public string Email { get; set; }
      [Required]
      public string Password { get; set; }
   }
}
