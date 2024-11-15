using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_253505_AZAROV.UI.Services;
using WEB_253505_AZAROV.Domain.Entities;

namespace WEB_253505_AZAROV.UI.Areas.Admin.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        public DetailsModel(ICategoryService categoryService, IProductService productService)
        {
            _productService = productService; 
            _categoryService = categoryService;
        }
        public Item Item { get; set; } = default!;
        public Category Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemData = await _productService.GetProductByIdAsync(id.Value);
            if (itemData == null)
            {
                return NotFound();
            }
            else
            {
                Item = itemData.Data!;
                var categories = await _categoryService.GetCategoryListAsync();
                Category = categories.Data?.FirstOrDefault(c => c.Id == Item.CategoryId)!;
            }
            return Page();
        }
    }
}
