using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using NSubstitute;
using WEB_253505_AZAROV.API.Data;
using WEB_253505_AZAROV.API.Services;
using WEB_253505_AZAROV.Domain.Entities;
using WEB_253505_AZAROV.Domain.Models;

namespace WEB_253505_AZAROV.Tests;

public class ProductServiceTests
{
    private readonly DbContextOptions<AppDbContext> _dbContextOptions;
    private readonly Microsoft.Extensions.Configuration.IConfiguration configuration;
    public ProductServiceTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                                    .UseInMemoryDatabase("ProductServiceTests")
                                    .Options;
        configuration = Substitute.For<Microsoft.Extensions.Configuration.IConfiguration>();
    }

    [Fact]
    public void Handle_ValidRequest_ShouldReturnPaginatedListWith3ItemsAndCorrectTotalPagesCount()
    {
        // Arrange
        using var context = CreateContext();

        var service = new ProductService(context, configuration);

        // Act
        var result = service.GetProductListAsync(null).Result;

        // Assert
        Assert.IsType<ResponseData<ListModel<Item>>>(result);
        Assert.True(result.Successfull);
        Assert.Equal(1, result.Data.CurrentPage);
        Assert.Equal(3, result.Data.Items.Count);
        Assert.Equal(2, result.Data.TotalPages);
        Assert.Equal(context.Items.First(), result.Data.Items[0]);
    }

    [Fact]
    public void Handle_ValidReuqest_ShouldCorrectlyChooseGivenPage()
    {
        // Arrange
        using var context = CreateContext();
        var service = new ProductService(context, configuration);
        int pageNo = 2;

        // Act
        var result = service.GetProductListAsync(null, pageNo: 2).Result;

        // Assert
        Assert.IsType<ResponseData<ListModel<Item>>>(result);
        Assert.True(result.Successfull);
        Assert.Equal(2, result.Data.CurrentPage);
    }

    [Fact]
    public void Handle_ValidRequest_ShouldCorrectlyFilterByCategory()
    {
        // Arrange
        using var context = CreateContext();
        var service = new ProductService(context, configuration);
        string category = "name-1";

        // Act
        var result = service.GetProductListAsync(category).Result;

        // Assert
        Assert.IsType<ResponseData<ListModel<Item>>>(result);
        Assert.True(result.Successfull);
        Assert.Equal(2, result.Data.Items.Count);
    }

    [Fact]
    public void Handle_SetPageSizeGreaterThanMaximum_ShouldNotAllowSet()
    {
        // Arrange
        using var context = CreateContext();
        var service = new ProductService(context, configuration);
        int pageSize = 54;

        // Act
        var result = service.GetProductListAsync(null, pageSize: pageSize).Result;

        // Assert
        Assert.IsType<ResponseData<ListModel<Item>>>(result);
        Assert.True(result.Successfull);
        Assert.True((int)Math.Ceiling(result.Data.Items.Count / (double)result.Data.TotalPages) != pageSize);
    }

    [Fact]
    public void Handle_PageNoGreaterThanMaximumRequest_ReturnsSuccesfullIsFalse()
    {
        // Arrange
        using var context = CreateContext();
        var service = new ProductService(context, configuration);
        int pageNo = 54;

        // Act
        var result = service.GetProductListAsync(null, pageNo: pageNo).Result;

        // Assert
        Assert.IsType<ResponseData<ListModel<Item>>>(result);
        Assert.False(result.Successfull);
    }

    private AppDbContext CreateContext()
    {
        var context = new AppDbContext(_dbContextOptions);

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        context.Items.AddRange(
            new Item{ Description = "Descr1", Cost = 1, CategoryId = 1, Name = "Item" },
            new Item{ Description = "Descr2", Cost = 2, CategoryId = 2, Name = "Item" },
            new Item{ Description = "Descr3", Cost = 3, CategoryId = 3, Name = "Item" },
            new Item{ Description = "Descr4", Cost = 4, CategoryId = 1, Name = "Item" },
            new Item{ Description = "Descr5", Cost = 5, CategoryId = 2, Name = "Item" },
            new Item{ Description = "Descr6", Cost = 6, CategoryId = 3, Name = "Item" }
        );

        context.Categories.AddRange(
            new Category { Id = 1, Name = "Name1", NormalizedName = "name-1" },
            new Category { Id = 2, Name = "Name2", NormalizedName = "name-2" },
            new Category { Id = 3, Name = "Name3", NormalizedName = "name-3" });

        context.SaveChanges();

        return context;
    }
}