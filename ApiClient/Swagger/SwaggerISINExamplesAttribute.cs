using Microsoft.OpenApi.Any;

namespace ApiClient.Swagger
{
    public class SwaggerISINExamplesAttribute : Attribute
    {
        public Microsoft.OpenApi.Any.OpenApiArray Value { get; }

        public SwaggerISINExamplesAttribute()
        {
            var list = new Microsoft.OpenApi.Any.OpenApiArray();
            var values = new[] { "abcdefghijkl", "abcdefghijklmno", "abcdefghijk", "" };

            list.AddRange(values.Select(x => new OpenApiString(x)));
            Value = list;
        }


    }
}
