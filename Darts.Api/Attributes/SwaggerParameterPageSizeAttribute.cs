using System;

namespace Darts.Api.Attributes
{
	[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
	public class SwaggerParameterPageSizeAttribute : SwaggerParameterPageAttribute
    {
		public SwaggerParameterPageSizeAttribute()
		{
			Name = "pagesize";
        }
	}
}
