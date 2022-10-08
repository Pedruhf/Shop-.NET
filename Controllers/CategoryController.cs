using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Controller {
  [Route("categories")]
  public class CategoryController : ControllerBase {
    [HttpGet]
    [Route("")]
    [AllowAnonymous]
    [ResponseCache(VaryByHeader = "User Agent", Location = ResponseCacheLocation.Any, Duration = 30)]
    public async Task<ActionResult<List<Category>>> Get([FromServices]DataContext context) {
      try {
        var categories = await context.Categories.AsNoTracking().ToListAsync();
        return Ok(categories);
      } catch {
        return StatusCode(500, new { message = "Erro inesperado" });
      }
    }

    [HttpGet]
    [Route("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<Category?>> GetById(int id,[FromServices]DataContext context) {
      try {
        var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(category => category.Id == id);
        if (category == null) {
          return NotFound(new { message = "Categoria não encontrada" });
        }
        return Ok(category);
      } catch {
        return StatusCode(500, new { message = "Erro inesperado" });
      }
    }

    [HttpPost]
    [Route("")]
    [Authorize(Roles = "employee")]
    public async Task<ActionResult<Category>> Post([FromBody] Category category, [FromServices]DataContext context) {
      if (!ModelState.IsValid) {
        return BadRequest(ModelState);
      }

      try {
        context.Categories.Add(category);
        await context.SaveChangesAsync();
        return Ok(category);
      } catch {
        return StatusCode(500, new { message = "Erro ao criar categoria" });
      }
    }

    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles = "employee")]
    public async Task<ActionResult<Category>> Put(
      int id,
      [FromBody]Category category,
      [FromServices]DataContext context
    ) {
      if (category.Id != id) {
        return NotFound(new { message = "Categoria não encontrada" });
      }

      if (!ModelState.IsValid) {
        return BadRequest(ModelState);
      }

      try {
        context.Entry<Category>(category).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return Ok(category);
      } catch (DbUpdateConcurrencyException) {
        return StatusCode(500, new { message = "Este registro já foi atualizado" });
      } catch (Exception) {
        return StatusCode(500, new { message = "Erro ao atualizar categoria" });
      }
    }

    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = "employee")]
    public async Task<ActionResult<Category>> Delete(int id, [FromServices]DataContext context) {
      try {
        var category = await context.Categories.FirstOrDefaultAsync(category => category.Id == id);
        if (category == null) {
          return NotFound(new { message = "Categoria não encontrada" });
        }
        context.Categories.Remove(category);
        await context.SaveChangesAsync();
        return Ok();
      } catch (Exception) {
        return StatusCode(500, new { message = "Erro ao deletar categoria" });
      }
    }
  }
}