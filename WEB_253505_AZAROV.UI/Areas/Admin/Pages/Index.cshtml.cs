using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_253505_AZAROV.Domain.Models;
using WEB_253505_AZAROV.UI.Services;
using WEB_253505_AZAROV.Domain.Entities;

namespace WEB_253505_AZAROV.UI.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;
        [BindProperty]
        public List<Item> Items { get; set;} = new();
        public IndexModel(IProductService productService)
        {
            _productService = productService;
            var items = _productService.GetProductListAsync(null).Result.Data;
            for (int i = 1; i <= items?.TotalPages; i++) 
            {
                Items.AddRange(_productService.GetProductListAsync(null, i).Result.Data!.Items);
            }
        }
        public void OnGet()
        {
        }
    }
}
