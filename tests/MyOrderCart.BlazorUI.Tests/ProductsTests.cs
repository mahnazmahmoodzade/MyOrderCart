using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MyOrderCart.BlazorUI.Components.Pages;
using MyOrderCart.BlazorUI.Models;
using MyOrderCart.BlazorUI.Services;
using MyOrderCart.Domain.Entities;

namespace MyOrderCart.BlazorUI.Tests;

public class ProductsTests : TestContext
{
	[Fact]
	public void Add_To_Cart_Button_Click_Calls_Cart_Service()
	{
		// Arrange
		var mockApiService = new Mock<IProductApiService>();
		mockApiService.Setup(service => service.GetProductsAsync())
			.ReturnsAsync(new List<ProductDto>
			{
				new ProductDto { Id = 1, Title = "Product 1", Price = 10 },
			});

		Services.AddSingleton(mockApiService.Object);

		var cartService = new CartService();
		Services.AddSingleton(cartService);

		// Act
		var cut= RenderComponent<Products>();

		var button = cut.Find("button");
		button.Click();

		//Assert
		var cart = cartService.GetCart();
		cart.Items.Count.Should().Be(1);
		cart.Items.First().Product.Title.Should().Be("Product 1");
	}

	[Fact]
	public void Product_Search_Filters_List()
	{
		// Arrange
		var mockApiService = new Mock<IProductApiService>();
		mockApiService.Setup(s => s.GetProductsAsync()).ReturnsAsync(new List<ProductDto>
		{
			new ProductDto { Id = 1, Title = "Product 1", Price = 10 },
			new ProductDto { Id = 2, Title = "Something Else", Price = 20 }
		});
		Services.AddSingleton(mockApiService.Object);
		Services.AddSingleton(new CartService());

		var cut = RenderComponent<Products>();

		// Act
		var searchBox = cut.Find("input");
		searchBox.Input("Product");

		// Assert
		var rows = cut.FindAll("td");
		rows.Any(td => td.TextContent.Contains("Product 1")).Should().BeTrue();
		rows.Any(td => td.TextContent.Contains("Something Else")).Should().BeFalse();
	}

	[Fact]
	public void Product_Component_Shows_No_Table_When_No_Products()
	{
		//Arrange
		var mockApiService = new Mock<IProductApiService>();
		mockApiService.Setup(s => s.GetProductsAsync()).ReturnsAsync(new List<ProductDto>());
		Services.AddSingleton(mockApiService.Object);
		Services.AddSingleton(new CartService());

		// Act
		var cut = RenderComponent<Products>();

		// Assert
		cut.FindAll("table").Should().BeEmpty();
	}
}