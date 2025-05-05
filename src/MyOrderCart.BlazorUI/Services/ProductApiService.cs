using MyOrderCart.BlazorUI.Models;

namespace MyOrderCart.BlazorUI.Services;

public class ProductApiService : IProductApiService
{
	private readonly HttpClient _httpClient;

	public ProductApiService(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<List<ProductDto>> GetProductsAsync()
	{
		var response = await _httpClient.GetFromJsonAsync<List<ProductDto>>("https://fakestoreapi.com/products");
		return response ?? new List<ProductDto>();
	}
}