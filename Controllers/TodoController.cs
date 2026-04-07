using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FinalProjectDss.DTOs;
using FinalProjectDss.Services;

namespace FinalProjectDss.Controllers;

[ApiController]
[Route("api/todos")]
public class TodoController : ControllerBase
{
    private readonly TodoService _todoService;

    public TodoController(TodoService todoService)
    {
        _todoService = todoService;
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
        return Guid.Parse(userIdClaim!.Value);
    }

    [HttpGet("public")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPublicTodos([FromQuery] TodoQueryParams queryParams)
    {
        var result = await _todoService.GetPublicTodosAsync(queryParams);
        return Ok(result);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUserTodos([FromQuery] TodoQueryParams queryParams)
    {
        var userId = GetUserId();
        var result = await _todoService.GetUserTodosAsync(userId, queryParams);
        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateTodo([FromBody] CreateTodoRequest request)
    {
        var userId = GetUserId();
        var result = await _todoService.CreateTodoAsync(userId, request);
        return CreatedAtAction(nameof(GetTodoById), new { id = result.Id }, result);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetTodoById(Guid id)
    {
        var userId = GetUserId();
        var result = await _todoService.GetTodoByIdAsync(id, userId);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateTodo(Guid id, [FromBody] UpdateTodoRequest request)
    {
        var userId = GetUserId();
        var result = await _todoService.UpdateTodoAsync(id, userId, request);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPatch("{id}/completion")]
    [Authorize]
    public async Task<IActionResult> ToggleCompletion(Guid id, [FromBody] SetCompletionRequest request)
    {
        var userId = GetUserId();
        var result = await _todoService.ToggleCompletionAsync(id, userId, request.IsCompleted);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteTodo(Guid id)
    {
        var userId = GetUserId();
        var result = await _todoService.DeleteTodoAsync(id, userId);
        if (!result) return NotFound();
        return NoContent();
    }
}
