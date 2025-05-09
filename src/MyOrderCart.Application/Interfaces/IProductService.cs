
using MyOrderCart.Application.DTOs;

namespace MyOrderCart.Application.Interfaces;

public interface IProductService
{
	Task<List<ProductDto>> GetProductsAsync();
}