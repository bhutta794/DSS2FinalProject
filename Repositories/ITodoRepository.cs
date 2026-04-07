using FinalProjectDss.DTOs;
using FinalProjectDss.Models;
using FinalProjectDss.DTOs;

namespace FinalProjectDss.Repositories;

public interface ITodoRepository : IGenericRepository<Todo>
{
    Task<(IEnumerable<Todo> Items, int Total)> GetUserTodosAsync(Guid userId, TodoQueryParams queryParams);
    Task<(IEnumerable<Todo> Items, int Total)> GetPublicTodosAsync(TodoQueryParams queryParams);
    Task<bool> VerifyOwnershipAsync(Guid todoId, Guid userId);
}
