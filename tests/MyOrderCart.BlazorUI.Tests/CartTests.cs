using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MyOrderCart.Application.Interfaces;
using MyOrderCart.BlazorUI.Components.Pages;
using MyOrderCart.BlazorUI.Models;
using MyOrderCart.BlazorUI.Services;
using MyOrderCart.Domain.Entities;
using Cart = MyOrderCart.Domain.Entities.Cart;

namespace MyOrderCart.BlazorUI.Tests;

public class CartTests : TestContext
{
	[Fact]
	public void Cart_Component_Displays_Cart_Items_And_Total()
	{
		// Arrange
		var cartService = new CartService();
		var product = new Product
		{
			Id = 1,
			Title = "Test Product",
			Price = 50m
		};
		cartService.AddProduct(product);

		var mockOrderService = new Mock<IOrderService>();
		Services.AddSingleton(mockOrderService.Object);
		Services.AddSingleton(cartService);
		var cut = RenderComponent<Components.Pages.Cart>();

		// Assert
		cut.Markup.Should().Contain("Test Product");
		cut.Markup.Should().Contain("50"); // Unit price
		cut.Markup.Should().Contain("1");  // Quantity
		cut.Markup.Should().Contain("50"); // Total per line and total
	}

	[Fact]
	public void Cart_Component_Confirm_Button_Confirms_Order()
	{
		// Arrange
		var cartService = new CartService();
		var product = new MyOrderCart.Domain.Entities.Product
		{
			Id = 1,
			Title = "Test Product",
			Price = 100m
		};
		cartService.AddProduct(product);

		Services.AddSingleton(cartService);

		var mockOrderService = new Mock<IOrderService>();
		mockOrderService
			.Setup(s => s.ConfirmOrderAsync(It.IsAny<MyOrderCart.Domain.Entities.Cart>(), It.IsAny<CancellationToken>()))
			.Callback<MyOrderCart.Domain.Entities.Cart, CancellationToken>((cart, _) => cart.Confirm())
			.Returns(Task.CompletedTask);
		Services.AddSingleton(mockOrderService.Object);

		var cut = RenderComponent<Components.Pages.Cart>();

		// Act
		var confirmButton = cut.Find("button"); 
		confirmButton.Click();

		// Assert
		cut.Find("button").HasAttribute("disabled").Should().BeTrue();
		cut.Markup.Should().Contain("Order confirmed");
	}


}