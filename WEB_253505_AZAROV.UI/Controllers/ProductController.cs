using Microsoft.AspNetCore.Mvc;
using WEB_253505_AZAROV.Domain.Entities;
using WEB_253505_AZAROV.UI.Services;
using WEB_253505_AZAROV.UI.Extensions;
namespace WEB_253505_AZAROV.UI.Controllers;
public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private ILogger<ProductController> _logger;
    public ProductController(IProductService productService, ICategoryService categoryService, ILogger<ProductController> logger) 
    {
        _productService = productService;
        _categoryService = categoryService;
        _logger = logger;
    }
    public async Task<ActionResult> Index(string? category, [FromQuery] int pageNo = 1)
    {
        var _categories = _categoryService.GetCategoryListAsync().Result.Data!;
        var productResponse =
            await _productService.GetProductListAsync(category, pageNo);
        if(!productResponse.Successfull)
            return NotFound(productResponse.ErrorMessage);
        var curCategory = _categories.FirstOrDefault(c => c.NormalizedName == category);
        ViewData["currentCategory"] = curCategory?.Name;
        ViewData["Categories"] = _categories;
        if (Request.IsAjaxRequest())
        {
            return PartialView("_PaginationPartial", new
            {
                CurrentCategory = category,
                Categories = _categories,
                Items = productResponse.Data!.Items,
                ReturnUrl = Request.Path + Request.QueryString.ToUriComponent(),
                CurrentPage = productResponse.Data.CurrentPage,
                TotalPages = productResponse.Data.TotalPages,
                Admin = false
            });
        }
        return View(productResponse.Data);
    }

}
