using Moq.Protected;
using Moq;
using MyOrderCart.Domain.Entities;
using System.Net;
using MyOrderCart.Infrastructure.Services;

namespace MyOrderCart.UnitTests.Infrastructure;

public class ExternalOrderSenderTests
{
	[Fact]
	public async Task SendOrderAsync_Should_Return_True_On_200_OK()
	{
		// Arrange
		var handlerMock = new Mock<HttpMessageHandler>();
		handlerMock.Protected()
			.Setup<Task<HttpResponseMessage>>(
				"SendAsync",
				ItExpr.IsAny<HttpRequestMessage>(),
				ItExpr.IsAny<CancellationToken>()
			)
			.ReturnsAsync(new HttpResponseMessage
			{
				StatusCode = HttpStatusCode.OK
			});

		var httpClient = new HttpClient(handlerMock.Object);

		var sender = new ExternalOrderSender(httpClient);

		var cart = new Cart();
		cart.AddProduct(new Product { Id = 1, Title = "Test product", Price = 10 });

		// Act
		var result = await sender.SendOrderAsync(cart);

		// Assert
		Assert.True(result);
	}


	[Fact]
	public async Task SendOrderAsync_Should_Return_False_On_500()
	{
		// Arrange
		var handlerMock = new Mock<HttpMessageHandler>();
		handlerMock.Protected()
			.Setup<Task<HttpResponseMessage>>(
				"SendAsync",
				ItExpr.IsAny<HttpRequestMessage>(),
				ItExpr.IsAny<CancellationToken>()
			)
			.ReturnsAsync(new HttpResponseMessage
			{
				StatusCode = HttpStatusCode.InternalServerError
			});

		var httpClient = new HttpClient(handlerMock.Object);

		var sender = new ExternalOrderSender(httpClient);

		var cart = new Cart();
		cart.AddProduct(new Product { Id = 1, Title = "Test Product", Price = 10 });

		// Act
		var result = await sender.SendOrderAsync(cart);

		// Assert
		Assert.False(result);
	}


	[Fact]
	public async Task SendOrderAsync_Should_Return_False_On_Timeout()
	{
		// Arrange
		var handlerMock = new Mock<HttpMessageHandler>();
		handlerMock.Protected()
			.Setup<Task<HttpResponseMessage>>(
				"SendAsync",
				ItExpr.IsAny<HttpRequestMessage>(),
				ItExpr.IsAny<CancellationToken>()
			)
			.ThrowsAsync(new TaskCanceledException("Simulated timeout"));

		var httpClient = new HttpClient(handlerMock.Object);

		var sender = new ExternalOrderSender(httpClient);

		var cart = new Cart();
		cart.AddProduct(new Product { Id = 1, Title = "Test", Price = 10 });

		// Act
		var result = await sender.SendOrderAsync(cart);

		// Assert
		Assert.False(result);
	}

}