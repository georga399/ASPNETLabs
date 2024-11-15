using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_253505_AZAROV.UI.Services;
using WEB_253505_AZAROV.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WEB_253505_AZAROV.UI.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        public EditModel(IProductService productService, ICategoryService categoryService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }
        [BindProperty]
        public Item Item { get; set; } = default!;
        [BindProperty]
        public IFormFile? Image { get; set; }
        public SelectList? Categories { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var item =  await _productService.GetProductByIdAsync(id.Value);
            if (item == null)
            {
                return NotFound();
            }
            Item = item.Data!;
            Categories = new SelectList(_categoryService.GetCategoryListAsync().Result.Data, "Id", "Name", Item.CategoryId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _productService.UpdateProductAsync(Item.Id, Item, Image);

            return RedirectToPage("./Index");
        }
    }
}
