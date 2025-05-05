using System.Text.Json;
using MyOrderCart.Application.Interfaces;
using MyOrderCart.Domain.Entities;

namespace MyOrderCart.Infrastructure.Services;

public class ExternalOrderSender: IExternalOrderSender
{

	private readonly HttpClient _httpClient;
	private readonly string _orderEndpoint= "https://myordercart.requestcatcher.com";

	public ExternalOrderSender(HttpClient httpClient)
	{
		_httpClient = httpClient;
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
			var response = await _httpClient.PostAsync(_orderEndpoint, content, cancellationToken);
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