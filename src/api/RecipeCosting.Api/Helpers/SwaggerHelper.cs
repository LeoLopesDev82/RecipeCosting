using System.Reflection;
using Microsoft.OpenApi;

namespace RecipeCosting.Api.Helpers;

/// <summary>
/// Builds the Swagger document from the XML documentation the project generates.
/// </summary>
public static class SwaggerHelper
{
    private const string BearerScheme = "Bearer";

    /// <summary>
    /// Registers the Swagger generator, feeds it the compiled XML summaries and
    /// adds the button that carries a token on the try-it-out calls.
    /// </summary>
    /// <param name="services">The service collection being configured.</param>
    public static void AddDocumentedSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Recipe Costing API",
                Version = "v1",
                Description = "Prices what a baker sells, from the cost of the ingredients "
                    + "and the worth of the hours spent making it.",
            });

            options.IncludeXmlComments(XmlDocumentationPath(), includeControllerXmlComments: true);

            options.AddSecurityDefinition(BearerScheme, new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Paste the token returned by /api/auth/login.",
            });

            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference(BearerScheme, document)] = [],
            });
        });
    }

    private static string XmlDocumentationPath()
    {
        var fileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

        return Path.Combine(AppContext.BaseDirectory, fileName);
    }
}
