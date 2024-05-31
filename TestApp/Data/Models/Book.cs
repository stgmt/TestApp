namespace TestApp.Data.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public string ISBN { get; set; }
        public byte[]? CoverImage { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public int ReleaseYear { get; internal set; }
    }
}
