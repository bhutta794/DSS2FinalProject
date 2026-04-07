using System.ComponentModel.DataAnnotations;

namespace FinalProjectDss.DTOs;

public class TodoResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Details { get; set; }
    public string Priority { get; set; } = string.Empty;
    public DateOnly? DueDate { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsPublic { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateTodoRequest
{
    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Details { get; set; }

    [Required]
    public string Priority { get; set; } = "medium";

    public DateOnly? DueDate { get; set; }
    public bool IsPublic { get; set; } = false;
}

public class UpdateTodoRequest
{
    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Details { get; set; }

    [Required]
    public string Priority { get; set; } = string.Empty;

    public DateOnly? DueDate { get; set; }
    public bool IsPublic { get; set; }
    public bool IsCompleted { get; set; }
}

public class SetCompletionRequest
{
    [Required]
    public bool IsCompleted { get; set; }
}

public class PaginatedResponse<T>
{
    public List<T> Items { get; set; } = new();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
}

public class TodoQueryParams
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Status { get; set; } = "all";
    public string? Priority { get; set; }
    public DateOnly? DueFrom { get; set; }
    public DateOnly? DueTo { get; set; }
    public string? SortBy { get; set; } = "createdAt";
    public string? SortDir { get; set; } = "desc";
    public string? Search { get; set; }
}
