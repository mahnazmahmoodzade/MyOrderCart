using MyOrderCart.Domain.Entities;

namespace MyOrderCart.Application.Interfaces;

public interface IOrderService
{
	Task ConfirmOrderAsync(Cart cart, CancellationToken cancellationToken = default);
}