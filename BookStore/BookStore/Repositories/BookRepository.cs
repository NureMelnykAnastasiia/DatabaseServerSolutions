using System.Data;
using BookStore.Data;
using BookStore.Models;
using Dapper;

namespace BookStore.Repositories;

public class BookRepository
    {
        private readonly DapperContext _context;

        public BookRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            var query = @"
                SELECT b.*, p.NAME as PublisherName, g.NAME as GenreName, a.FULL_NAME as AuthorName
                FROM Books b
                LEFT JOIN Publishers p ON b.PUB_ID = p.PUB_ID
                LEFT JOIN Genres g ON b.GENRE_ID = g.GENRE_ID
                LEFT JOIN Authors a ON b.AUTHOR_ID = a.AUTHOR_ID";

            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Book>(query);
        }

        public async Task<IEnumerable<Sale>> GetSalesByBookIdAsync(int bookId)
        {
            var query = "SELECT * FROM Sales WHERE BOOK_ID = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Sale>(query, new { Id = bookId });
        }

        public async Task DiscountBooksAsync(string genre, decimal percent)
        {
            using var connection = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("GenreName", genre);
            parameters.Add("Percent", percent);
            
            await connection.ExecuteAsync("DiscountBooksByGenre", parameters, commandType: CommandType.StoredProcedure);
        }
        
        public async Task<string> GetMaxCheckByPublisherAsync(string publisherName)
        {
            using var connection = _context.CreateConnection();
            var query = "SELECT dbo.GetMaxCheckByPublisher(@PublisherName)";
            return await connection.ExecuteScalarAsync<string>(query, new { PublisherName = publisherName });
        }
        
        public async Task AddBookAsync(Book book)
        {
            var query = @"
                INSERT INTO Books (TITLE, PRICE, STOCK_QTY, PUB_ID, GENRE_ID, AUTHOR_ID, PUBLISH_YEAR)
                VALUES (@Title, @Price, @Stock_Qty, @Pub_Id, @Genre_Id, @Author_Id, @Publish_Year)";

            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, book);
        }
    }