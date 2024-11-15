using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_253505_AZAROV.Domain.Entities;
using WEB_253505_AZAROV.UI.Services;

namespace WEB_253505_AZAROV.UI.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        public CreateModel(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
            Categories = new SelectList(_categoryService.GetCategoryListAsync().Result.Data, "Id", "Name");
        }
        public IActionResult OnGet()
        {
            return Page();
        }
        [BindProperty]
        public Item Item { get; set; } = default!;
        [BindProperty]
        public IFormFile? Image {get;set;}
        public SelectList Categories {get;set;}
        public async Task<IActionResult> OnPost() 
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var response = await _productService.CreateProductAsync(Item, Image);
            if (!response.Successfull)
            {
                return Page();
            }
            return RedirectToPage("./Index");
        }
    }
}
