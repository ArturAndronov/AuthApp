using AuthApp.Data;
using AuthApp.Models;
using Microsoft.AspNetCore.Mvc;
using AuthApp.Data; // Замени RegistrationApp на имя твоего проекта
using AuthApp.Models; // Замени RegistrationApp на имя твоего проекта
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class AccountController : Controller
{
    private readonly AppDbContext _context;

    public AccountController(AppDbContext context)
    {
        _context = context;
    }
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(User model)
    {
        if (ModelState.IsValid)
        {
            _context.Users.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Login");
        }
        return View(model);
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        if (user != null)
        {
            // TODO: Реализовать аутентификацию
            return RedirectToAction("Index", "Home");
        }
        ModelState.AddModelError("", "Неверный логин или пароль");
        return View();
    }
}