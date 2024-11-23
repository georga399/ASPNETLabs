using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System.Diagnostics.CodeAnalysis;
using WEB_253505_AZAROV.UI.Services;
using WEB_253505_AZAROV.Domain.Entities;
using WEB_253505_AZAROV.Domain.Models;
using WEB_253505_AZAROV.UI.Controllers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json.Linq;

namespace WEB_253505_AZAROV.Tests;

public class ProductControllerTests
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    public ProductControllerTests()
    {
        _productService = Substitute.For<IProductService>();
        _categoryService = Substitute.For<ICategoryService>();
    }

    [Fact]
    public void Index_GettingCategoryListFailed_404()
    {
        // Arrange
        _productService.GetProductListAsync(null).Returns(new ResponseData<ListModel<Item>>()
        {
            Successfull = false
        });

        _categoryService.GetCategoryListAsync().Returns(new ResponseData<List<Category>>()
        {
            Successfull = false
        });

        var controllerContext = new ControllerContext();
        var httpContext = Substitute.For<HttpContext>();
        httpContext.Request.Headers.Returns(new HeaderDictionary());
        controllerContext.HttpContext = httpContext;

        var controller = new ProductController(_productService, _categoryService)
        {
            ControllerContext = controllerContext
        };

        // Act
        var result = controller.Index(null).Result;

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void Index_GettingProductListFailed_404()
    {
        // Arrange
        _productService.GetProductListAsync(null).Returns(new ResponseData<ListModel<Item>>()
        {
            Successfull = false
        });

        _categoryService.GetCategoryListAsync().Returns(new ResponseData<List<Category>>()
        {
            Successfull = true
        });

        var controllerContext = new ControllerContext();
        var httpContext = Substitute.For<HttpContext>();
        httpContext.Request.Headers.Returns(new HeaderDictionary());
        controllerContext.HttpContext = httpContext;

        var controller = new ProductController(_productService, _categoryService)
        {
            ControllerContext = controllerContext
        };

        // Act
        var result = controller.Index(null).Result;

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void Index_ViewData_Should_Contain_CategoryList()
    {
        // Arrange
        _productService.GetProductListAsync(null).Returns(new ResponseData<ListModel<Item>>()
        {
            Successfull = true,
            Data = new ListModel<Item>
            {
                Items = GetTestConstructors()
            }
        });

        _categoryService.GetCategoryListAsync().Returns(new ResponseData<List<Category>>()
        {
            Successfull = true,
            Data = GetTestCategories()
        });

        var controllerContext = new ControllerContext();
        var tempDataProvider = Substitute.For<ITempDataProvider>();
        var httpContext = Substitute.For<HttpContext>();
        httpContext.Request.Headers.Returns(new HeaderDictionary());
        controllerContext.HttpContext = httpContext;

        var controller = new ProductController(_productService, _categoryService)
        {
            ControllerContext = controllerContext,
            TempData = new TempDataDictionary(controllerContext.HttpContext, tempDataProvider)
        };

        // Act
        var result = controller.Index(null).Result;

        // Assert
        Assert.NotNull(result);

        var viewResult = Assert.IsType<ViewResult>(result);

        var categories = viewResult.ViewData["Categories"] as List<Category>;

        Assert.NotNull(categories);
        Assert.NotEmpty(categories);
        Assert.Equal(GetTestCategories(), categories, new CategoryComparer());
    }

    [Fact]
    public void Index_ViewData_Should_Contain_CorrectCurrentCategory()
    {
        // Arrange
        _productService.GetProductListAsync(GetTestCategories()[0].NormalizedName)
            .Returns(new ResponseData<ListModel<Item>>()
        {
            Successfull = true,
            Data = new ListModel<Item>
            {
                Items = GetTestConstructors()
            }
        });

        _categoryService.GetCategoryListAsync().Returns(new ResponseData<List<Category>>()
        {
            Successfull = true,
            Data = GetTestCategories()
        });

        var controllerContext = new ControllerContext();
        var tempDataProvider = Substitute.For<ITempDataProvider>();
        var httpContext = Substitute.For<HttpContext>();
        httpContext.Request.Headers.Returns(new HeaderDictionary());
        controllerContext.HttpContext = httpContext;

        var controller = new ProductController(_productService, _categoryService)
        {
            ControllerContext = controllerContext,
            TempData = new TempDataDictionary(controllerContext.HttpContext, tempDataProvider)
        };

        // Act
        var result = controller.Index(GetTestCategories()[0].NormalizedName).Result;

        // Assert
        Assert.NotNull(result);

        var viewResult = Assert.IsType<ViewResult>(result);

        var currentCategory = viewResult.ViewData["CurrentCategory"].ToString();

        Assert.NotNull(currentCategory);
        Assert.NotEmpty(currentCategory);
        Assert.Equal(GetTestCategories()[0].Name, currentCategory);
    }

    [Fact]
    public void Index_View_Should_Contain_ProductList()
    {
        // Arrange
        _productService.GetProductListAsync(null).Returns(new ResponseData<ListModel<Item>>()
        {
            Successfull = true,
            Data = new ListModel<Item>
            {
                Items = GetTestConstructors()
            }
        });

        _categoryService.GetCategoryListAsync().Returns(new ResponseData<List<Category>>()
        {
            Successfull = true,
            Data = GetTestCategories()
        });

        var controllerContext = new ControllerContext();
        var tempDataProvider = Substitute.For<ITempDataProvider>();
        var httpContext = Substitute.For<HttpContext>();
        httpContext.Request.Headers.Returns(new HeaderDictionary());
        controllerContext.HttpContext = httpContext;

        var controller = new ProductController(_productService, _categoryService)
        {
            ControllerContext = controllerContext,
            TempData = new TempDataDictionary(controllerContext.HttpContext, tempDataProvider)
        };

        // Act
        var result = controller.Index(null).Result;

        // Assert
        Assert.NotNull(result);
        var viewResult = Assert.IsType<ViewResult>(result);

        var productsList = Assert.IsType<ListModel<Item>>(viewResult.Model);

        Assert.NotNull(productsList);
        Assert.NotEmpty(productsList.Items);
        Assert.Equal(GetTestConstructors(), productsList.Items, new ItemComparer());
    }

    private List<Category> GetTestCategories()
    {
        return new List<Category>() 
            {
                new Category() { Id = 1, Name="Name1", NormalizedName="name-1"},
                new Category() { Id = 2, Name="Name2", NormalizedName="name-2"}
            };
    }

    private List<Item> GetTestConstructors()
    {
        return new List<Item>()
                {
                    new() { Id = 1, Cost=1032, Description="Description1", Name = "Device1", CategoryId = 1},
                    new() { Id = 2, Cost=66, Description="Description2", Name = "Device1", CategoryId = 2},
                };
    }
}

public class CategoryComparer : IEqualityComparer<Category>
{
    public bool Equals(Category? x, Category? y)
    {
        if (ReferenceEquals(x, y))
            return true;

        if (ReferenceEquals(y, null) || ReferenceEquals(x, null))
            return false;

        return x.Name == y.Name && x.NormalizedName == y.NormalizedName;
    }

    public int GetHashCode([DisallowNull] Category obj)
    {
        return obj.Id.GetHashCode() ^ obj.Name.GetHashCode() ^ obj.NormalizedName.GetHashCode();
    }
}

public class ItemComparer : IEqualityComparer<Item>
{
    public bool Equals(Item? x, Item? y)
    {
        if (ReferenceEquals(x, y))
            return true;

        if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            return false;

        return x.CategoryId == y.CategoryId
            && x.Description == y.Description
            && x.Name == y.Name
            && x.Cost == y.Cost
            && x.ImageURI == y.ImageURI;
    }

    public int GetHashCode([DisallowNull] Item obj)
    {
        return obj.Id.GetHashCode()
            ^ obj.Cost.GetHashCode()
            ^ obj.CategoryId.GetHashCode()
            ^ obj.Description.GetHashCode()
            ^ obj.Name.GetHashCode();
    }
}