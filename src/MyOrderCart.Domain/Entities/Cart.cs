namespace MyOrderCart.Domain.Entities;

public class Cart
{
	private readonly List<CartItem> _items = new();

	public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();
	public decimal TotalPrice => _items.Sum(i => i.TotalPrice);

	public void AddProduct(Product product)
	{
		if (product == null)
			throw new ArgumentNullException(nameof(product));

		if (product.Id <= 0 || product.Price<= 0)
			throw new ArgumentException("Invalid product.");

		var existingItem = _items.FirstOrDefault(i => i.Product.Id == product.Id);
		if (existingItem != null)
		{
			existingItem.IncrementQuantity();
			return;
		}

		_items.Add(new CartItem(product));
	}

	public void RemoveProduct(int productId)
	{
		var item = _items.FirstOrDefault(i => i.Product.Id == productId);
		if (item != null)
			_items.Remove(item);
	}
}