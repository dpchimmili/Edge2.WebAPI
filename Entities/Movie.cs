using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edge2.WebAPIs.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public int MovieGenreId { get; set; }
        public string Title { get; set; }
        public int NumberInStock { get; set; }
        public decimal DailyRentalRate { get; set; }
    }
}
