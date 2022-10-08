using Microsoft.AspNetCore.Mvc;
using Shop.Data;
using Shop.Models;

namespace Shop.Controller {
  [Route("v1")]
  public class HomeController : ControllerBase {
    [HttpGet]
    [Route("")]
    public async Task<ActionResult<dynamic>> Get([FromServices]DataContext context) {
      try {
        var employee = new User { Id = 1, Username = "Pedro", Password = "Pedro", Role = "employee" };
        var manager = new User { Id = 1, Username = "Freitas", Password = "Freitas", Role = "manager" };
        var category = new Category { Id = 1, Title = "Smartphones" };
        var product = new Product { Id = 1, Title = "Iphone 14 Pro Max", Category = category, CategoryId = category.Id, Description = "Modern smartphone by apple", Price = 12000 };
        context.Users.Add(employee);
        context.Users.Add(manager);
        context.Categories.Add(category);
        context.Products.Add(product);
        await context.SaveChangesAsync();
        return Ok(new { message = "Dados configurados" });
      } catch {
        return StatusCode(500, new { message = "Erro inesperado" });
      }
    }
  }
}