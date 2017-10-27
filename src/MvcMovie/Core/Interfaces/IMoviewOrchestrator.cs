using MvcMovie.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovie.Infrastructure
{
    public interface IMoviewOrchestrator    //:IDisposable
    {
        Task<List<Movie>> GetAllAsync();
        Task<List<Movie>> FindAllAsync(string searchString);
        Task<Tuple<List<string>, List<Movie>>> FindAllAsync(string movieGenre, string searchString);
        Task<Movie> FindAsync(int? id);
        Task<int> AddAsync(Movie movie);
        Task<int> UpdateAsync(int id, Movie movie);
        Task<int> DeleteAsync(int? id);
    }
}
