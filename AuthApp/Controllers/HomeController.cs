using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AuthApp.Models;
using AuthApp.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;

    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        List<Post> posts = _context.Posts.Include(p => p.Author).ToList();
        return View(posts);
    }

    public IActionResult Privacy()
    {
        return View();
    }
    public IActionResult CreatePost()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost]
    public async Task<IActionResult> DeletePost(int id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post != null)
        {
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }
    [HttpPost]
    public async Task<IActionResult> CreatePost(Post post)
    {
      
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized();
        }

        post.AuthorId = int.Parse(userIdClaim.Value); 

        if (ModelState.IsValid)
        {
            var author = await _context.Users.FindAsync(post.AuthorId);
            if (author == null)
            {
                ModelState.AddModelError("AuthorId", "Автор не найден.");
                return View(post);
            }

            post.Author = author;

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        return View(post);
    }
}
