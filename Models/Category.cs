using System.ComponentModel.DataAnnotations;

namespace Shop.Models {
  public class Category {
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "O Titulo é obrigatório")]
    [MaxLength(60, ErrorMessage = "O Titulo deve ter no máximo 60 caracteres")]
    [MinLength(3, ErrorMessage = "O Titulo deve ter no máximo 3 caracteres")]
    public string Title { get; set; }
  }
}