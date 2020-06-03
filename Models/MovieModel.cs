namespace Edge2.WebAPIs.Models
{
    public class MovieModel
    {
        public int _id { get; set; }

        public string title { get; set; }
        public int numberInStock { get; set; }
        public decimal dailyRentalRate { get; set; }
        public MovieGenreModel genre { get; set; }
        public int genreId { get; set; }
    }
}
