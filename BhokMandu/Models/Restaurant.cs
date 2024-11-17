namespace BhokMandu.Models;

public class Restaurant
{
    public int Id { get; set; }
    public string? RestaurantName { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? OpeningHours { get; set; }
    public string? Rating { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}