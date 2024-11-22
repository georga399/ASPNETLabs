using WEB_253505_AZAROV.UI.Services;
using WEB_253505_AZAROV.UI.HelperClasses;
using WEB_253505_AZAROV.UI.Services.FileService;
using WEB_253505_AZAROV.UI.Services.Authentication;

public static class HostingExtensions
{
    public static void RegisterCustomServices(
                            this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<ICategoryService, APICategoryService>();
        builder.Services.AddScoped<IProductService, APIProductService>();
        builder.Services.Configure<KeycloakData>(builder.Configuration.GetSection("Keycloak"));
        builder.Services.AddScoped<ICategoryService, APICategoryService>()
                        .AddScoped<IProductService, APIProductService>()
                        .AddScoped<IFileService, APIFileService>()
                        .AddScoped<IAuthService, KeycloakAuthService>();;
    }
}