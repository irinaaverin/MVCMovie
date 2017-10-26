using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using MvcMovie.Infrastructure;
using MvcMovie.Controllers;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Infrastructure.Data;
using System;
using MvcMovie.Core.Models;
using System.Collections.Generic;

namespace MVCMovie.Tests.Controller
{
    public class MovieControllerShould : IClassFixture<DatabaseFixture>
    {
        private readonly IMoviewOrchestrator _orchestrator;
        private readonly MoviesController _controller;
        List<int> _ids = new List<int>();
        DatabaseFixture fixture;
        public MovieControllerShould(DatabaseFixture fixture)
        {
            _orchestrator = fixture.Orchestrator;
            _controller = fixture.Controller;
            _ids = fixture.IDs;
        }

        [Fact]
        public async Task ReturnDataForIndexAsync()
        {
            //Act
            Tuple<List<string>, List<Movie>> tuple = await _orchestrator.FindAllAsync(null, null);
            List<Movie> movies = tuple.Item2;
            //Assert
            Assert.Equal(movies.Count, _ids.Count);
        }
        [Fact]
        public async Task ReturnViewForIndex()
        {
            //Act
            IActionResult result = await _controller.Index(null, null);
            //Assert
            Assert.IsType<ViewResult>(result);
        }
        [Fact]
        public async Task ReturnDataForDetailsOK()
        {
            //Arrange
            int id = _ids[0];
            //Act
            var movie = await _orchestrator.FindAsync(id);
            //Assert
            Assert.Equal(id, movie.ID);
        }
        [Fact]
        public async Task ReturnDataForDetailsNotOK()
        {
            //Arrange
            int id = _ids[_ids.Count-1] + 100;
            //Act
            var movie = await _orchestrator.FindAsync(id);
            //Assert
            Assert.Null(movie);
        }
        [Fact]
        public async Task ReturnViewForDetailsOk()
        {
            //Arrange
            int? id = _ids[0];
            //Act
            IActionResult result = await _controller.Details(id);
            //Assert
            Assert.IsType<ViewResult>(result);
        }
        [Fact]
        public async Task ReturnViewForDetailsNotOk()
        {
            //Arrange
            int? id = 0;
            //Act
            IActionResult result = await _controller.Details(id);
            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

    }
}
