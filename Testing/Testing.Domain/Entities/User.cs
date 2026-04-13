namespace Testing.Domain.Entities;

public partial class User
{
    public int IdUser { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? PasswordHash { get; set; }

    public bool? EmailConfirmed { get; set; }

    public int? IdRole { get; set; }

    public int? IdCountry { get; set; }

    public virtual ICollection<ConfirmationCode> ConfirmationCodes { get; set; } = new List<ConfirmationCode>();

    public virtual Country? IdCountryNavigation { get; set; }

    public virtual Role? IdRoleNavigation { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<UserSession> UserSessions { get; set; } = new List<UserSession>();
}
