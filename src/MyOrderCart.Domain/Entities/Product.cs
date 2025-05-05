namespace MyOrderCart.Domain.Entities;

public class Product
{
	public int Id { get; set; }
	public string Title { get; set; }= string.Empty;
	public decimal Price { get; set; }
}