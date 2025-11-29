namespace BookStore.Models;

public class DiscountRequest
{
    public string GenreName { get; set; } = string.Empty;
    public decimal Percent { get; set; }
}