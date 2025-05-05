using MyOrderCart.Application.Interfaces;
using MyOrderCart.Domain.Entities;

namespace MyOrderCart.Application.Services;

public class OrderService: IOrderService
{
	private readonly IExternalOrderSender _sender;

	public OrderService(IExternalOrderSender sender)
	{
		_sender = sender;
	}

	public async Task ConfirmOrderAsync(Cart cart, CancellationToken cancellationToken = default)
	{

		if (cart.IsConfirmed)
			throw new InvalidOperationException("Cart is already confirmed.");

		var result = await _sender.SendOrderAsync(cart, cancellationToken);
		if (!result)
			throw new Exception("Failed to send order.");
		cart.Confirm();
	}
}