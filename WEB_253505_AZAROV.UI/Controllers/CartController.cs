using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_253505_AZAROV.Domain.Cart;
using WEB_253505_AZAROV.UI.Extensions;
using WEB_253505_AZAROV.UI.Services;
namespace WEB_253505_AZAROV.UI.Controllers;
public class CartController : Controller
{
    private readonly IProductService _productService;
    private readonly Cart _cart;
    public CartController(IProductService productService, Cart cart)
    {
      _productService = productService;
      _cart = cart;
    }
    [Authorize]
    public ActionResult Index()
    {
        return View(_cart);
    }
    [Authorize]
    [Route("[controller]/add/{id:int}")]
    public async Task<ActionResult> Add(int id, string returnUrl)
    {
        var data = await _productService.GetProductByIdAsync(id);
        if (data.Successfull)
        {
            _cart.AddToCart(data.Data!);
        }
        return Redirect(returnUrl);
    }
    [Authorize]
    [Route("[controller]/delete/{id:int}")]
    public ActionResult Delete(int id)
    {
        _cart.RemoveItems(id);
        return RedirectToAction("Index", "Cart");
    }
    
}