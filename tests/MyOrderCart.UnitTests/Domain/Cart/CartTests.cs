using MyOrderCart.Domain.Entities;
namespace MyOrderCart.UnitTests.Domain.Cart;

public class CartTests
{
	[Fact]
	public void Add_New_Product_Should_Create_CartItem_With_Quantity_1()
	{
		// Arrange
		var cart = new MyOrderCart.Domain.Entities.Cart();
		var product = new Product { Id = 1, Title = "Test Product", Price = 10.0m };

		// Act
		cart.AddProduct(product);

		// Assert
		Assert.Single(cart.Items);
		var item = cart.Items.First();
		Assert.Equal(1, item.Quantity);
		Assert.Equal(10.0m, item.TotalPrice);
	}

	[Fact]
	public void Add_Same_Product_Twice_Should_Increment_Quantity()
	{
		//arrange
		var cart = new MyOrderCart.Domain.Entities.Cart();
		var product = new Product { Id = 1, Title = "Test Product", Price = 10.0m };

		//act
		cart.AddProduct(product);
		cart.AddProduct(product);

		//assert
		Assert.Single(cart.Items);
		var item = cart.Items.First();
		Assert.Equal(2, item.Quantity);
		Assert.Equal(20.0m, item.TotalPrice);
	}

	[Fact]
	public void Total_Price_Should_Calculate_Sum_Of_All_Cart_Items()
	{
		//arrange
		var cart = new MyOrderCart.Domain.Entities.Cart();
		var product1 = new Product { Id = 1, Title = "Test Product 1", Price = 5.0m };
		var product2 = new Product { Id = 2, Title = "Test Product 2", Price = 7.5m };

		cart.AddProduct(product1);
		cart.AddProduct(product1);
		cart.AddProduct(product2);

		//act
		decimal totalPrice = cart.TotalPrice;

		//assert
		Assert.Equal(17.5m, totalPrice);
	}

	[Fact]
	public void Remove_Product_Should_Remove_CartItem()
	{
		//arrange
		var cart = new MyOrderCart.Domain.Entities.Cart();
		var product = new Product { Id = 1, Title = "Test Product", Price = 10.0m };
		cart.AddProduct(product);
		Assert.Single(cart.Items);

		//act
		cart.RemoveProduct(product.Id);

		//assert
		Assert.Empty(cart.Items);
		Assert.Equal(0, cart.TotalPrice);
	}

	[Fact]
	public void Add_Null_Product_Should_Throw_ArgumentNullException()
	{
		//arrange
		var cart = new MyOrderCart.Domain.Entities.Cart();

		//act & assert
		Assert.Throws<ArgumentNullException>(() => cart.AddProduct(null));
	}

	[Theory]
	[InlineData(0, 10.0)]    // Invalid Id
	[InlineData(1, 0.0)]     // Invalid Price
	[InlineData(2, -5.0)]    // Negative Price
	public void Add_Invalid_Product_Should_Throw_ArgumentException(int id, decimal price)
	{
		//arrange
		var cart = new MyOrderCart.Domain.Entities.Cart();
		var product = new Product { Id = id, Title = "Test Product", Price = price };


		//act & assert
		Assert.Throws<ArgumentException>(() => cart.AddProduct(product));
	}

	[Fact]
	public void Remove_Non_Existent_Product_Should_Not_Throw_Exception()
	{
		//arrange
		var cart = new MyOrderCart.Domain.Entities.Cart();
		var product = new Product { Id = 1, Title = "Test Product", Price = 10.0m };

		//act 
		var exception = Record.Exception(() => cart.RemoveProduct(product.Id));

		//assert
		Assert.Null(exception);
		Assert.Empty(cart.Items);
	}

	[Fact]
	public void Cart_Total_Should_Be_Zero_When_Empty()
	{
		// Arrange
		var cart = new MyOrderCart.Domain.Entities.Cart();

		// Act
		var total = cart.TotalPrice;

		// Assert
		Assert.Equal(0.0m, total);
	}


	[Fact]
	public void Add_Product_Should_Throw_When_Cart_Is_Confirmed()
	{
		// Arrange
		var cart = new MyOrderCart.Domain.Entities.Cart();
		cart.Confirm(); // lock the cart

		var product = new Product { Id = 1, Title = "Test Product", Price = 10 };

		// Act & Assert
		Assert.Throws<InvalidOperationException>(() => cart.AddProduct(product));
	}

	[Fact]
	public void Remove_Product_Should_Throw_When_Cart_Is_Confirmed()
	{
		// Arrange
		var cart = new MyOrderCart.Domain.Entities.Cart();
		var product = new Product { Id = 1, Title = "Test", Price = 10 };
		cart.AddProduct(product);
		cart.Confirm(); // lock the cart

		// Act & Assert
		Assert.Throws<InvalidOperationException>(() => cart.RemoveProduct(product.Id));
	}


	[Fact]
	public void Decrement_Product_Should_Decrease_Quantity_If_Exists()
	{
		// Arrange
		var cart = new MyOrderCart.Domain.Entities.Cart();
		var product = new Product { Id = 1, Title = "Test Product", Price = 10 };

		cart.AddProduct(product);
		cart.AddProduct(product); 

		// Act
		cart.DecrementProduct(product.Id);

		// Assert
		var item = cart.Items.FirstOrDefault(i => i.Product.Id == product.Id);
		Assert.NotNull(item);
		Assert.Equal(1, item.Quantity);
	}

	[Fact]
	public void Decrement_Product_Should_Remove_Item_When_Quantity_Reaches_Zero()
	{
		// Arrange
		var cart = new MyOrderCart.Domain.Entities.Cart();
		var product = new Product { Id = 1, Title = "Test Product", Price = 10 };
		cart.AddProduct(product); 

		// Act
		cart.DecrementProduct(product.Id);

		// Assert
		Assert.DoesNotContain(cart.Items, i => i.Product.Id == product.Id);
	}
}