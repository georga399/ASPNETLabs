using System.Text.Json.Serialization;
using WEB_253505_AZAROV.Domain.Cart;
using WEB_253505_AZAROV.Domain.Entities;
using WEB_253505_AZAROV.UI.Extensions;
namespace WEB_253505_AZAROV.UI.Services.CartService;
public class SessionCart : Cart
{
    [JsonIgnore]
    public ISession? Session { get; set; }
    public static Cart GetCart(IServiceProvider services)
    {
        ISession session = services.GetRequiredService<IHttpContextAccessor>().HttpContext.Session;
        SessionCart cart = session.Get<SessionCart>("cart") ?? new SessionCart();
        cart.Session = session;
        return cart;
    }
    public override void AddToCart(Item item)
    {
        base.AddToCart(item);
        Session?.Set<SessionCart>("cart", this);
    }
    public override void RemoveItems(int id)
    {
        base.RemoveItems(id);
        Session?.Set<SessionCart>("cart", this);
    }
    public override void ClearAll()
    {
        base.ClearAll();
        Session?.Remove("cart");
    }
}