using BhokMandu.Models;

public class Order
{
    public int Id { get; set; }
    public string? CustomerName { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    // Foriegn key to the User Table
    public int UserId { get; set; }
    // Navigation property to User
    public User User { get; set; }
    public List<OrderItem> Items { get; set; } = new List<OrderItem>();
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