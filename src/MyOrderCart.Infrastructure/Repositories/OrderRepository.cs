using Microsoft.EntityFrameworkCore;
using MyOrderCart.Application.Interfaces;
using MyOrderCart.Domain.Entities;
using MyOrderCart.Infrastructure.Persistence;

namespace MyOrderCart.Infrastructure.Repositories;

public class OrderRepository: IOrderRepository
{
	private readonly OrderDbContext _context;
	public OrderRepository(OrderDbContext context)
	{
		_context = context;
	}
	public async Task SaveOrderAsync(Order order, CancellationToken cancellationToken)
	{
		await _context.Orders.AddAsync(order, cancellationToken);
		await _context.SaveChangesAsync(cancellationToken);
	}
}