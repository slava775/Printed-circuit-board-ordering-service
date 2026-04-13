namespace Testing.Domain.Entities;

public partial class ConfirmationCode
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string? Email { get; set; }

    public DateTime? ExpiresIn { get; set; }

    public int? IdUser { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}
