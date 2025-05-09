using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using MyOrderCart.Application.DTOs;
using MyOrderCart.Application.Interfaces;
using MyOrderCart.Application.Options;

namespace MyOrderCart.Infrastructure.Services;

public class ProductApiService : IProductService
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