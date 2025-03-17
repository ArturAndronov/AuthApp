using Microsoft.AspNetCore.Mvc;
using AuthApp.Data;
using AuthApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AuthApp.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly AppDbContext _context;

        public PostController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Edit(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if(post == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (post.UserId.ToString() != userId)
            {
                return Forbid(); 
            }
            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Post updatedPost)
        {
            if (id != updatedPost.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var post = await _context.Posts.FindAsync(id);
                if (post == null)
                {
                    return NotFound();
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (post.UserId.ToString() != userId)
                {
                    return Forbid();
                }

                post.Title = updatedPost.Title;
                post.Content = updatedPost.Content;

                _context.Posts.Update(post);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(updatedPost);
        }

        public IActionResult Index()
        {
            var posts = _context.Posts.Include(p => p.User).ToList();
            return View(posts);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Post post)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (int.TryParse(userId, out int authorId))
                {
                    post.UserId = authorId;
                    _context.Posts.Add(post);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return Unauthorized();
                }
            }
            return View(post);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}