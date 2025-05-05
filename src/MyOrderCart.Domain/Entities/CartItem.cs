namespace MyOrderCart.Domain.Entities;

public class CartItem
{
	public Product Product { get; private set; }
	public int Quantity { get; private set; }
	public decimal TotalPrice => Quantity * Product.Price;

	public CartItem(Product product)
	{
		Product = product;
		Quantity = 1;
	}

	public void IncrementQuantity()
	{
		Quantity++;
	}

}