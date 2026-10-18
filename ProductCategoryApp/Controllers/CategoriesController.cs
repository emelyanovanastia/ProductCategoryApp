using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCategoryApp.Data;
using ProductCategoryApp.Models;

namespace ProductCategoryApp.Controllers;

public class CategoriesController : Controller   //Контроллер списка категорий
{
    private readonly AppDbContext _db;   //Связь с контекстом БД 

    public CategoriesController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()  //Асихронный метод к таблице категорий Categories
    {
        var categories = await _db.Categories
            .OrderBy(c => c.Name)
            .ToListAsync();

        return View(categories);
    }

    [HttpGet]    //Метод get, чтобы показать пустую форму
    public IActionResult Create()
    {
        return View();
    }


    [HttpPost]    //Метод post, чтобы принять форму с данными и сохранить в БД
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name")] Category category)
    {
        if (ModelState.IsValid)
        {
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(category);
    }
}