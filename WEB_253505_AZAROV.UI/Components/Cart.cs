using Microsoft.AspNetCore.Mvc;

namespace WEB_253505_AZAROV.UI.Components;

public class CartComponent: ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}