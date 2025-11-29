namespace BookStore.Models;

public class Sale
{
    public int Sale_Id { get; set; }
    public int Check_No { get; set; }
    public DateTime Date_Sale { get; set; }
    public int Quantity { get; set; }
    public decimal Total_Price { get; set; }
}