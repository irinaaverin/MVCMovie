using Microsoft.EntityFrameworkCore;
using MvcMovie.Core.Models;
using MvcMovie.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MvcMovie.Infrastructure
{
    public class MoviewOrchestrator : IMoviewOrchestrator
    {
        private MvcMovieContext _context;

        public MoviewOrchestrator(MvcMovieContext context)
        {
            _context = context;
        }
        public async Task<List<Movie>> GetAllAsync()
        {
            return await _context.Movie.ToListAsync();

        }

        public async Task<List<Movie>> FindAllAsync(string searchString)
        {
            var movies = from m in _context.Movie
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title.Contains(searchString));
            }

            return await movies.ToListAsync();
        }
        public async Task<Tuple<List<string>, List<Movie>>> FindAllAsync(string movieGenre, string searchString)
        {
            // Use LINQ to get list of genres.
            IQueryable<string> genreQuery = from m in _context.Movie
                                            orderby m.Genre
                                            select m.Genre;
            var movies = from m in _context.Movie
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(movieGenre))
            {
                movies = movies.Where(x => x.Genre == movieGenre);
            }
            return new Tuple<List<string>, List<Movie>>(await genreQuery.Distinct().ToListAsync(), await movies.ToListAsync());

        }
        public async Task<Movie> FindAsync(int? id)
        {
            if (id == null)
            {
                return null;
            }
            return await _context.Movie
                .SingleOrDefaultAsync(m => m.ID == id);
        }
        public async Task<int> AddAsync(Movie movie)
        {
            _context.Add(movie);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> UpdateAsync(int id, Movie movie)
        {
            try
            {
                Movie movieOrig = await FindAsync(id);
                movieOrig.Title = movie.Title;
                //_context.Update(movie);
                return await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(movie.ID))
                {
                    return 0;
                }
                else
                {
                    throw;
                }
            }
        }
        public async Task<int> DeleteAsync(int? id)
        {
            var movie = await _context.Movie.SingleOrDefaultAsync(m => m.ID == id);
            _context.Movie.Remove(movie);
            return await _context.SaveChangesAsync();
        }
        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.ID == id);
        }
        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null) _context.Dispose();
            }
        }
        #endregion
        public async Task<IEnumerable<Movie>> GetAsync(
            Func<IQueryable<Movie>, IQueryable<Movie>> queryShaper,
            CancellationToken cancellationToken
            )
        {
            var query = queryShaper(_context.Movie);
            return await query.ToArrayAsync(cancellationToken);
        }
        public async Task<TResult> GetAsync<TResult>(
           Func<IQueryable<Movie>, TResult> queryShaper,
           CancellationToken cancellationToken
           )
        {
            var set = _context.Movie;
            var query = queryShaper;
            var factory = Task<TResult>.Factory;
            return await factory.StartNew(() => query(set), cancellationToken);
        }
    }

}
