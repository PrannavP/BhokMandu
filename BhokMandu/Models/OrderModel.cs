public class Order
{
    public int Id { get; set; }
    public string? CustomerName { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public List<OrderItem> Items { get; set; }
}

public enum OrderStatus
{
    Pending = 1,
    Confirmed = 2,
    InProgress = 3,
    Shipped = 4,
    Delivered = 5,
    Cancelled = 6,
    Returned = 7
}

public class OrderItem
{
    public int Id { get; set; }
    public string? FoodName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}