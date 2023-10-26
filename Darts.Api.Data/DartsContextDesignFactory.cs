using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Darts.Api.Data
{
   public class DartsContextDesignFactory : IDesignTimeDbContextFactory<DartsContext>
   {
      public DartsContext CreateDbContext(string[] args)
      {
         var builder = new DbContextOptionsBuilder<DartsContext>();
         var connStr = "Server=194.60.87.105;Database=DartsTest;User Id=Admin;Password=123DartsTest**";
         builder.UseSqlServer(connStr);
         return new DartsContext(builder.Options);
      }
   }
}
