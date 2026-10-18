using System.ComponentModel.DataAnnotations;

namespace ProductCategoryApp.Models;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Введите название товара")]
    [StringLength(200)]
    [Display(Name = "Название")]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000)]
    [Display(Name = "Описание")]
    public string? Description { get; set; }

    [Range(0.01, 1000000, ErrorMessage = "Цена должна быть больше 0")]
    [Display(Name = "Цена")]
    public decimal Price { get; set; }

    [Range(0, 100000, ErrorMessage = "Количество от 0 до 100000")]
    [Display(Name = "Остаток")]
    public int Stock { get; set; }

    [Display(Name = "Категория")]
    public int CategoryId { get; set; }   //Внешний ключ для связи товаров и категорий

    public Category? Category { get; set; }
}