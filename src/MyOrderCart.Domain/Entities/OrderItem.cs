﻿namespace MyOrderCart.Domain.Entities;

public class OrderItem
{
	public int Id { get; set; }
	public int ProductId { get; set; }
	public string Title { get; set; } = string.Empty;
	public decimal Price { get; set; }
	public int Quantity { get; set; }
	public int OrderId { get; set; }
	public Order? Order { get; set; }
}