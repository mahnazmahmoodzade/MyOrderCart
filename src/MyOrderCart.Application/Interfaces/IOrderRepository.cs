using MyOrderCart.Domain.Entities;

namespace MyOrderCart.Application.Interfaces;

public interface IOrderRepository
{
	Task SaveOrderAsync(Order order, CancellationToken cancellationToken = default);
}