using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using TestApp.Data.Models;
using TestApp.Data.Repositories;

namespace TestApp.Services
{
    public class BookService
    {
        private readonly BookRepository _bookRepository;

        public BookService(BookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        

        public async Task<IEnumerable<Book>> GetAllBooksAsync(int pageNumber, int pageSize)
        {
            return await _bookRepository.GetAllBooksAsync(pageNumber, pageSize);
        }

        public async Task<IEnumerable<Book>> SearchBooksAsync(string searchText, int pageNumber, int pageSize)
        {
            return await _bookRepository.SearchBooksAsync(searchText, pageNumber, pageSize);
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            return await _bookRepository.GetBookByIdAsync(id);
        }

        public async Task AddBookAsync(Book book)
        {
            await _bookRepository.AddBookAsync(book);
        }

        public async Task UpdateBookAsync(Book book)
        {
            await _bookRepository.UpdateBookAsync(book);
        }

        public async Task DeleteBookAsync(int id)
        {
            await _bookRepository.DeleteBookAsync(id);
        }
    }
}
