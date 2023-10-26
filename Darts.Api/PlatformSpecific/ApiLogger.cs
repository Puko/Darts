using Development.Support.Services;
using System.Reflection;

namespace Darts.Api.PlatformSpecific
{
   public class ApiLogger : LogService
   {
      private string _version;

      public ApiLogger(string url) : base(url)
      {
          
      }

      protected override string AppName => "Api";
      protected override string PackageName => "Darts.Api";
      protected override string Version
      {
         get
         {
            if (_version == null)
               _version = Assembly.GetExecutingAssembly().
                           GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

            return _version;
         }
      }
      protected override string Platform => "Api";
      protected override string Manufacturer => "Api";
      protected override string Model => "Api";

      protected override string LogToken => "yvKX9W1adRJ1QK6aJGXM";
    }
}
