using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TestApp.Data.Models;
using TestApp.Services;

namespace TestApp
{
    public partial class AddEditBookWindow : Window
    {
        private readonly BookService _bookService;
        private readonly Book _book;

        public AddEditBookWindow(BookService bookService, Book book = null)
        {
            InitializeComponent();
            _bookService = bookService;
            _book = book;

            if (_book != null)
            {
                TitleTextBox.Text = _book.Title;
                AuthorTextBox.Text = _book.Author;
                YearTextBox.Text = _book.ReleaseYear.ToString();
                IsbnTextBox.Text = _book.ISBN;
                DescriptionTextBox.Text = _book.Description;
                GenreTextBox.Text = _book.Genre;
            }
            else
            {
                TitleTextBox.Text = "Title";
                AuthorTextBox.Text = "Author";
                YearTextBox.Text = "Release Year";
                IsbnTextBox.Text = "ISBN";
                DescriptionTextBox.Text = "Description";
                GenreTextBox.Text = "Genre";

                TitleTextBox.Foreground = new SolidColorBrush(Colors.Gray);
                AuthorTextBox.Foreground = new SolidColorBrush(Colors.Gray);
                YearTextBox.Foreground = new SolidColorBrush(Colors.Gray);
                IsbnTextBox.Foreground = new SolidColorBrush(Colors.Gray);
                DescriptionTextBox.Foreground = new SolidColorBrush(Colors.Gray);
                GenreTextBox.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidateInput())
                {
                    MessageBox.Show("Please fill in all fields correctly.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var book = _book ?? new Book
                {
                    Title = TitleTextBox.Text,
                    Author = AuthorTextBox.Text,
                    ReleaseYear = int.Parse(YearTextBox.Text),
                    ISBN = IsbnTextBox.Text,
                    Description = DescriptionTextBox.Text,
                    Genre = GenreTextBox.Text
                };

                if (_book == null)
                {
                    await _bookService.AddBookAsync(book);
                }
                else
                {
                    book.Id = _book.Id;
                    book.CoverImage = _book.CoverImage;
                    await _bookService.UpdateBookAsync(book);
                }

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving the book: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CoverImageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    var filePath = openFileDialog.FileName;
                    var imageData = File.ReadAllBytes(filePath);
                    if (_book != null)
                    {
                        _book.CoverImage = imageData;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading the cover image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null && (textBox.Text == "Title" || textBox.Text == "Author" || textBox.Text == "Release Year" || textBox.Text == "ISBN" || textBox.Text == "Description" || textBox.Text == "Genre"))
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
                if (textBox.Name == "TitleTextBox") textBox.Text = "Title";
                else if (textBox.Name == "AuthorTextBox") textBox.Text = "Author";
                else if (textBox.Name == "YearTextBox") textBox.Text = "Release Year";
                else if (textBox.Name == "IsbnTextBox") textBox.Text = "ISBN";
                else if (textBox.Name == "DescriptionTextBox") textBox.Text = "Description";
                else if (textBox.Name == "GenreTextBox") textBox.Text = "Genre";
                textBox.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text) || TitleTextBox.Text == "Title") return false;
            if (string.IsNullOrWhiteSpace(AuthorTextBox.Text) || AuthorTextBox.Text == "Author") return false;
            if (!int.TryParse(YearTextBox.Text, out int _)) return false;
            if (string.IsNullOrWhiteSpace(IsbnTextBox.Text) || IsbnTextBox.Text == "ISBN") return false;
            if (string.IsNullOrWhiteSpace(DescriptionTextBox.Text) || DescriptionTextBox.Text == "Description") return false;
            if (string.IsNullOrWhiteSpace(GenreTextBox.Text) || GenreTextBox.Text == "Genre") return false;

            return true;
        }
    }
}
