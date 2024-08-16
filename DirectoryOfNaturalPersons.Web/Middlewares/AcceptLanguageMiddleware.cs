using System.Globalization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DirectoryOfNaturalPersons.Middlewares
{
    public class AcceptLanguageMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly CultureInfo DefaultCulture = new("en-US");

        private static readonly CultureInfo[] SupportedCultures =
        {
            new CultureInfo("en-US"),
            new CultureInfo("ka-GE")
        };

        public AcceptLanguageMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var acceptLanguageHeader = context.Request.Headers["Accept-Language"].ToString();
            var culture = DefaultCulture;

            if (!string.IsNullOrWhiteSpace(acceptLanguageHeader))
            {
                var languages = acceptLanguageHeader.Split(',')
                    .Select(lang => lang.Trim().Split(';')[0]);

                culture = languages.Select(TryGetSupportedCulture)
                    .FirstOrDefault(c => c != null) ?? DefaultCulture;
            }

            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
            context.Items["Accept-Language"] = culture;
            context.Response.Headers["Content-Language"] = culture.Name;

            await _next(context);
        }

        private static CultureInfo? TryGetSupportedCulture(string name)
        {
            return SupportedCultures.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public class CustomHeaderSwaggerAttribute : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                operation.Parameters ??= new List<OpenApiParameter>();
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "Accept-Language",
                    In = ParameterLocation.Header,
                    Required = false,
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                        Enum = new List<IOpenApiAny>
                        {
                            new OpenApiString("en-US"),
                            new OpenApiString("ka-GE")
                        },
                        Default = new OpenApiString("en-US")
                    }
                });
            }
        }
    }
}