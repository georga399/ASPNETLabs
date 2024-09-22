using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WEB_253505_AZAROV.API.Data;
using WEB_253505_AZAROV.API.Services;
using WEB_253505_AZAROV.Domain.Entities;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private readonly IProductService _productService;
    public ItemsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    [HttpGet("{category}")]
    public async Task<ActionResult<IEnumerable<Item>>> GetItems(string? category,
                                            int pageNo = 1,
                                            int pageSize = 3)
    {
        return Ok(await _productService.GetProductListAsync(
                                            category,
                                            pageNo,
                                            pageSize));
    }

    // [HttpGet("{id}")]
    // public ActionResult<Item> GetItem(int id)
    // {
    //     var item = _context.Items.Find(id);
    //     if (item == null)
    //     {
    //         return NotFound();
    //     }
    //     return Ok(item);
    // }

    // [HttpPost]
    // public ActionResult<Item> CreateItem(Item item)
    // {
    //     _context.Items.Add(item);
    //     _context.SaveChanges();
    //     return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item);
    // }
}
