using MyOrderCart.Domain.Entities;

namespace MyOrderCart.BlazorUI.Services;

public class CartService
{
	private readonly Cart _cart = new();

	public event Action? OnChange;
	public IReadOnlyCollection<CartItem> Items => _cart.Items;
	public decimal TotalPrice => _cart.TotalPrice;
	public bool IsConfirmed => _cart.IsConfirmed;

	public void AddProduct(Product product)
	{
		if (_cart.IsConfirmed) return;
		_cart.AddProduct(product);
		NotifyStateChanged();
	}

	public void RemoveProduct(int productId)
	{
		if (_cart.IsConfirmed) return;
		_cart.RemoveProduct(productId);
		NotifyStateChanged();
	}

	public void Confirm()
	{
		_cart.Confirm();
		NotifyStateChanged();
	}

	public Cart GetCart()
	{
		return _cart;
	}

	private void NotifyStateChanged() => OnChange?.Invoke();

	public void DecrementProduct(int productId)
	{
		if (_cart.IsConfirmed) return;
		_cart.DecrementProduct(productId);
		NotifyStateChanged();
	}
}