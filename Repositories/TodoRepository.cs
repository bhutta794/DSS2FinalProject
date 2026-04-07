using Microsoft.EntityFrameworkCore;
using FinalProjectDss.Data;
using FinalProjectDss.DTOs;
using FinalProjectDss.Models;
using FinalProjectDss.DTOs;

namespace FinalProjectDss.Repositories;

public class TodoRepository : GenericRepository<Todo>, ITodoRepository
{
    public TodoRepository(ApplicationDbContext context) : base(context) { }

    public async Task<(IEnumerable<Todo> Items, int Total)> GetUserTodosAsync(Guid userId, TodoQueryParams queryParams)
    {
        var query = _dbSet.Where(t => t.UserId == userId);

        if (queryParams.Status == "completed") query = query.Where(t => t.IsCompleted);
        else if (queryParams.Status == "active") query = query.Where(t => !t.IsCompleted);
        if (!string.IsNullOrEmpty(queryParams.Priority)) query = query.Where(t => t.Priority == queryParams.Priority);
        if (queryParams.DueFrom.HasValue) query = query.Where(t => t.DueDate >= queryParams.DueFrom);
        if (queryParams.DueTo.HasValue) query = query.Where(t => t.DueDate <= queryParams.DueTo);
        if (!string.IsNullOrEmpty(queryParams.Search)) query = query.Where(t => t.Title.Contains(queryParams.Search) || (t.Details != null && t.Details.Contains(queryParams.Search)));

        var total = await query.CountAsync();
        var items = await query.Skip((queryParams.Page - 1) * queryParams.PageSize).Take(queryParams.PageSize).ToListAsync();
        return (items, total);
    }

    public async Task<(IEnumerable<Todo> Items, int Total)> GetPublicTodosAsync(TodoQueryParams queryParams)
    {
        var query = _dbSet.Where(t => t.IsPublic);

        if (queryParams.Status == "completed") query = query.Where(t => t.IsCompleted);
        else if (queryParams.Status == "active") query = query.Where(t => !t.IsCompleted);
        if (!string.IsNullOrEmpty(queryParams.Priority)) query = query.Where(t => t.Priority == queryParams.Priority);
        if (queryParams.DueFrom.HasValue) query = query.Where(t => t.DueDate >= queryParams.DueFrom);
        if (queryParams.DueTo.HasValue) query = query.Where(t => t.DueDate <= queryParams.DueTo);
        if (!string.IsNullOrEmpty(queryParams.Search)) query = query.Where(t => t.Title.Contains(queryParams.Search) || (t.Details != null && t.Details.Contains(queryParams.Search)));

        var total = await query.CountAsync();
        var items = await query.Skip((queryParams.Page - 1) * queryParams.PageSize).Take(queryParams.PageSize).ToListAsync();
        return (items, total);
    }

    public async Task<bool> VerifyOwnershipAsync(Guid todoId, Guid userId)
        => await _dbSet.AnyAsync(t => t.Id == todoId && t.UserId == userId);
}
