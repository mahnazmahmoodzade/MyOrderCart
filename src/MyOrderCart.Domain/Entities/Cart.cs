namespace MyOrderCart.Domain.Entities;

public class Cart
{
	private readonly List<CartItem> _items = new();

	public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();
	public decimal TotalPrice => _items.Sum(i => i.TotalPrice);
	public bool IsConfirmed { get; private set; } = false;

	public void AddProduct(Product product)
	{
		if (IsConfirmed)
			throw new InvalidOperationException("Cannot modify a confirmed cart.");

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
		if (IsConfirmed)
			throw new InvalidOperationException("Cannot modify a confirmed cart.");

		var item = _items.FirstOrDefault(i => i.Product.Id == productId);
		if (item != null)
			_items.Remove(item);
	}

	public void Confirm()
	{
		IsConfirmed = true;
	}

	public void DecrementProduct(int productId)
	{
		if(IsConfirmed)
			throw new InvalidOperationException("Cannot modify a confirmed cart.");

		var item = _items.FirstOrDefault(i => i.Product.Id == productId);
		
		if (item == null) return;

		if (item.Quantity > 1)
		{
			item.DecrementQuantity();
		}
		else
		{
			_items.Remove(item);
		}
	}
}