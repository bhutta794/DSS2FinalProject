using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using FinalProjectDss.Models;

namespace FinalProjectDss.Models;
public class Todo
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Details { get; set; }

    public string Priority { get; set; } = "medium";
    public DateOnly? DueDate { get; set; }
    public bool IsCompleted { get; set; } = false;
    public bool IsPublic { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [JsonIgnore]
    public User? User { get; set; }
}
