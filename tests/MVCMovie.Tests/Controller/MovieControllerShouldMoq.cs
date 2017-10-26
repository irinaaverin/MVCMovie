using Microsoft.AspNetCore.Mvc;
using Moq;
using MvcMovie.Controllers;
using MvcMovie.Core.Models;
using MvcMovie.Infrastructure;
using MvcMovie.Infrastructure.Data;
using MvcMovie.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MVCMovie.Tests.Controller
{
    public class MovieControllerShouldMoq
    {
        List<Movie> _movies = new List<Movie>();
        public MovieControllerShouldMoq()
        {            
            _movies.AddRange(SeedData.AddTestData());
            int id = 1;
            foreach(Movie item in _movies)
            {
                item.ID = id;
                id++;
            }
        }
        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfMovies()
        {
            // Arrange
            var mockRepo = new Mock<IMoviewOrchestrator>();
            mockRepo.Setup(repo => repo.FindAllAsync(null,null)).Returns(Task.FromResult(GetTestMoviesWithGenres()));
            var controller = new MoviesController(mockRepo.Object);

            // Act
            var result = await controller.Index(null,null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<MovieGenreViewModel>(viewResult.ViewData.Model);
            Assert.Equal(4, model.movies.Count());
        }
        [Fact]
        public async Task Edit_Get_ReturnsAViewResult_WithAMovie()
        {
            // Arrange
            var mockRepo = new Mock<IMoviewOrchestrator>();
            mockRepo.Setup(repo => repo.FindAsync(1)).Returns(Task.FromResult(_movies[0]));
            var controller = new MoviesController(mockRepo.Object);

            // Act
            var result = await controller.Edit(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<Movie>(
                viewResult.ViewData.Model);
            Assert.Equal(_movies[0].Title, model.Title);
        }
        [Fact]
        public async Task Edit_Post_ReturnsBadRequestResult_WhenModelStateIsInvalid()
        {
            // Arrange
            var mockRepo = new Mock<IMoviewOrchestrator>();
            int updateAsyncResultBad = 0;
            var newMovie = _movies[0];
            newMovie.Title = "A";

            mockRepo.Setup(repo => repo.UpdateAsync(1, newMovie)).Returns(Task.FromResult(updateAsyncResultBad));
            var controller = new MoviesController(mockRepo.Object);
            controller.ModelState.AddModelError("Title", "Required");


            // Act
            var result = await controller.Edit(1, newMovie);

            // Assert
            Assert.IsNotType<RedirectToActionResult>(result);
            Assert.IsNotType<NotFoundResult>(result);
            mockRepo.Verify();

        }
        [Fact]
        public async Task Edit_Post_ReturnsBadRequestResult_FailedSave()
        {
            // Arrange
            var mockRepo = new Mock<IMoviewOrchestrator>();
            var newMovie = _movies[0];
            newMovie.Title += " A test";
            int updateAsyncResultBad = 0;
            mockRepo.Setup(repo => repo.UpdateAsync(1,newMovie)).Returns(Task.FromResult(updateAsyncResultBad));
            var controller = new MoviesController(mockRepo.Object);

            // Act
            var result = await controller.Edit(1, newMovie);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            mockRepo.Verify();
        }

        [Fact]
        public async Task Edit_Post_ReturnsGoodRequestResult()
        {
            // Arrange
            int updateAsyncResultGood = 1;
            var mockRepo = new Mock<IMoviewOrchestrator>();
            var newMovie = _movies[0];
            newMovie.Title += " A test";

            mockRepo.Setup(repo => repo.UpdateAsync(1, newMovie)).Returns(Task.FromResult(updateAsyncResultGood));
            var controller = new MoviesController(mockRepo.Object);

            // Act
            var result = await controller.Edit(1, newMovie);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);

            mockRepo.Verify();

        }
        private Tuple<List<string>, List<Movie>> GetTestMoviesWithGenres()
        {
            var genreQuery = _movies.Select(x => x.Genre).Distinct().ToList();
                                            
            return new Tuple<List<string>, List<Movie>>(genreQuery, _movies);

           
        }
    }
}

