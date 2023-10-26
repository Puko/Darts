using System;

namespace Darts.Api.Attributes
{
	[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
	public class SwaggerParameterPageAttribute : Attribute
	{
		public SwaggerParameterPageAttribute()
		{
			Name = "page";
            DataType = "int";
            ParameterType = "query";
        }

		public string Name { get; set; }
		public string DataType { get; set; }
		public string ParameterType { get; set; }
		public string Description { get; set; }
		public bool Required { get; set; } = false;
	}
}
