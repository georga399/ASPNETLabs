namespace WEB_253505_AZAROV.Domain.Entities;

public class Item 
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public double Cost { get; set; }
    public string ImageURI {get; set; } = "";
    public string Mime { get; set; } = string.Empty;

}