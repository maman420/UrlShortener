using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Models;
using UrlShortener.Services;

namespace UrlShortener.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class BlogController : Controller
    {
        private readonly BlogDataService _blogService;

        public BlogController(BlogDataService blogService)
        {
            _blogService = blogService;
        }

        // GET: Customer/Blog
        public async Task<IActionResult> Index()
        {
              return View(_blogService.GetAllPosts());
        }

        // GET: Customer/Blog/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _blogService.GetPost(id) == null)
            {
                return NotFound();
            }

            var blogPostModel = _blogService.GetPost(id);
            if (blogPostModel == null)
            {
                return NotFound();
            }

            return View(blogPostModel);
        }

        // GET: Customer/Blog/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customer/Blog/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content,ImgSrc")] BlogPostModel blogPostModel)
        {
            if (ModelState.IsValid)
            {
                _blogService.AddPost(blogPostModel);
                return RedirectToAction(nameof(Index));
            }
            return View(blogPostModel);
        }

        // GET: Customer/Blog/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var blogPostModel = _blogService.GetPost(id);

            if (id == null || blogPostModel == null)
            {
                return NotFound();
            }

            return View(blogPostModel);
        }

        // POST: Customer/Blog/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,ImgSrc")] BlogPostModel blogPostModel)
        {
            if (id != blogPostModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _blogService.UpdatePost(id, blogPostModel);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(blogPostModel);
        }

        // GET: Customer/Blog/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {

            var blogPostModel = _blogService.GetPost(id);
            if (id == null || blogPostModel == null)
            {
                return NotFound();
            }

            return View(blogPostModel);
        }

        // POST: Customer/Blog/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var blogPostModel = _blogService.GetPost(id);
            if (blogPostModel == null)
            {
                return Problem("Entity set 'DataContext.BlogPostModel'  is null.");
            }
            if (blogPostModel != null)
            {
                _blogService.DeletePost(id);
                ViewData["Success"] = "item deleted succesfully";
            }
            
            return RedirectToAction("Index");
        }
    }
}
