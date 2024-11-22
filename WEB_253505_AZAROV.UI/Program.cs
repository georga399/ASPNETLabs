using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Configuration;
using  Microsoft.AspNetCore.Authentication.JwtBearer;
using WEB_253505_AZAROV.UI.Services;
using WEB_253505_AZAROV.UI.Services.FileService;
using WEB_253505_AZAROV.UI.HelperClasses;
using WEB_253505_AZAROV.UI.Services.Authentication;

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

var keycloakData =
builder.Configuration.GetSection("Keycloak").Get<KeycloakData>();
builder.Services.AddAuthentication(options =>
                {
                    options.DefaultScheme =
                    CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme =
                    OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie()
                .AddJwtBearer()
                .AddOpenIdConnect(opt =>
                {
                    opt.Authority = $"{keycloakData!.Host}/auth/realms/{keycloakData.Realm}";
                    opt.ClientId = keycloakData.ClientId;
                    opt.ClientSecret = keycloakData.ClientSecret;
                    opt.ResponseType = OpenIdConnectResponseType.Code;
                    opt.Scope.Add("openid");
                    opt.SaveTokens = true;
                    opt.RequireHttpsMetadata = false;
                    opt.MetadataAddress = $"{keycloakData.Host}/realms/{keycloakData.Realm}/.well-known/openid-configuration";
                });
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<ITokenAccessor, KeycloakTokenAccessor>();

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
