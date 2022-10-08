using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Controller {
  [Route("products")]
  public class ProductController : ControllerBase {
    [HttpGet]
    [Route("")]
    [AllowAnonymous]
    public async Task<ActionResult<List<Product>>> Get([FromServices]DataContext context) {
      try {
        var products = await context.Products.Include(product => product.Category).AsNoTracking().ToListAsync();
        return Ok(products);
      } catch {
        return StatusCode(500, new { message = "Erro inesperado" });
      }
    }

    [HttpGet]
    [Route("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<Product?>> GetById(int id,[FromServices]DataContext context) {
      try {
        var Product = await context.Products.Include(product => product.Category).AsNoTracking().FirstOrDefaultAsync(Product => Product.Id == id);
        if (Product == null) {
          return NotFound(new { message = "Produto não encontrado" });
        }
        return Ok(Product);
      } catch {
        return StatusCode(500, new { message = "Erro inesperado" });
      }
    }

    [HttpGet]
    [Route("categories/{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<Product?>> GetByCategory(int id, [FromServices]DataContext context) {
      try {
        var Product = await context.Products.Include(product => product.Category).AsNoTracking().Where(product => product.CategoryId == id).ToListAsync();
        if (Product == null) {
          return NotFound(new { message = "Produto não encontrado" });
        }
        return Ok(Product);
      } catch {
        return StatusCode(500, new { message = "Erro inesperado" });
      }
    }

    [HttpPost]
    [Route("")]
    [Authorize(Roles = "employee")]
    public async Task<ActionResult<Product>> Post([FromBody] Product Product, [FromServices]DataContext context) {
      if (!ModelState.IsValid) {
        return BadRequest(ModelState);
      }

      try {
        context.Products.Add(Product);
        await context.SaveChangesAsync();
        return Ok(Product);
      } catch {
        return StatusCode(500, new { message = "Erro inesperado" });
      }
    }

    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles = "employee")]
    public async Task<ActionResult<Product>> Put(
      int id,
      [FromBody]Product Product,
      [FromServices]DataContext context
    ) {
      if (Product.Id != id) {
        return NotFound(new { message = "Produto não encontrada" });
      }

      if (!ModelState.IsValid) {
        return BadRequest(ModelState);
      }

      try {
        context.Entry<Product>(Product).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return Ok(Product);
      } catch (DbUpdateConcurrencyException) {
        return StatusCode(500, new { message = "Este registro já foi atualizado" });
      } catch (Exception) {
        return StatusCode(500, new { message = "Erro inesperado" });
      }
    }

    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = "employee")]
    public async Task<ActionResult<Product>> Delete(int id, [FromServices]DataContext context) {
      try {
        var Product = await context.Products.FirstOrDefaultAsync(Product => Product.Id == id);
        if (Product == null) {
          return NotFound(new { message = "Produto não encontrada" });
        }
        context.Products.Remove(Product);
        await context.SaveChangesAsync();
        return Ok();
      } catch (Exception) {
        return StatusCode(500, new { message = "Erro inesperado" });
      }
    }
  }
}