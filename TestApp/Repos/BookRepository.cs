// TestApp.Data/Repositories/BookRepository.cs
using Microsoft.EntityFrameworkCore;
using TestApp.Data.Models;

namespace TestApp.Data.Repositories
{
    public class BookRepository
    {
        private readonly BookCatalogContext _context;

        public BookRepository(BookCatalogContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Book>> GetAllBooksAsync(int pageNumber, int pageSize)
        {
            return await _context.Books
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> SearchBooksAsync(string searchText, int pageNumber, int pageSize)
        {
            return await _context.Books
                .Where(b => b.Title.Contains(searchText) || b.Author.Contains(searchText) || b.Genre.Contains(searchText))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
 

        public async Task<IEnumerable<Book>> SearchBooksAsync(string searchText)
        {
            return await _context.Books
                .Where(b => b.Title.Contains(searchText) || b.Author.Contains(searchText) || b.Genre.Contains(searchText))
                .ToListAsync();
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task AddBookAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBookAsync(Book book)
        {
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }
    }
}