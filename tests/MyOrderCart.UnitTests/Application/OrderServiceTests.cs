using Moq;
using MyOrderCart.Application.Interfaces;
using MyOrderCart.Application.Services;
using MyOrderCart.Domain.Entities;

namespace MyOrderCart.UnitTests.Application;

public class OrderServiceTests
{
	[Fact]
	public async Task ConfirmOrder_Should_LockCart_After_Successful_Post()
	{
		//arrange
		var cart = new Cart();
		var product = new Product { Id = 1, Title = "Test Product", Price = 10 };
		cart.AddProduct(product);


		var mockSender = new Mock<IExternalOrderSender>();
		mockSender
			.Setup(s => s.SendOrderAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(true);
		
		var repository = new Mock<IOrderRepository>().Object;

		var service= new OrderService(mockSender.Object,repository);

		//act
		await service.ConfirmOrderAsync(cart, CancellationToken.None);

		//assert
		Assert.True(cart.IsConfirmed);

	}

	[Fact]
	public async Task ConfirmOrder_Should_Throw_When_Cart_Already_Confirmed()
	{
		// Arrange
		var cart = new Cart();
		cart.Confirm(); 

		var mockSender = new Mock<IExternalOrderSender>();
		var repository = new Mock<IOrderRepository>().Object;
		var service = new OrderService(mockSender.Object, repository);

		// Act & Assert
		await Assert.ThrowsAsync<InvalidOperationException>(() =>
			service.ConfirmOrderAsync(cart));
	}


	[Fact]
	public async Task ConfirmOrder_Should_Throw_When_Sender_Fails()
	{
		// Arrange
		var cart = new Cart();
		cart.AddProduct(new Product { Id = 1, Title = "Test", Price = 10 });

		var mockSender = new Mock<IExternalOrderSender>();
		mockSender
			.Setup(s => s.SendOrderAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(false);

		var repository = new Mock<IOrderRepository>().Object;
		var service = new OrderService(mockSender.Object, repository);

		// Act & Assert
		await Assert.ThrowsAsync<Exception>(() =>
			service.ConfirmOrderAsync(cart));
	}

}