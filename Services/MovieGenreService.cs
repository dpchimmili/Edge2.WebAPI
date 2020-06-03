using Edge2.WebAPIs.Entities;
using Edge2.WebAPIs.Helpers;
using Edge2.WebAPIs.Models;
using System.Collections.Generic;
using System.Linq;

namespace Edge2.WebAPIs.Services
{
    public interface IMovieGenresService
    {
        IEnumerable<MovieGenreModel> GetAll();
        MovieGenreModel GetSingle(int id);
        bool AddMovieGenre(MovieGenreModel movie);
        bool UpdateMovieGenre(MovieGenreModel movie);
        bool DeleteMovieGenre(int movieId);
    }

    public class MovieGenresService : IMovieGenresService
    {
        private DataContext _context;
        public MovieGenresService(DataContext context)
        {
            _context = context;
        }
        public IEnumerable<MovieGenreModel> GetAll()
        {
            var genres = new List<MovieGenreModel>();
            foreach (var genre in _context.MovieGenres.OrderBy(a => a.Id))
            {
                var gen = new MovieGenreModel
                {
                    _id = genre.Id,
                    name = genre.Name,
                };

                genres.Add(gen);
            }
            return genres;
        }

        public MovieGenreModel GetSingle(int id)
        {
            var gen = _context.MovieGenres.Find(id);
            var genre = new MovieGenreModel
            {
                _id = gen.Id,
                name = gen.Name,
            };
            return genre;
        }
        public bool AddMovieGenre(MovieGenreModel movieGenre)
        {
            var newId = _context.MovieGenres.OrderByDescending(a => a.Id).FirstOrDefault().Id + 1;
            _context.MovieGenres.Add(new MovieGenre { Id = movieGenre._id, Name = movieGenre.name });
            return true;
        }
        public bool UpdateMovieGenre(MovieGenreModel movieGenre)
        {
            var updateMovieGenre = _context.MovieGenres.OrderByDescending(a => a.Id == movieGenre._id).FirstOrDefault();
            updateMovieGenre.Id = movieGenre._id;
            updateMovieGenre.Name = movieGenre.name;
            _context.MovieGenres.Update(updateMovieGenre);
            return true;
        }
        public bool DeleteMovieGenre(int movieGenreId)
        {
            var updateMovie = _context.MovieGenres.OrderByDescending(a => a.Id == movieGenreId).FirstOrDefault();
            _context.MovieGenres.Remove(updateMovie);
            return true;
        }

    }
}
