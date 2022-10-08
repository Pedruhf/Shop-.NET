using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using Shop.Services;

namespace Shop.Controller {
  [Route("users")]
  public class UserController : ControllerBase {
    [HttpGet]
    [Authorize(Roles = "manager")]
    public async Task<ActionResult<List<User>>> Get([FromServices]DataContext context) {
      try {
        var users = await context.Users.AsNoTracking().ToListAsync();
        return Ok(users);
      } catch {
        return StatusCode(500, new { message = "Erro inesperado" });
      }
    }

    [HttpPost]
    [Route("")]
    [AllowAnonymous]
    // [Authorize(Roles = "manager")]
    public async Task<ActionResult<User>> Create([FromBody] User user, [FromServices]DataContext context) {
      if (!ModelState.IsValid) {
        return BadRequest(ModelState);
      }

      try {
        user.Role = "employee";
        context.Users.Add(user);
        await context.SaveChangesAsync();
        user.Password = null;
        return Ok(user);
      } catch {
        return StatusCode(500, new { message = "Erro inesperado" });
      }
    }

    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles = "manager")]
    public async Task<ActionResult<User>> Update(int id, [FromBody] User user, [FromServices]DataContext context) {
      if (user.Id != id) {
        return NotFound(new { message = "Usuário não encontrado" });
      }
      if (!ModelState.IsValid) {
        return BadRequest(ModelState);
      }
      try {
        context.Entry(user).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return Ok(user);
      } catch {
        return StatusCode(500, new { message = "Erro inesperado" });
      }
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<dynamic>> Authenticate(
      [FromBody] User model,
      [FromServices] DataContext context
    ) {
      var user = await context.Users.AsNoTracking().Where(user => user.Username == model.Username && user.Password == model.Password).FirstOrDefaultAsync();
      if (user == null) {
        return NotFound(new { message = "Usuário ou senha inválidos"});
      }
      var token = TokenService.GenerateToken(user);
      return new {
        user,
        token,
      };
    }
  }
}