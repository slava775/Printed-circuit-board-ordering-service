namespace Testing.Domain.Entities;

public partial class Product
{
    public int IdProduct { get; set; }

    public int? IdCategory { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? ImageUrl { get; set; }

    public virtual Category? IdCategoryNavigation { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<PcbSpecification> PcbSpecifications { get; set; } = new List<PcbSpecification>();
}
