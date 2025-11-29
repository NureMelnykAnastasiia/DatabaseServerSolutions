using BookStore.Models;
using BookStore.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace BookStore.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly BookRepository _repo;

    public BooksController(BookRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public async Task<IActionResult> GetBooks()
    {
        var books = await _repo.GetAllBooksAsync();
        return Ok(books);
    }

    [HttpGet("{id}/sales")]
    public async Task<IActionResult> GetBookSales(int id)
    {
        var sales = await _repo.GetSalesByBookIdAsync(id);
        return Ok(sales);
    }
    
    [HttpPost("discount")]
    public async Task<IActionResult> ApplyDiscount([FromBody] DiscountRequest request)
    {
        try
        {
            await _repo.DiscountBooksAsync(request.GenreName, request.Percent);
            return Ok(new { Message = "Ціни успішно оновлено!" });
        }
        catch (SqlException ex)
        {
            return BadRequest(new ErrorResponse { Message = ex.Message });
        }
    }
    
    [HttpGet("max-check")]
    public async Task<IActionResult> GetMaxCheck([FromQuery] string publisher)
    {
        var result = await _repo.GetMaxCheckByPublisherAsync(publisher);
        return Ok(new { Result = result });
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateBook([FromBody] Book book)
    {
        try
        {
            await _repo.AddBookAsync(book);
            return Ok(new { Message = "Книгу додано!" });
        }
        catch (SqlException ex)
        {
            return BadRequest(new ErrorResponse { Message = $"SQL Error: {ex.Message}" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponse { Message = ex.Message });
        }
    }
}