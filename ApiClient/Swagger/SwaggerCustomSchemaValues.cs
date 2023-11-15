using ApiClient.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace ApiClient.Swagger
{
    public class SwaggerCustomSchemaValues : ISchemaFilter
    {
        /// <summary>
        /// Apply is called for each parameter
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.ParameterInfo?.Member != null)
            {
                var att = context.ParameterInfo.Member.GetCustomAttribute<SwaggerISINExamplesAttribute>();
                if (att != null)
                {
                    schema.Example = att.Value;
                }
            }
        }
    }
}
