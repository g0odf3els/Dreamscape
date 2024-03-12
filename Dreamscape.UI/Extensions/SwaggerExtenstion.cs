using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace Dreamscape.UI.Extensions
{
    public static class SwaggerExtenstion
    {
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "My API",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                 new OpenApiSecurityScheme
                 {
                   Reference = new OpenApiReference
                   {
                     Type = ReferenceType.SecurityScheme,
                     Id = "Bearer"
                   }
                  },
                  new string[] { }
                }
               });
                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    return apiDesc.RelativePath.StartsWith("api/");
                });
                c.EnableAnnotations();

            });
        }
    }
}
