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
		var cart = new Cart();
		var product = new Product { Id = 1, Title = "Test", Price = 10 };
		cart.AddProduct(product);
		cart.AddProduct(product);

		var senderMock = new Mock<IExternalOrderSender>();
		senderMock
			.Setup(s => s.SendOrderAsync(cart, It.IsAny<CancellationToken>()))
			.ReturnsAsync(true);

		var repoMock = new Mock<IOrderRepository>();
		repoMock
			.Setup(r => r.SaveOrderAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
			.Returns(Task.CompletedTask);

		var service = new OrderService(senderMock.Object, repoMock.Object);

		// Act
		await service.ConfirmOrderAsync(cart);

		// Assert
		senderMock.Verify(s => s.SendOrderAsync(cart, It.IsAny<CancellationToken>()), Times.Once);
		repoMock.Verify(r => r.SaveOrderAsync(
				It.Is<Order>(o =>
					o.TotalPrice == 20 &&
					o.Items.Count == 1 &&
					o.Items.Single().Quantity == 2
				),
				It.IsAny<CancellationToken>()),
			Times.Once);

	}
}