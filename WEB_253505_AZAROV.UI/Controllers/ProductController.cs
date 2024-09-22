using Microsoft.AspNetCore.Mvc;
using WEB_253505_AZAROV.Domain.Entities;
using WEB_253505_AZAROV.UI.Services;
namespace WEB_253505_AZAROV.UI.Controllers;
public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    public ProductController(IProductService productService, ICategoryService categoryService) 
    {
        _productService = productService;
        _categoryService = categoryService;
    }
    public async Task<ActionResult> Index([FromQuery] string? category, [FromQuery] int pageNo = 1)
    {
        var _categories = _categoryService.GetCategoryListAsync().Result.Data!;
        var productResponse =
            await _productService.GetProductListAsync(category, pageNo);
        if(!productResponse.Successfull)
            return NotFound(productResponse.ErrorMessage);
        var curCategory = _categories.FirstOrDefault(c => c.NormalizedName == category);
        ViewData["currentCategory"] = curCategory?.Name;
        ViewData["Categories"] = _categories;
        return View(productResponse.Data);
    }

}
