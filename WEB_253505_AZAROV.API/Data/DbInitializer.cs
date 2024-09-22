using Microsoft.EntityFrameworkCore;
using WEB_253505_AZAROV.Domain.Entities;

namespace WEB_253505_AZAROV.API.Data;

public class DbInitializer
{
    public static async Task SeedData(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();await dbContext.Database.EnsureCreatedAsync();
            var _categories = new List<Category>
            {
                new Category {Name="Phones", NormalizedName="phones"},
                new Category {Name="Laptops", NormalizedName="laptops"},
                new Category {Name="Watches", NormalizedName="watches"},
                new Category {Name="Gadgets", NormalizedName="gadgets"},
                new Category {Name="Other", NormalizedName="other"},
            };
            await dbContext.Categories.AddRangeAsync(_categories);
            await dbContext.SaveChangesAsync();

            // Construct base URL
            var baseUrl = app.Configuration["ImageURL"]!;
            var items = new List<Item> {
                // Phones
                new Item 
                {
                    Name = "Samsung A99",
                    Description = "Новый флагман от Samsung",
                    Cost = 999,
                    ImageURI = $"{baseUrl}/images/samsung.jpg",
                    CategoryId = dbContext.Categories.FirstOrDefault(c => c.NormalizedName.Equals("phones"))!.Id
                },
                new Item 
                {
                    Name = "iPhone 14",
                    Description = "Последняя модель от Apple",
                    Cost = 1099,
                    ImageURI = $"{baseUrl}/images/iphone14.png",
                    CategoryId = dbContext.Categories.FirstOrDefault(c => c.NormalizedName.Equals("phones"))!.Id
                },
                new Item 
                {
                    Name = "Google Pixel 7",
                    Description = "Флагманский смартфон от Google с отличной камерой",
                    Cost = 899,
                    ImageURI = $"{baseUrl}/images/iphone14.png",
                    CategoryId = dbContext.Categories.FirstOrDefault(c => c.NormalizedName.Equals("phones"))!.Id
                },
                new Item 
                {
                    Name = "OnePlus 10 Pro",
                    Description = "Смартфон с высокой производительностью и быстрой зарядкой",
                    Cost = 799,
                    ImageURI = $"{baseUrl}/images/oneplus10pro.png",
                    CategoryId = dbContext.Categories.FirstOrDefault(c => c.NormalizedName.Equals("phones"))!.Id
                },

                // Laptops
                new Item 
                {
                    Name = "Dell XPS 13",
                    Description = "Компактный и мощный ноутбук",
                    Cost = 1299,
                    ImageURI = $"{baseUrl}/images/dellxps13.png",
                    CategoryId = dbContext.Categories.FirstOrDefault(c => c.NormalizedName.Equals("laptops"))!.Id
                },
                new Item 
                {
                    Name = "MacBook Pro",
                    Description = "Ноутбук для профессионалов с высокой производительностью",
                    Cost = 2399,
                    ImageURI = $"{baseUrl}/images/macbookpro.png",
                    CategoryId = dbContext.Categories.FirstOrDefault(c => c.NormalizedName.Equals("laptops"))!.Id
                },
                new Item 
                {
                    Name = "HP Spectre x360",
                    Description = "Ультратонкий и легкий ноутбук с сенсорным экраном",
                    Cost = 1499,
                    ImageURI = $"{baseUrl}/images/hpspectrex360.png",
                    CategoryId = dbContext.Categories.FirstOrDefault(c => c.NormalizedName.Equals("laptops"))!.Id
                },
                
                // Watches
                new Item 
                {
                    Name = "Apple Watch Series 8",
                    Description = "Умные часы от Apple с множеством функций",
                    Cost = 399,
                    ImageURI = $"{baseUrl}/images/applewatch8.png",
                    CategoryId = dbContext.Categories.FirstOrDefault(c => c.NormalizedName.Equals("watches"))!.Id
                },
                new Item 
                {
                    Name = "Samsung Galaxy Watch 5",
                    Description = "Стильные и функциональные часы от Samsung",
                    Cost = 329,
                    ImageURI = $"{baseUrl}/images/samsunggalaxywatch5.png",
                    CategoryId = dbContext.Categories.FirstOrDefault(c => c.NormalizedName.Equals("watches"))!.Id
                },
                new Item 
                {
                    Name="Fitbit Versa 3",
                    Description="Умные часы с функциями отслеживания здоровья и физической активности.",
                    Cost=229,
                    ImageURI=$"{baseUrl}/images/fitbitversa3.png",
                    CategoryId=dbContext.Categories.FirstOrDefault(c => c.NormalizedName.Equals("watches"))!.Id
                },

                // Gadgets
                new Item 
                {
                    Name="Bluetooth Headphones",
                    Description="Беспроводные наушники с отличным звуком.",
                    Cost=199,
                    ImageURI=$"{baseUrl}/images/bluetoothheadphones.png",
                    CategoryId=dbContext.Categories.FirstOrDefault(c => c.NormalizedName.Equals("gadgets"))!.Id
                },
                new Item 
                {
                    Name="Smart Home Assistant",
                    Description="Умный помощник для вашего дома.",
                    Cost=129,
                    ImageURI=$"{baseUrl}/images/smarthomeassistant.png",
                    CategoryId=dbContext.Categories.FirstOrDefault(c => c.NormalizedName.Equals("gadgets"))!.Id
                },
                new Item 
                {
                    Name="Wireless Charging Pad",
                    Description="Беспроводная зарядка для ваших устройств.",
                    Cost=49,
                    ImageURI=$"{baseUrl}/images/wirelesschargingpad.png",
                    CategoryId=dbContext.Categories.FirstOrDefault(c => c.NormalizedName.Equals("gadgets"))!.Id
                },

                // Other
                new Item 
                {
                    Name="Portable Charger",
                    Description="Портативное зарядное устройство для ваших устройств.",
                    Cost=49,
                    ImageURI=$"{baseUrl}/images/portablecharger.png",
                    CategoryId=dbContext.Categories.FirstOrDefault(c => c.NormalizedName.Equals("other"))!.Id
                },
                new Item 
                {
                    Name="USB-C Hub",
                    Description="USB-C концентратор для подключения нескольких устройств.",
                    Cost=39,
                    ImageURI=$"{baseUrl}/images/usbc_hub.png",
                    CategoryId=dbContext.Categories.FirstOrDefault(c => c.NormalizedName.Equals("other"))!.Id
                }
            };
            await dbContext.Items.AddRangeAsync(items);
            await dbContext.SaveChangesAsync();
        }
    }
}