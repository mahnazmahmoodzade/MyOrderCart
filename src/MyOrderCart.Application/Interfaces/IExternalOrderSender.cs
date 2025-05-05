using MyOrderCart.Domain.Entities;

namespace MyOrderCart.Application.Interfaces;

public interface IExternalOrderSender
{
	Task<bool> SendOrderAsync(Cart cart, CancellationToken cancellationToken = default);
}