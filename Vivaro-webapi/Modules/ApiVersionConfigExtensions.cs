using Microsoft.Extensions.DependencyInjection;

namespace Vivaro_webapi.Modules;

public static class ApiVersionConfigExtensions
{
    public static IServiceCollection AddApiVersion(this IServiceCollection services)
    {
        _ = services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
        });

        _ = services.AddVersionedApiExplorer(setup =>
        {
            setup.GroupNameFormat = "'v'VVV";
            setup.SubstituteApiVersionInUrl = true;
        });

        return services;
    }
}