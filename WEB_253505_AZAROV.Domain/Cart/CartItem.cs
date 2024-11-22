using WEB_253505_AZAROV.Domain.Entities;
namespace WEB_253505_AZAROV.Domain.Cart;
public class CartItem
{
    public Item Item { get; set; }
    public int Count { get; set; }
}