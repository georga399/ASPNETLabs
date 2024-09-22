using WEB_253505_AZAROV.UI.Services;
public static class HostingExtensions
{
    public static void RegisterCustomServices(
                            this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<ICategoryService, MemoryCategoryService>();
        builder.Services.AddScoped<IProductService, MemoryProductService>();
    }
}