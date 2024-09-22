using Microsoft.EntityFrameworkCore;
using WEB_253505_AZAROV.API.Data;
using WEB_253505_AZAROV.Domain.Entities;
using WEB_253505_AZAROV.Domain.Models;

namespace WEB_253505_AZAROV.API.Services;
public class ProductService : IProductService
{
    private readonly AppDbContext _context;
    private readonly int _maxPageSize = 20;
    private readonly string _imageURI;
    public ProductService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _imageURI = configuration["ImageURL"]!;
    }
    public async Task<ResponseData<Item>> CreateProductAsync(Item product)
    {
        var cat = _context.Categories.FirstOrDefault(c => c.Id == product.CategoryId);
        if (cat == null)
        {
            return ResponseData<Item>.Error("Category not found");
        }
        _context.Items.Add(product);
        await _context.SaveChangesAsync();
        return ResponseData<Item>.Success(product);
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await _context.Items.FindAsync(id);
        if (product != null)
            _context.Items.Remove(product);
    }

    public async Task<ResponseData<Item>> GetProductByIdAsync(int id)
    {
        var product = await _context.Items.FindAsync(id);
        if (product == null)
        {
            return ResponseData<Item>.Error("Product not found.");
        }
        return ResponseData<Item>.Success(product);
    }

    public async Task<ResponseData<ListModel<Item>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
    {
        if (pageSize > _maxPageSize)
            pageSize = _maxPageSize;
        var query = _context.Items.AsQueryable();
        var dataList = new ListModel<Item>();
        var cat = await _context.Categories
                    .FirstOrDefaultAsync(c => 
                        c.NormalizedName == categoryNormalizedName);
        if (cat is not null) 
            query = query.Where(d => d.CategoryId == cat.Id);
        if (categoryNormalizedName is null)
            query = query
                .Where(d => categoryNormalizedName==null);
        // количество элементов в списке
        var count = await query.CountAsync();
        if(count==0)
        {
            return ResponseData<ListModel<Item>>.Success(dataList);
        }
        // количество страниц
        int totalPages = (int)Math.Ceiling(count / (double)pageSize);
        if (pageNo > totalPages)
            return ResponseData<ListModel<Item>>.Error("No such page");
        dataList.Items = await query
                            .OrderBy(d=>d.Id)
                            .Skip((pageNo - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
        dataList.CurrentPage = pageNo;
        dataList.TotalPages = totalPages;
        return ResponseData<ListModel<Item>>.Success(dataList); 
    }
    public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)

    {
        if (formFile == null || formFile.Length == 0)
            return ResponseData<string>.Error("Invalid image file.");
        var product = await _context.Items.FindAsync(id);
        if (product == null)
            return ResponseData<string>.Error("Product not found.");
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
        Directory.CreateDirectory(uploadsFolder);
        var fileName = $"{Guid.NewGuid()}_{formFile.FileName}";
        var filePath = Path.Combine(uploadsFolder, fileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await formFile.CopyToAsync(stream);
        }
        product.ImageURI = $"/images/{fileName}";
        await _context.SaveChangesAsync();
        return ResponseData<string>.Success(product.ImageURI);
    }

    public async Task UpdateProductAsync(int id, Item product)
    {
        var existingProduct = await _context.Items.FirstOrDefaultAsync(i => i.Id == id);
        if (existingProduct == null)
        {
            return;
        }
        // Update properties
        var existingCat = await _context.Categories.FirstOrDefaultAsync(c => c.Id == product.CategoryId);
        if (existingCat == null)
        {
            return;
        }
        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Cost = product.Cost;
        existingProduct.CategoryId = product.CategoryId;
        await _context.SaveChangesAsync();
    }
}