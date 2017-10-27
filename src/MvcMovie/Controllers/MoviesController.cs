using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Core.Models;
using MvcMovie.Infrastructure.Data;
using MvcMovie.Infrastructure;
using MvcMovie.ViewModels;

namespace MvcMovie.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMoviewOrchestrator _orchestrator;

        public MoviesController(IMoviewOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
        }
        // GET: Movies
        public async Task<IActionResult> Index(string movieGenre, string searchString)
        {
            var tuple = await _orchestrator.FindAllAsync(movieGenre, searchString);
            var movieGenreVM = new MovieGenreViewModel
            {
                genres = new SelectList(tuple.Item1),
                movies = tuple.Item2
            };
            return View(movieGenreVM);
        }
        //[HttpPost]
        //public IActionResult Index(string searchString, bool notUsed)
        //{
        //    return Ok("From [HttpPost]Index: filter on " + searchString);
        //}

        //// GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var movie = await _orchestrator.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        //// GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        //// POST: Movies/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                await _orchestrator.AddAsync(movie);
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        //// GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var movie = await _orchestrator.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        //// POST: Movies/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
        {
            if (id != movie.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _orchestrator.UpdateAsync(id, movie);
                if (result == 0)
                {
                    return NotFound();
                }

                return RedirectToAction("Index");
            }
            return View(movie);
        }

        //// GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var movie = await _orchestrator.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        //// POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _orchestrator.DeleteAsync(id);
            return RedirectToAction("Index");
        }


    }
    
}
