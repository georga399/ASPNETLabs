using WEB_253505_AZAROV.UI.Services;
using WEB_253505_AZAROV.UI.Services.FileService;

var builder = WebApplication.CreateBuilder(args);

URIData.APIURI = builder.Configuration["UriData:ApiUri"]!;

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.RegisterCustomServices();
builder.Services.AddRazorPages();
builder.Services
    .AddHttpClient<IProductService, APIProductService>(opt=>
            opt.BaseAddress=new Uri(URIData.APIURI));
builder.Services
    .AddHttpClient<ICategoryService, APICategoryService>(opt=>
            opt.BaseAddress=new Uri(URIData.APIURI));
builder.Services.AddHttpClient<IFileService, APIFileService>(opt =>opt.BaseAddress = new Uri($"{URIData.APIURI}Files"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
