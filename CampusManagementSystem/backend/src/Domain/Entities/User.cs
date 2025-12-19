using CampusManagementSystem.Domain.Enums;

namespace CampusManagementSystem.Domain.Entities;

public class User
{
    public Guid UserId { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public RoleType RoleType { get; set; }
    public UserStatus Status { get; set; } = UserStatus.Active;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public virtual ICollection<IssueReport> CreatedIssues { get; set; } = new List<IssueReport>();
}
