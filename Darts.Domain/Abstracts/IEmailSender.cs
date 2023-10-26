using System.Net;
using System.Threading.Tasks;

namespace Darts.Domain.Abstracts
{
   public interface IEmailService
   {
      public Task<HttpStatusCode> SendEmailAsync(string from, string to, string subject, string message);
   }
}
