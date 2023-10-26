using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Darts.Api.Binders
{
   public class DataInclusionModelBinder : IModelBinder
   {
      public const string IncludeParameterName = "include";

      public Task BindModelAsync(ModelBindingContext bindingContext)
      {
         ValueProviderResult includeValue = bindingContext.ValueProvider.GetValue(IncludeParameterName);
         if (!includeValue.Values.Any())
         {
            bindingContext.Model = Activator.CreateInstance(bindingContext.ModelType);
         }
         else
         {
            IEnumerable<PropertyInfo> inclusionProperties = bindingContext.ModelType
               .GetProperties()
               .Where(x => x.CanWrite && x.PropertyType == typeof(bool));

            string[] rawDataToInclude = includeValue.Values[0].Split(',');

            bindingContext.Model = Activator.CreateInstance(bindingContext.ModelType);

            foreach (PropertyInfo inclusionProperty in inclusionProperties)
            {
               if (rawDataToInclude.Any(x => string.Equals(x, inclusionProperty.Name, StringComparison.InvariantCultureIgnoreCase)))
               {
                  inclusionProperty.SetValue(bindingContext.Model, true);
               }
            }

            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
         }


         return Task.CompletedTask;
      }
   }
}