using WEB_253505_AZAROV.Domain.Entities;
namespace WEB_253505_AZAROV.Domain.Cart;
public class Cart
{
    public Dictionary<int, CartItem> CartItems { get; set; } = new();
    public virtual void AddToCart(Item item)
    {
        var exists = CartItems.ContainsKey(item.Id);
        if (exists)
        {
            CartItems[item.Id].Count++;
        }
        else
        {
            CartItems.Add(item.Id, new CartItem { Count = 1, Item = item });
        }
    }
    public virtual void RemoveItems(int id)
    {
        CartItems.Remove(id);
    }
    public virtual void ClearAll()
    {
        CartItems.Clear();
    }
    public int Count { get => CartItems.Sum(item => item.Value.Count);  }
    public double TotalCost { get => CartItems.Sum(item 
        => item.Value.Item.Cost * item.Value.Count); }
}