using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Darts.Domain.Csv
{
   public class CsvHelper<T>
   {
      public static IEnumerable<T> Read(Stream stream)
      {
         using var reader = new StreamReader(stream);

         CsvConfiguration configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
         {
            Delimiter = ",",
            IgnoreBlankLines = true,
            TrimOptions = TrimOptions.Trim,
            Quote = '\0'

         };

         using var csv = new CsvReader(reader, configuration);
         var records = csv.GetRecords<T>();
         return records;
      }
   }
}
