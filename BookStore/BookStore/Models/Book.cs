namespace BookStore.Models;

public class Book
{
    public int Book_Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock_Qty { get; set; }
    public int Pub_Id { get; set; }
    public int Genre_Id { get; set; }
    public int Author_Id { get; set; }
    public int Publish_Year { get; set; }
    
    public string? PublisherName { get; set; }
    public string? GenreName { get; set; }
    public string? AuthorName { get; set; }
}