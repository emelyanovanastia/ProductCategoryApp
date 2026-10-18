using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCategoryApp.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductCategoryApp.Models;

namespace ProductCategoryApp.Controllers;

public class ProductsController : Controller
{
    private readonly AppDbContext _db;   //Создаем контроллер для товаров

    public ProductsController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index(string? search, int? categoryId)  //Метод Index() принимает на вход 2 необязательных поля, иначе показываются все строки товаров
    {
        var query = _db.Products
            .Include(p => p.Category)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p => p.Name.Contains(search));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        await LoadCategoriesAsync(categoryId);
        ViewBag.Search = search;

        var products = await query
            .OrderBy(p => p.Name)
            .ToListAsync();

        return View(products);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadCategoriesAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Name,Description,Price,Stock,CategoryId")] Product product)
    {
        if (ModelState.IsValid)
        {
            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        await LoadCategoriesAsync(product.CategoryId);
        return View(product);
    }

    private async Task LoadCategoriesAsync(int? selectedId = null)
    {
        ViewBag.Categories = new SelectList(
            await _db.Categories.OrderBy(c => c.Name).ToListAsync(),
            "Id", "Name", selectedId);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var product = await _db.Products.FindAsync(id);
        if (product == null) return NotFound();

        await LoadCategoriesAsync(product.CategoryId);
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind("Id,Name,Description,Price,Stock,CategoryId")] Product product)
    {
        if (id != product.Id) return NotFound();

        if (ModelState.IsValid)
        {
            _db.Update(product);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        await LoadCategoriesAsync(product.CategoryId);
        return View(product);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var product = await _db.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (product == null) return NotFound();

        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product != null)
        {
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

}