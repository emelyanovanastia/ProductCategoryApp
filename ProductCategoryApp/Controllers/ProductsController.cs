using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCategoryApp.Data;

namespace ProductCategoryApp.Controllers;

public class ProductsController : Controller
{
    private readonly AppDbContext _db;   //Создаем контроллер для товаров

    public ProductsController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _db.Products
            .Include(p => p.Category)  //Подгружаем связанную с товаром категорию
            .OrderBy(p => p.Name)
            .ToListAsync();

        return View(products);
    }
}