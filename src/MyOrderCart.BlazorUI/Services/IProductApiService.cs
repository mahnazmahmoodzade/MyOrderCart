using MyOrderCart.BlazorUI.Models;

namespace MyOrderCart.BlazorUI.Services;

public interface IProductApiService
{
	Task<List<ProductDto>> GetProductsAsync();
}