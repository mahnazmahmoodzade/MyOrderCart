﻿namespace MyOrderCart.Domain.Entities;

public class Order
{
	public int Id { get; set; }
	public decimal TotalPrice { get; set; }
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	public List<OrderItem> Items { get; set; } = new();
}