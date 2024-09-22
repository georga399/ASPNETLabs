using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using WEB_253505_AZAROV.Domain.Entities;
using WEB_253505_AZAROV.Domain.Models;

namespace WEB_253505_AZAROV.UI.Services;
public class MemoryProductService : IProductService
{
    List<Item> _items;
    List<Category> _categories;
    int _pageSize;
public MemoryProductService([FromServices] IConfiguration config, ICategoryService categoryService)
{
    _categories = categoryService.GetCategoryListAsync()
            .Result
            .Data!;
    SetupData();
    _pageSize = config.GetValue<int>("ItemsPerPage");
}

    public Task<ResponseData<Item>> CreateProductAsync(Item product, IFormFile? formFile)
    {
        throw new NotImplementedException();
    }

    public Task DeleteProductAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseData<Item>> GetProductByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<ResponseData<ListModel<Item>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
    {
        var response = new ResponseData<ListModel<Item>>();
        List<Item> items = new();
        int totalPages = 0;
        if (categoryNormalizedName is null)
        {
            await Task.Run(() => 
            {
                items = _items
                        .Skip((pageNo-1)*_pageSize)
                        .Take(_pageSize)
                        .ToList();
            });
            totalPages = Convert.ToInt32(Math.Ceiling((double)_items.Count/(double)_pageSize));
        }
        else
        {
            var category = _categories.FirstOrDefault(c => c.NormalizedName == categoryNormalizedName);
            if (category is null) 
            {
                response.Successfull = false;
                response.ErrorMessage = "Category not found";
                return response;
            }
            await Task.Run(() => 
            {
                items = _items
                        .Where(i => i.CategoryId == category.Id)
                        .Skip((pageNo - 1)*_pageSize)
                        .Take(_pageSize)
                        .ToList();
            });
            await Task.Run(() => 
            {
                totalPages = Convert.ToInt32(
                    Math.Ceiling(
                        (double)_items
                            .Where(i => i.CategoryId == category.Id)
                            .Count()
                        / (double)_pageSize));
            });
        }
        var listModel = new ListModel<Item> {
            Items = items,
            CurrentPage = pageNo,
            TotalPages = totalPages,
        };
        response.Successfull = true;
        response.Data = listModel;
        return response;
    }

    public Task UpdateProductAsync(int id, Item product, IFormFile? formFile)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Инициализация списков
    /// </summary>
    private void SetupData()
    {
        _items = new List<Item> {
            // Phones
            new Item 
            {
                Id = 1,
                Name = "Samsung A99",
                Description = "Новый флагман от Samsung",
                Cost = 999,
                ImageURI = "images/samsung.jpg",
                CategoryId = _categories.Find(c => c.NormalizedName.Equals("phones"))!.Id
            },
            new Item 
            {
                Id = 2,
                Name = "iPhone 14",
                Description = "Последняя модель от Apple",
                Cost = 1099,
                ImageURI = "images/iphone14.png",
                CategoryId = _categories.Find(c => c.NormalizedName.Equals("phones"))!.Id
            },
            new Item 
            {
                Id = 3,
                Name = "Google Pixel 7",
                Description = "Флагманский смартфон от Google с отличной камерой",
                Cost = 899,
                ImageURI = "images/iphone14.png",
                CategoryId = _categories.Find(c => c.NormalizedName.Equals("phones"))!.Id
            },
            new Item 
            {
                Id = 4,
                Name = "OnePlus 10 Pro",
                Description = "Смартфон с высокой производительностью и быстрой зарядкой",
                Cost = 799,
                ImageURI = "images/oneplus10pro.png",
                CategoryId = _categories.Find(c => c.NormalizedName.Equals("phones"))!.Id
            },

            // Laptops
            new Item 
            {
                Id = 5,
                Name = "Dell XPS 13",
                Description = "Компактный и мощный ноутбук",
                Cost = 1299,
                ImageURI = "images/dellxps13.png",
                CategoryId = _categories.Find(c => c.NormalizedName.Equals("laptops"))!.Id
            },
            new Item 
            {
                Id = 6,
                Name = "MacBook Pro",
                Description = "Ноутбук для профессионалов с высокой производительностью",
                Cost = 2399,
                ImageURI = "images/macbookpro.png",
                CategoryId = _categories.Find(c => c.NormalizedName.Equals("laptops"))!.Id
            },
            new Item 
            {
                Id = 7,
                Name = "HP Spectre x360",
                Description = "Ультратонкий и легкий ноутбук с сенсорным экраном",
                Cost = 1499,
                ImageURI = "images/hpspectrex360.png",
                CategoryId = _categories.Find(c => c.NormalizedName.Equals("laptops"))!.Id
            },
            
            // Watches
            new Item 
            {
                Id = 8,
                Name = "Apple Watch Series 8",
                Description = "Умные часы от Apple с множеством функций",
                Cost = 399,
                ImageURI = "images/applewatch8.png",
                CategoryId = _categories.Find(c => c.NormalizedName.Equals("watches"))!.Id
            },
            new Item 
            {
                Id = 9,
                Name = "Samsung Galaxy Watch 5",
                Description = "Стильные и функциональные часы от Samsung",
                Cost = 329,
                ImageURI = "images/samsunggalaxywatch5.png",
                CategoryId = _categories.Find(c => c.NormalizedName.Equals("watches"))!.Id
            },
            new Item 
            {
                Id=10,
                Name="Fitbit Versa 3",
                Description="Умные часы с функциями отслеживания здоровья и физической активности.",
                Cost=229,
                ImageURI="images/fitbitversa3.png",
                CategoryId=_categories.Find(c => c.NormalizedName.Equals("watches"))!.Id
            },

            // Gadgets
            new Item 
            {
                Id=11,
                Name="Bluetooth Headphones",
                Description="Беспроводные наушники с отличным звуком.",
                Cost=199,
                ImageURI="images/bluetoothheadphones.png",
                CategoryId=_categories.Find(c => c.NormalizedName.Equals("gadgets"))!.Id
            },
            new Item 
            {
                Id=12,
                Name="Smart Home Assistant",
                Description="Умный помощник для вашего дома.",
                Cost=129,
                ImageURI="images/smarthomeassistant.png",
                CategoryId=_categories.Find(c => c.NormalizedName.Equals("gadgets"))!.Id
            },
            new Item 
            {
                Id=13,
                Name="Wireless Charging Pad",
                Description="Беспроводная зарядка для ваших устройств.",
                Cost=49,
                ImageURI="images/wirelesschargingpad.png",
                CategoryId=_categories.Find(c => c.NormalizedName.Equals("gadgets"))!.Id
            },

            // Other
            new Item 
            {
                Id=14,
                Name="Portable Charger",
                Description="Портативное зарядное устройство для ваших устройств.",
                Cost=49,
                ImageURI="images/portablecharger.png",
                CategoryId=_categories.Find(c => c.NormalizedName.Equals("other"))!.Id
            },
            new Item 
            {
                Id=15,
                Name="USB-C Hub",
                Description="USB-C концентратор для подключения нескольких устройств.",
                Cost=39,
                ImageURI="images/usbc_hub.png",
                CategoryId=_categories.Find(c => c.NormalizedName.Equals("other"))!.Id
            }
        };
    }
}