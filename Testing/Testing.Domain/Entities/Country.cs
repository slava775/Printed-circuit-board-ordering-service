namespace Testing.Domain.Entities;

public partial class Country
{
    public int IdCountry { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
