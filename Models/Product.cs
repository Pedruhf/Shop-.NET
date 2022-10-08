using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Shop.Models {
  public class Product {
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "O Titulo é obrigatório")]
    [MaxLength(60, ErrorMessage = "O Titulo deve ter no máximo 60 caracteres")]
    [MinLength(3, ErrorMessage = "O Titulo deve ter no máximo 3 caracteres")]
    public string Title { get; set; }

    [MaxLength(1024, ErrorMessage = "A descrição deve ter no máximo 1024 caracteres")]
    public string Description { get; set; }

    [Required(ErrorMessage = "O preço é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "O preço deve ser maior que zero")]
    [Precision(18,2)]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "A categoria é obrigatória")]
    [Range(1, int.MaxValue, ErrorMessage = "Categoria inválida")]
    public int CategoryId { get; set; }

    public Category? Category { get; set; }

  }
}
