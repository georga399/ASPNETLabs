using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
                                            [FromQuery] int pageNo = 1,
                                            [FromQuery] int pageSize = 3)
    {
        return Ok(await _productService.GetProductListAsync(
                                            category,
                                            pageNo,
                                            pageSize));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Item>> GetItem(int id)
    {
        var item = await _productService.GetProductByIdAsync(id);
        if (!item.Successfull)
        {
            return NotFound();
        }
        return Ok(item);
    }
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteItem(int id)
    {
        await _productService.DeleteProductAsync(id);
        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Item>> CreateItem(Item item)
    {
        var response = await _productService.CreateProductAsync(item);
        if (!response.Successfull)
        {
            return BadRequest(response.ErrorMessage);
        }
        return Ok(response);
    }
    [HttpPut("{id:int}")]
    public async Task<ActionResult<Item>> UpdateItem(int id, Item item)
    {
        await _productService.UpdateProductAsync(id, item);
        return Ok();
    }
}
