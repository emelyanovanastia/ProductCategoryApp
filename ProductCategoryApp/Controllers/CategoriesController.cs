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

    //Добавляем метод для редактирования записей

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var category = await _db.Categories.FindAsync(id);
        if (category == null) return NotFound();

        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Category category)
    {
        if (id != category.Id) return NotFound();

        if (ModelState.IsValid)
        {
            _db.Update(category);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(category);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var category = await _db.Categories.FindAsync(id);
        if (category == null) return NotFound();

        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var category = await _db.Categories.FindAsync(id);
        if (category != null)
        {
            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}