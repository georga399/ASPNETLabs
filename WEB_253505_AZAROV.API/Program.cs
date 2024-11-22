using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WEB_253505_AZAROV.API.Models;
using WEB_253505_AZAROV.API.Data;
using WEB_253505_AZAROV.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>( options => options.UseSqlite(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddControllers();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();

var authServer = builder.Configuration.GetSection("AuthServer")
                                      .Get<AuthServerData>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
                {
                    opt.MetadataAddress = $"{authServer!.Host}/realms/{authServer.Realm}/.well-known/openid-configuration";
                    opt.Authority = $"{authServer.Host}/realms/{authServer.Realm}";
                    opt.Audience = "account";
                    opt.RequireHttpsMetadata = false;
                });
builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("admin", p => p.RequireRole("POWER-USER"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

// app.UseHttpsRedirection();

app.UseRouting();

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

await DbInitializer.SeedData(app);

app.Run();
