using MyOrderCart.Application.Interfaces;
using MyOrderCart.Domain.Entities;

namespace MyOrderCart.Application.Services;

public class OrderService : IOrderService
{
	private readonly IExternalOrderSender _sender;
	private readonly IOrderRepository _orderRepository;


	public OrderService(IExternalOrderSender sender, IOrderRepository orderRepository)
	{
		_sender = sender;
		_orderRepository = orderRepository;
	}

	public async Task ConfirmOrderAsync(Cart cart, CancellationToken cancellationToken = default)
	{
		if (cart.IsConfirmed)
			throw new InvalidOperationException("Cart is already confirmed.");

		var result = await _sender.SendOrderAsync(cart, cancellationToken);
		if (!result)
			throw new Exception("Failed to send order.");
		cart.Confirm();

		var order = CreateOrder(cart);

		await _orderRepository.SaveOrderAsync(order, cancellationToken);
	}

	private Order CreateOrder(Cart cart)
	{
		return new Order
		{
			TotalPrice = cart.TotalPrice,
			Items = cart.Items.Select(i => new OrderItem
			{
				ProductId = i.Product.Id,
				Title = i.Product.Title,
				Price = i.Product.Price,
				Quantity = i.Quantity
			}).ToList()
		};
	}
}