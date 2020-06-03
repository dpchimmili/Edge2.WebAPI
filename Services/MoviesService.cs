using Edge2.WebAPIs.Entities;
using Edge2.WebAPIs.Helpers;
using Edge2.WebAPIs.Models;
using System.Collections.Generic;
using System.Linq;

namespace Edge2.WebAPIs.Services
{
    public interface IMoviesService
    {
        IEnumerable<MovieModel> GetAll();
        MovieModel GetSingle(int id);
        bool AddMovie(MovieModel movie);
        bool UpdateMovie(MovieModel movie);
        bool DeleteMovie(int movieId);
    }

    public class MoviesService : IMoviesService
    {
        private DataContext _context;
        public MoviesService(DataContext context)
        {
            _context = context;
        }
        public IEnumerable<MovieModel> GetAll()
        {
            var movies = new List<MovieModel>();
            foreach (var movie in _context.Movies.OrderBy(a => a.Id))
            {
                var mov = new MovieModel
                {
                    _id = movie.Id,
                    title = movie.Title,
                    numberInStock = movie.NumberInStock,
                    dailyRentalRate = movie.DailyRentalRate,
                    genre = new MovieGenreModel
                    {
                        _id = movie.MovieGenreId,
                        name = _context.MovieGenres.FirstOrDefault(b => b.Id == movie.MovieGenreId).Name,
                    }
                };

                movies.Add(mov);
            }
            return movies;
        }

        public MovieModel GetSingle(int id)
        {
            var mov = _context.Movies.Find(id);
            var movie = new MovieModel
            {
                _id = mov.Id,
                title = mov.Title,
                numberInStock = mov.NumberInStock,
                dailyRentalRate = mov.DailyRentalRate,
                genre = new MovieGenreModel
                {
                    _id = mov.MovieGenreId,
                    name = _context.MovieGenres.FirstOrDefault(b => b.Id == mov.MovieGenreId).Name,
                }
            };
            return movie;
        }
        public bool AddMovie(MovieModel movie)
        {
            var newId = _context.Movies.OrderByDescending(a => a.Id).FirstOrDefault().Id + 1;
            _context.Movies.Add(new Movie { Id = movie._id, MovieGenreId = movie.genreId, Title = movie.title, NumberInStock = movie.numberInStock, DailyRentalRate = movie.dailyRentalRate });
            _context.SaveChanges();
            return true;
        }
        public bool UpdateMovie(MovieModel movie)
        {
            var updateMovie = _context.Movies.OrderByDescending(a => a.Id == movie._id).FirstOrDefault();
            updateMovie.Id = movie._id;
            updateMovie.Title = movie.title;
            updateMovie.NumberInStock = movie.numberInStock;
            updateMovie.DailyRentalRate = movie.dailyRentalRate;
            updateMovie.MovieGenreId = movie.genreId;
            _context.Movies.Update(updateMovie);
            _context.SaveChanges();
            return true;
        }
        public bool DeleteMovie(int movieId)
        {
            var updateMovie = _context.Movies.OrderByDescending(a => a.Id == movieId).FirstOrDefault();
            _context.Movies.Remove(updateMovie);
            _context.SaveChanges();
            return true;
        }

    }
}
