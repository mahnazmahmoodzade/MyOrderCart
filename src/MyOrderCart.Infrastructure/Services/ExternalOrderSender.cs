using System.Text.Json;
using Microsoft.Extensions.Options;
using MyOrderCart.Application.Interfaces;
using MyOrderCart.Application.Options;
using MyOrderCart.Domain.Entities;

namespace MyOrderCart.Infrastructure.Services;

public class ExternalOrderSender: IExternalOrderSender
{

	private readonly HttpClient _httpClient;
	private readonly string _endpoint;

	public ExternalOrderSender(HttpClient httpClient,IOptions<OrderOptions> options)
	{
		_httpClient = httpClient;
		_endpoint = options.Value.Endpoint;	
	}

	public async Task<bool> SendOrderAsync(Cart cart, CancellationToken cancellationToken = default)
	{
		var payload = new
		{
			OrderStatus = "Confirmed",
			TotalPrice = cart.TotalPrice,
			Products = cart.Items.Select(i => new
			{
				Id = i.Product.Id,
				Title = i.Product.Title,
				Price = i.Product.Price,
				Quantity  = i.Quantity,
			}),
			
		};

		var json = JsonSerializer.Serialize(payload);
		var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

		try
		{
			var response = await _httpClient.PostAsync(_endpoint, content, cancellationToken);
			return response.IsSuccessStatusCode;
		}

		catch (TaskCanceledException)
		{
			// timeout 
			return false;
		}

		catch
		{
			//TODO: Log exception
			return false;
		}
	}
}