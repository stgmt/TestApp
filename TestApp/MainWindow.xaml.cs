using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TestApp.Data.Models;
using TestApp.Services;

namespace TestApp
{
    public partial class MainWindow : Window
    {
        private readonly BookService _bookService;
        private ObservableCollection<Book> _books;
        private int _pageNumber = 1;
        private int _pageSize = 10; // Choose an appropriate page size

        public MainWindow(BookService bookService)
        {
            _bookService = bookService;
            InitializeComponent();
            LoadBooks();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null && (textBox.Text == "Search..." || textBox.Text == "Title" || textBox.Text == "Author" || textBox.Text == "Release Year" || textBox.Text == "ISBN" || textBox.Text == "Description" || textBox.Text == "Genre"))
            {
                textBox.Text = string.Empty;
                textBox.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null && string.IsNullOrWhiteSpace(textBox.Text))
            {
                if (textBox.Name == "SearchTextBox") textBox.Text = "Search...";
                else if (textBox.Name == "TitleTextBox") textBox.Text = "Title";
                else if (textBox.Name == "AuthorTextBox") textBox.Text = "Author";
                else if (textBox.Name == "YearTextBox") textBox.Text = "Release Year";
                else if (textBox.Name == "IsbnTextBox") textBox.Text = "ISBN";
                else if (textBox.Name == "DescriptionTextBox") textBox.Text = "Description";
                else if (textBox.Name == "GenreTextBox") textBox.Text = "Genre";
                textBox.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        private async void LoadBooks()
        {
            try
            {
                _books = new ObservableCollection<Book>(await _bookService.GetAllBooksAsync(_pageNumber, _pageSize));
                BooksDataGrid.ItemsSource = _books;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading books: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var searchText = SearchTextBox.Text == "Search..." ? string.Empty : SearchTextBox.Text;
                _books = new ObservableCollection<Book>(await _bookService.SearchBooksAsync(searchText, _pageNumber, _pageSize));
                BooksDataGrid.ItemsSource = _books;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while searching for books: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            _pageNumber++;
            LoadBooks();
        }

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (_pageNumber > 1)
            {
                _pageNumber--;
                LoadBooks();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var addBookWindow = new AddEditBookWindow(_bookService);
                addBookWindow.ShowDialog();
                LoadBooks();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while adding a book: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedBook = BooksDataGrid.SelectedItem as Book;
            if (selectedBook == null) return;

            try
            {
                var editBookWindow = new AddEditBookWindow(_bookService, selectedBook);
                editBookWindow.ShowDialog();
                LoadBooks();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while editing the book: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedBook = BooksDataGrid.SelectedItem as Book;
            if (selectedBook == null) return;

            try
            {
                await _bookService.DeleteBookAsync(selectedBook.Id);
                LoadBooks();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while deleting the book: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BooksDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EditButton.IsEnabled = BooksDataGrid.SelectedItem != null;
            DeleteButton.IsEnabled = BooksDataGrid.SelectedItem != null;
        }

        private void BooksDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            // Customize the row loading logic if necessary
            var row = e.Row;
            var book = row.Item as Book;
            if (book != null && book.ReleaseYear < 2000)
            {
                row.Background = new SolidColorBrush(Colors.LightGray);
            }
        }
    }
}
