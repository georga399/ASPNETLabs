using Microsoft.EntityFrameworkCore;
using WEB_253505_AZAROV.API.Data;
using WEB_253505_AZAROV.Domain.Entities;
using WEB_253505_AZAROV.Domain.Models;

namespace WEB_253505_AZAROV.API.Services;
public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;
    public CategoryService(AppDbContext appDbContext) 
    {
        _context = appDbContext;
    }
    public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
    {
        var response = new ResponseData<List<Category>>();
        response.Data = await _context.Categories.ToListAsync();
        return response;
    }
}