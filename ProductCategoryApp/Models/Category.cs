using System.ComponentModel.DataAnnotations;

namespace ProductCategoryApp.Models;

public class Category
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Введите название категории")]
    [StringLength(100)]
    [Display(Name = "Категория")]
    public string Name { get; set; } = string.Empty;

    public List<Product> Products { get; set; } = new();  //В класс категорий добавляем список товаров

}