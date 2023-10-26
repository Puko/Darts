using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace Darts.Api.Attributes
{
    public class SwaggerParameterAttributeFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var attributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                .Union(context.MethodInfo.GetCustomAttributes(true))
                .OfType<SwaggerParameterPageAttribute>();

            foreach (var attribute in attributes)
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = attribute.Name,
                    Description = attribute.Description,
                    In = ParameterLocation.Header,
                    Required = attribute.Required
                });
        }
    }
}
