using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_253505_AZAROV.UI.Services;
using WEB_253505_AZAROV.Domain.Entities;

namespace WEB_253505_AZAROV.UI.Areas.Admin.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        public DeleteModel(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService; 
        }
        [BindProperty]
        public Item Item { get; set; } = default!;
        public Category Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var item = await _productService.GetProductByIdAsync(id.Value);

            if (item is null)
            {
                return NotFound();
            }
            else
            {
                Item = item.Data!;
                var categories = await _categoryService.GetCategoryListAsync();
                Category = categories.Data?.FirstOrDefault(c => c.Id == Item?.CategoryId)!;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _productService.GetProductByIdAsync(id.Value);
            if (item != null)
            {
                Item = item.Data!;
                await _productService.DeleteProductAsync(id.Value);
            }
            return RedirectToPage("./Index");
        }
    }
}
