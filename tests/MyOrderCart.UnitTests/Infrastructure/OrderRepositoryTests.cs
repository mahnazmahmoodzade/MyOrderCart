using Microsoft.EntityFrameworkCore;
using Moq;
using MyOrderCart.Application.Interfaces;
using MyOrderCart.Application.Services;
using MyOrderCart.Domain.Entities;
using MyOrderCart.Infrastructure.Persistence;
using MyOrderCart.Infrastructure.Repositories;

namespace MyOrderCart.UnitTests.Infrastructure;

public class OrderRepositoryTests
{

	[Fact]
	public async Task ConfirmOrder_Should_Save_Order_To_Database()
	{
		// Arrange
		var cart =new Cart();
		var product= new Product { Id = 1, Title = "Test Product", Price = 10 };
		cart.AddProduct(product);
		cart.AddProduct(product);

		// In-memory Ef Core context setup
		var options = new DbContextOptionsBuilder<OrderDbContext>()
			.UseInMemoryDatabase(databaseName: "OrderDb")
			.Options;

		await using var context = new OrderDbContext(options);

		var repository = new OrderRepository(context);
		var sender =new Mock<IExternalOrderSender>();
		sender.Setup(s => s.SendOrderAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(true);
		var orderService =new OrderService(sender.Object, repository);

		// Act
		await orderService.ConfirmOrderAsync(cart, CancellationToken.None);

		// Assert
		var savedOrder= await context.Orders
			.Include(o => o.Items)
			.FirstOrDefaultAsync();
		Assert.NotNull(savedOrder);
		Assert.Equal(1, savedOrder.Items.Count);
		Assert.Equal(2, savedOrder.Items.First().Quantity);
		Assert.Equal(20, savedOrder.TotalPrice);
	}
}