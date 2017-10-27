using Microsoft.EntityFrameworkCore;
using MvcMovie.Controllers;
using MvcMovie.Infrastructure;
using MvcMovie.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace MVCMovie.Tests
{
    public class DatabaseFixture : IDisposable
    {
        public DatabaseFixture()
        {
            var builder = new DbContextOptionsBuilder<MvcMovieContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            var options = builder.Options;
            MvcMovieContext dbContext = new MvcMovieContext(options);

            dbContext.AddRange(SeedData.AddTestData());
            dbContext.SaveChanges();

            Orchestrator = new MoviewOrchestrator(dbContext);
            Controller = new MoviesController(Orchestrator);

            PopulateFromInMemoryIds();
        }
        private async Task PopulateFromInMemoryIds()
        {
            var movies = await Orchestrator.GetAllAsync();
            IDs = movies.Select(x => x.ID).ToList();
        }
        public void Dispose()
        {
            // ... clean up test data from the database ...
            Controller.Dispose();
            Orchestrator = null;
            IDs = null;
        }
        public IMoviewOrchestrator Orchestrator { get; private set; }
        public MoviesController Controller { get; private set; }
        public List<int> IDs  { get; private set; }
}
}
