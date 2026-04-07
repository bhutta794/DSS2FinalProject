using FinalProjectDss.DTOs;
using FinalProjectDss.Models;
using FinalProjectDss.Repositories;
using FinalProjectDss.DTOs;

namespace FinalProjectDss.Services;

public class TodoService
{
    private readonly ITodoRepository _todoRepository;

    public TodoService(ITodoRepository todoRepository) => _todoRepository = todoRepository;

    public async Task<TodoResponse> CreateTodoAsync(Guid userId, CreateTodoRequest request)
    {
        var todo = new Todo
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Title = request.Title,
            Details = request.Details,
            Priority = request.Priority,
            DueDate = request.DueDate,
            IsPublic = request.IsPublic
        };
        var created = await _todoRepository.AddAsync(todo);
        return MapToResponse(created);
    }

    public async Task<PaginatedResponse<TodoResponse>> GetUserTodosAsync(Guid userId, TodoQueryParams queryParams)
    {
        var (items, total) = await _todoRepository.GetUserTodosAsync(userId, queryParams);
        return new PaginatedResponse<TodoResponse>
        {
            Items = items.Select(MapToResponse).ToList(),
            Page = queryParams.Page,
            PageSize = queryParams.PageSize,
            TotalItems = total,
            TotalPages = (int)Math.Ceiling((double)total / queryParams.PageSize)
        };
    }

    public async Task<PaginatedResponse<TodoResponse>> GetPublicTodosAsync(TodoQueryParams queryParams)
    {
        var (items, total) = await _todoRepository.GetPublicTodosAsync(queryParams);
        return new PaginatedResponse<TodoResponse>
        {
            Items = items.Select(MapToResponse).ToList(),
            Page = queryParams.Page,
            PageSize = queryParams.PageSize,
            TotalItems = total,
            TotalPages = (int)Math.Ceiling((double)total / queryParams.PageSize)
        };
    }

    public async Task<TodoResponse?> GetTodoByIdAsync(Guid id, Guid userId)
    {
        var todo = await _todoRepository.GetByIdAsync(id);
        return todo == null || todo.UserId != userId ? null : MapToResponse(todo);
    }

    public async Task<TodoResponse?> UpdateTodoAsync(Guid id, Guid userId, UpdateTodoRequest request)
    {
        var todo = await _todoRepository.GetByIdAsync(id);
        if (todo == null || todo.UserId != userId) return null;

        todo.Title = request.Title;
        todo.Details = request.Details;
        todo.Priority = request.Priority;
        todo.DueDate = request.DueDate;
        todo.IsPublic = request.IsPublic;
        todo.IsCompleted = request.IsCompleted;
        todo.UpdatedAt = DateTime.UtcNow;

        await _todoRepository.UpdateAsync(todo);
        return MapToResponse(todo);
    }

    public async Task<bool> DeleteTodoAsync(Guid id, Guid userId)
    {
        var todo = await _todoRepository.GetByIdAsync(id);
        if (todo == null || todo.UserId != userId) return false;
        await _todoRepository.DeleteAsync(todo);
        return true;
    }

    public async Task<TodoResponse?> ToggleCompletionAsync(Guid id, Guid userId, bool isCompleted)
    {
        var todo = await _todoRepository.GetByIdAsync(id);
        if (todo == null || todo.UserId != userId) return null;

        todo.IsCompleted = isCompleted;
        todo.UpdatedAt = DateTime.UtcNow;
        await _todoRepository.UpdateAsync(todo);
        return MapToResponse(todo);
    }

    private static TodoResponse MapToResponse(Todo todo) => new()
    {
        Id = todo.Id,
        Title = todo.Title,
        Details = todo.Details,
        Priority = todo.Priority,
        DueDate = todo.DueDate,
        IsCompleted = todo.IsCompleted,
        IsPublic = todo.IsPublic,
        CreatedAt = todo.CreatedAt,
        UpdatedAt = todo.UpdatedAt
    };
}
