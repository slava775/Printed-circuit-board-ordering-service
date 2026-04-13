namespace Testing.Domain.Entities;

public partial class UserSession
{
    public int IdSession { get; set; }

    public int? IdUser { get; set; }

    public string? RefreshToken { get; set; }

    public string? CreatedAt { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public bool? IsActive { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}
