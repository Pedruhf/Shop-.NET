using System.ComponentModel.DataAnnotations;

namespace Shop.Models {
  public class User {
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "O usuário é obrigatório")]
    [MaxLength(20, ErrorMessage = "O usuário deve ter no máximo 20 caracteres")]
    [MinLength(3, ErrorMessage = "O usuário deve ter no máximo 3 caracteres")]
    public string Username { get; set; }

    [Required(ErrorMessage = "A senha é obrigatório")]
    [MaxLength(20, ErrorMessage = "A senha deve ter no máximo 20 caracteres")]
    [MinLength(3, ErrorMessage = "A senha deve ter no máximo 3 caracteres")]
    public string Password { get; set; }

    public string Role { get; set; }
  }
}