using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WEB_253505_AZAROV.API.Data;
using WEB_253505_AZAROV.API.Services;
using WEB_253505_AZAROV.Domain.Entities;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
    {
        return Ok(await _categoryService.GetCategoryListAsync());
    }

}
