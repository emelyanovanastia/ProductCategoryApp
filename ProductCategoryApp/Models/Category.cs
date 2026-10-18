using System.ComponentModel.DataAnnotations;

namespace ProductCategoryApp.Models;

public class Category
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Введите название категории")]
    [StringLength(100)]
    [Display(Name = "Категория")]
    public string Name { get; set; } = string.Empty;
}