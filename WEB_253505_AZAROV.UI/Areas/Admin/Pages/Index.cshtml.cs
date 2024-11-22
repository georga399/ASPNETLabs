using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_253505_AZAROV.Domain.Models;
using WEB_253505_AZAROV.UI.Services;
using WEB_253505_AZAROV.Domain.Entities;
using Newtonsoft.Json.Linq;
using WEB_253505_AZAROV.UI.Extensions;

namespace WEB_253505_AZAROV.UI.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;
        [BindProperty]
        public List<Item> Items { get; set;} = new();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public IndexModel(IProductService productService)
        {
            _productService = productService;
            var items = _productService.GetProductListAsync(null).Result.Data;
            for (int i = 1; i <= items?.TotalPages; i++) 
            {
                Items.AddRange(_productService.GetProductListAsync(null, i).Result.Data!.Items);
            }
        }
        public async Task<IActionResult> OnGet(int pageNo = 1)
        {
            var response = await _productService.GetProductListAsync(null, pageNo);
            if (response.Successfull)
            {
                Items = response!.Data!.Items;
                TotalPages = response.Data.TotalPages;
                CurrentPage = pageNo;

                if (Request.IsAjaxRequest())
                {
                    return Partial("_PaginationPartial", new
                    {
                        Admin = true,
                        CurrentPage = CurrentPage,
                        TotalPages = TotalPages,
                        Items = Items
                    });
                }

                return Page();
            }

            return RedirectToPage("/Error");
        }
    }
}
