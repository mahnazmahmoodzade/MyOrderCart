using Microsoft.Extensions.Options;
using MyOrderCart.Application.Options;
using MyOrderCart.BlazorUI.Models;

namespace MyOrderCart.BlazorUI.Services;

public class ProductApiService : IProductApiService
{
	private readonly HttpClient _httpClient;
	private readonly string _endpoint;

	public ProductApiService(HttpClient httpClient, IOptions<ProductApiOptions> options)
	{
		_httpClient = httpClient;
		_endpoint = options.Value.Endpoint;
	}

	public async Task<List<ProductDto>> GetProductsAsync()
	{
		var response = await _httpClient.GetFromJsonAsync<List<ProductDto>>(_endpoint);
		return response ?? new List<ProductDto>();
	}
}